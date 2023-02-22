using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models.DTO
{
    public class DomainDTO
    {
        [Url(ErrorMessage = "Необходимо указать полный URL")]
        [Required(ErrorMessage = "Необходимо указать полный URL")]
        public string Url { get; set; }
    }
}
