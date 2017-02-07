using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageImport.Models
{
    [Table("Beholder.MediaType")]
    public partial class MediaType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int? SortOrder { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }

        public int CreatedUserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DateModified { get; set; }

        public int? ModifiedUserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DateDeleted { get; set; }

        public int? DeletedUserId { get; set; }
    }
}