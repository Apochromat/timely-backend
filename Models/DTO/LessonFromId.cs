using static ServiceStack.LicenseUtils;
using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models.DTO
{
    public class LessonFromId
    {
        [Required(ErrorMessage = "Необходимо указать id пары")]

        public Guid NameId { get; set; }
        [Required(ErrorMessage = "Необходимо указать id типa пары")]
        public Guid TagId { get; set; }
        [Required(ErrorMessage = "Необходимо указать id группы")]

        public IList<Guid> GroupId { get; set; }
        [Required(ErrorMessage = "Необходимо указать id учителя")]

        public Guid TeacherId { get; set; }
        [Required(ErrorMessage = "Необходимо указать id интервалa")]
        public Guid TimeIntervalId { get; set; }
        [Required(ErrorMessage = "Необходимо указать дату")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Необходимо указать id аудитории")]
        public Guid ClassroomId { get; set; }
        public Guid? ChainId { get; set; }
    }
}
