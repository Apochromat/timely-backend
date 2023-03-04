using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models
{
    public class LessonName
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
