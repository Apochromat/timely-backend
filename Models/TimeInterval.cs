using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models
{
    public class TimeInterval
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Timezone { get; set; }
        //theDate.ToString("yyyy-MM-dd HH':'mm':'ss")


    }
}
