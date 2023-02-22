using static ServiceStack.LicenseUtils;
using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models.DTO
{
    public class ClassroomDTO
    {
        [Required(ErrorMessage = "Необходимо указать номер аудитории")]
        [MinLength(3)]
        [MaxLength(64)]
        public string Name { get; set; }
    }
}
