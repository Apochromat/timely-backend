using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models.DTO
{
    public class DomainDTO
    {
        [Required(ErrorMessage = "Необходимо указать полный URL")]
        public string Url { get; set; }
        public Guid? Id { get; set; }
    }
}
