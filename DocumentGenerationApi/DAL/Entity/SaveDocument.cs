using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentGenerationApi.DAL.Entity
{
    [Table("documentTbl")]
    public class SaveDocument
    {
        public string ObjectCode { get; set; }
        public string? ReferenceType { get; set; }
        public string? ReferenceNumber { get; set; }
        public Byte[]? Content { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string? LanguageCode { get; set; }
        public string? CreatedUser { get; set; }
        [Key]
        public DateTime? CreatedDateTime { get; set; }
        public bool isDeleted { get; set; }



    }
}
