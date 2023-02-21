using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentGenerationApi.DAL.Entity
{
    [Table("DocumentTemplate")]
    public class Document
    {
        [Key]
        public int Id { get; set; }
        public string DocumentCode { get; set; }
        public string Filename { get; set; }
        public string? Content { get; set; }
        public Byte[]? ContentBinary { get; set; }
        public bool isDeleted { get; set; }
        public string CreatedUser { get; set; }
        public DateTime? CreatedDateTime { get; set;}
        public string? ModifiedUser { get;set;}
        public DateTime? ModifiedDateTime { get; set; }

    }
}
