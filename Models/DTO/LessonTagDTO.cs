using static ServiceStack.LicenseUtils;
using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models.DTO
{
    public class LessonTagDTO
    {
        [Required(ErrorMessage = "Необходимо указать тип занятия")]
        [MinLength(3)]
        [MaxLength(64)]
        public string Name { get; set; }
    }
}
