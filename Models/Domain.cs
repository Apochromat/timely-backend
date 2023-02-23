using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models
{
    public class Domain
    {
        [Key]
       public Guid Id { get; set; } = Guid.NewGuid();
        [Url]
       public string Url { get; set; }
    }
}
