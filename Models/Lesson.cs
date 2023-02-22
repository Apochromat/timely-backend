using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models
{
    public class Lesson
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public LessonName Name { get; set; }
        public LessonTag Tag { get; set; }
        public Group Group { get; set; }
        public Teacher Teacher { get; set; }
        public TimeInterval TimeInterval { get; set; }
        public Guid? ChainId { get; set; }
       
    }
}
