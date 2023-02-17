using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models
{
    public class Teacher
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
    }
}
