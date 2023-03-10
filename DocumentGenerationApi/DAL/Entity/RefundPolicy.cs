using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentGenerationApi.DAL.Entity
{
    [Table("tblRefundPolicy")]
    public class RefundPolicy
    {
        [Key]
        public int? Id { get; set; }
        public string PolicyNumber { get; set; }
        public string? TemplateCode { get; set; }
        public string? TransactionNumber { get; set; }
        public string ChannelCode { get; set; }
        public string? ClientId { get; set; }
        public string? Destination { get; set; } 
        public string? Subject { get; set; } 
        public string? Body { get; set; } 
        public string? CreatedUserId { get; set; } 
    }
}
