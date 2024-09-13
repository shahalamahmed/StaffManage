using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace StaffManage.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [DisplayName("Mobile Number")]
        public string Mobile { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("Photo")]
        public string PhotoFile { get; set; }
    }
}
