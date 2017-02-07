using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageImport.Models
{
    [Table("Beholder.MediaPublishedContext")]
    public partial class MediaPublishedContext
    {
        public int Id { get; set; }

        public Guid? FileStreamID { get; set; }

        public int MimeTypeId { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }

        [StringLength(10)]
        public string DocumentExtension { get; set; }

        public byte[] ContextText { get; set; }

        public int? MediaPublishedId { get; set; }

        //public virtual MediaPublished MediaPublished { get; set; }
    }
}