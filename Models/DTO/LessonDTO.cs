using System.ComponentModel.DataAnnotations;
using static ServiceStack.LicenseUtils;

namespace timely_backend.Models.DTO
{
    public class LessonDTO
    {
        [Required(ErrorMessage = "Необходимо указать название пары")]

        public LessonNameDTO Name { get; set; }
        [Required(ErrorMessage = "Необходимо указать тип пары")]
        public LessonTagDTO Tag { get; set; }
        [Required(ErrorMessage = "Необходимо указать номер группы")]

        public GroupDTO Group { get; set; }
        [Required(ErrorMessage = "Необходимо указать учителя")]

        public TeacherDTO Teacher { get; set; }
        [Required(ErrorMessage = "Необходимо указать временной интервал")]
        public TimeIntervalDTO TimeInterval { get; set; }
        [Required(ErrorMessage = "Необходимо указать дату")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Необходимо указать номер аудитории")]
        public ClassroomDTO Classroom { get; set; }
        public Guid? Id { get; set; }
        public Guid? ChainId { get; set; }
    }
}
