using ImageImport.DataAccess;
using ImageImport.Models;
using System;
using System.Data.OleDb;
using System.IO;

namespace ImageImport
{
    class Program
    {

        private static void Main(string[] args)
        {
            Console.Write("Enter file path or exit to quit: ");
            var path = Console.ReadLine();

            if (path == "exit") Environment.Exit(0);

            Console.Write("Which environment (P)roduction or (T)est: ");
            var env = Console.ReadLine();

            var contextName = env?.ToUpper() == "P" ? "Prod" : "Test";

            Console.WriteLine($"Executing import on {contextName}");

            do
            {
                string connString = $"Provider=Microsoft.JET.OLEDB.4.0;Data Source={path}\\index.mdb";
                using (OleDbConnection connection = new OleDbConnection(connString))
                {
                    connection.Open();
                    OleDbDataReader reader = null;
                    OleDbCommand command = new OleDbCommand("SELECT * from  tblOcr", connection);

                    var userId = 1;
                    var confidentialityTypeId = 4;

                    var fi = new FileInfo(connection.DataSource);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string filename = fi.Directory + (string)reader["filename"];
                        using (var db = BeholderContext.Create(contextName))
                        {
                            if (!File.Exists(filename))
                            {
                                Console.WriteLine($"File missing {filename}");
                            }
                            else
                            {
                                Console.WriteLine(
                                    $"Creating {reader["Publication_Info"]} {reader["Volume"]} : {reader["Publication_Date"]}");
                                var pubContext = new MediaPublishedContext()
                                {
                                    MimeTypeId = 7,
                                    FileName = reader["Publication_Info"].ToString(),
                                    DocumentExtension = ".pdf",
                                    FileStreamID = Guid.NewGuid(),
                                    ContextText = File.ReadAllBytes(filename)
                                };
                                DateTime PubDate;
                                DateTime.TryParse(reader["Publication_Date"].ToString(), out PubDate);
                                var pub = new MediaPublished()
                                {
                                    MediaTypeId = confidentialityTypeId,
                                    PublishedTypeId = confidentialityTypeId,
                                    Name = reader["Publication_Info"].ToString(),
                                    DatePublished = Convert.ToDateTime(PubDate),
                                    DateReceived = Convert.ToDateTime(PubDate),
                                    MovementClassId = Convert.ToInt32(reader["Move_Class"]),
                                    ConfidentialityTypeId = confidentialityTypeId,
                                    CreatedUserId = userId,
                                    ModifiedUserId = userId,
                                    DateCreated = DateTime.Now,
                                    DateModified = DateTime.Now,
                                    MediaPublishedContext = pubContext
                                };

                                db.MediaPublished.Add(pub);
                                db.SaveChanges();

                                pubContext.MediaPublishedId = pub.Id;
                                db.SaveChanges();
                            }
                        }
                    }
                }
                Console.Write("Enter file path or exit to quit: ");
                path = Console.ReadLine();
                if (path == "exit") Environment.Exit(0);

                Console.Write("Which environment (P)roduction or (T)est: ");
                env = Console.ReadLine();

                contextName = env?.ToUpper() == "P" ? "Prod" : "Test";

                Console.WriteLine($"Executing import on {contextName}");

            } while (path != "exit");

            Console.Write("Finished");
            Console.ReadLine();
        }

    }
}
