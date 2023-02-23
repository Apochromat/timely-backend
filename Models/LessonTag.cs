using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models
{
    public class LessonTag
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
    }
}
