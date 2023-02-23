using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models.DTO
{
    public class TimeIntervalDTO
    {
        [Required(ErrorMessage = "Необходимо указать время начала урока")]
        [DataType(DataType.Time)]
        public TimeOnly StartTime { get; set; }
        [Required(ErrorMessage = "Необходимо указать время конца урока")]
        [DataType(DataType.Time)]
        public TimeOnly EndTime { get; set; }
        public Guid? Id { get; set; }
    }
}
