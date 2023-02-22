using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models.DTO
{
    public class TimeIntervalDTO
    {
        [Required(ErrorMessage = "Необходимо указать номер группы")]
        [MinLength(3)]
        [MaxLength(64)]
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string? Timezone { get; set; }
        public Guid? Id { get; set; }
    }
}
