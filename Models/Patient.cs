using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnarkanHospital.Models
{
    public class Patient
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Da5al Asmak ya 8aly Mad7aknash")]
        [MinLength(4, ErrorMessage = "Name Must Not be less than 4 Characters")]
        [MaxLength(50, ErrorMessage = "Name Must not Exceed 50 Characters")]
        [Display(Name = "Full Name")]

        public string Name { get; set; }

        [Required(ErrorMessage = "Age is Required")]

        public int Age { get; set; }
        [Required(ErrorMessage = "Health Condition Must Be Determined")]

        public string HealthProblem { get; set; }
        [Required(ErrorMessage = "Must be Determined to Choose The Right Action")]

        public bool Critical { get; set; }

        [DataType(DataType.DateTime)]

        public DateTime ArrivedAt { get; set; }

        [DataType(DataType.DateTime)]

        public DateTime LeavingAt { get; set; }

        [ValidateNever]
        public DateTime CreatedAt { get; set; }
        [ValidateNever]

        public DateTime LastUpdatedAt { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Choose a Valid Doctor")]
        [DisplayName("Doctor")]
        public int DoctorId { get; set; }
        [ValidateNever]
        public Doctor Doctor { get; set; }
    }
}
