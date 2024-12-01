using BusinessCardManagerAPI.Data.Models.Contract;

namespace BusinessCardManagerAPI.Data.Models
{
    public class BusinessCard : ISoftDeletable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? PhotoBase64 { get; set; }
        public string Address { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DateDeleted { get; set; } 
    }
}
