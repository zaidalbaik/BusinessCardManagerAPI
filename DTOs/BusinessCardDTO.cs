using System.ComponentModel.DataAnnotations;

namespace BusinessCardManagerAPI.DTOs
{
    public class BusinessCardDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Email length must be less than 255 and greater 6 characters.")]
        public string Email { get; set; }

        [Required]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [MaxLength(20, ErrorMessage = "Phone length must be less than 20")]
        public string Phone { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(10)]
        public string Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(500)]
        public string Address { get; set; }
         
        public string? PhotoBase64 { get; set; }
    }
}
