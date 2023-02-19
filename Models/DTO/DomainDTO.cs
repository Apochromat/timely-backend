using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models.DTO
{
    public class DomainDTO
    {
        [Url]
       public string Url { get; set; }
    }
}
