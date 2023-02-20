using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentGenerationApi.DAL.Entity
{
    [Table("userTbl")]
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? PolicyNumber { get; set; }
        public int Age { get; set; }
        public int Salary { get; set; }
        public string? Occupation { get; set; }
        public DateTime PolicyExpiryDate { get; set; }
        public string? Email { get; set; }
        public string? ProductCode { get; set; }
    }
}
