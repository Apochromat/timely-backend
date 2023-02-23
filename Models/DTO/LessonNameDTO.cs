using static ServiceStack.LicenseUtils;
using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models.DTO
{
    public class LessonNameDTO
    {
        [Required(ErrorMessage = "Необходимо указать название занятия")]
        [MinLength(3)]
        [MaxLength(64)]
        public string Name { get; set; }
        public Guid? Id { get; set; }
    }
}
