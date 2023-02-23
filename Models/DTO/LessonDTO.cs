using System.ComponentModel.DataAnnotations;
using static ServiceStack.LicenseUtils;

namespace timely_backend.Models.DTO
{
    public class LessonDTO
    {
        [Required(ErrorMessage = "Необходимо указать название пары")]

        public LessonName Name { get; set; }
        [Required(ErrorMessage = "Необходимо указать тип пары")]
        public LessonTag Tag { get; set; }
        [Required(ErrorMessage = "Необходимо указать номер группы")]

        public Group Group { get; set; }
        [Required(ErrorMessage = "Необходимо указать учителя")]

        public Teacher Teacher { get; set; }
        [Required(ErrorMessage = "Необходимо указать временной интервал")]
        public TimeInterval TimeInterval { get; set; }
        [Required(ErrorMessage = "Необходимо указать дату")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Необходимо указать номер аудитории")]
        public Classroom Classroom { get; set; }
        public Guid? Id { get; set; }
        public Guid? ChainId { get; set; }
    }
}
