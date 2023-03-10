using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models
{
    public class Lesson
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public LessonName Name { get; set; }
        public LessonTag Tag { get; set; }
        public IList<Group> Group { get; set; } = new List<Group>();
        public Teacher Teacher { get; set; }
        public TimeInterval TimeInterval { get; set; }
        public Classroom Classroom { get; set; }
        public DateTime Date { get; set; }
        public Guid? ChainId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsReadOnly { get; set; } = false;
       
    }
}
