using static ServiceStack.LicenseUtils;
using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models.DTO
{
    public class GroupDTO
    {

        [Required(ErrorMessage = "Необходимо указать номер группы")]
        [MinLength(3)]
        [MaxLength(64)]
        public string Name { get; set; }
    }
}
