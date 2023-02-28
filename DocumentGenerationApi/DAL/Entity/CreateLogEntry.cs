using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentGenerationApi.DAL.Entity
{
    [Table("LogTbl")]
    public class CreateLogEntry
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public bool isSent { get; set; }
    }
}
