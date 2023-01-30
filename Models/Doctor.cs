using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnarkanHospital.Models
{
    public class Doctor
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Da5al Asmak ya 8aly Mad7aknash")]
        [MinLength(4, ErrorMessage = "Name Must Not be less than 4 Characters")]
        [MaxLength(50, ErrorMessage = "Name Must not Exceed 50 Characters")]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [DisplayName("Speciality")]
        [Required(ErrorMessage = "You have to provide a valid Speciality")]


        public string Speciality { get; set; }

        public int Age { get; set; }

        [DataType(DataType.EmailAddress)]

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid Email Address")]

        public string Email { get; set; }
        [NotMapped]
        [Compare("Email", ErrorMessage = "Email Not Matched")]
        [DataType(DataType.EmailAddress)]

        public string ConfirmEmail { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Not Match")]

        public string ConfirmPassword { get; set; }
        [DataType(DataType.DateTime)]

        [ValidateNever]
        [DisplayName("Image")]
        public string ImgUrl { get; set; }
        [ValidateNever]
        [DisplayName("Resume")]

        public string CvUrl { get; set; }


        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone")]

        public int PhoneNumber { get; set; }

        public bool Availability { get; set; }

        [DataType(DataType.Currency)]
        [Range(2500, 2000000, ErrorMessage = "Salary must be between EGP 2500 and EGP 2M")]

        public int Salary { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime HiringDateTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [DataType(DataType.Time)]
        public DateTime AttendanceTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime LeavingTime { get; set; }
        [ValidateNever]
        public DateTime CreatedAt { get; set; }
        [ValidateNever]

        public DateTime LastUpdatedAt { get; set; }

        [ValidateNever]

        public ICollection<Patient> Patient { get; set; }
    }
}
