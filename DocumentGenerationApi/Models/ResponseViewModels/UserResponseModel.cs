namespace DocumentGenerationApi.Models.ResponseViewModels
{
    public class UserResponseModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? PolicyNumber { get; set; }
        public int Age { get; set; }
        public int Salary { get; set; }
        public string? Occupation { get; set; }
        public DateTime PolicyExpiryDate { get; set; }
        public string? ProductCode { get; set;}
        public string? Email { get; set; }
    }
}
