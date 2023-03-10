using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models
{
    public class TimeInterval
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsDeleted { get; set; } = false;


    }
}
