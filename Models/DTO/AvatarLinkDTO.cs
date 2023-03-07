using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models.DTO;

public class AvatarLinkDTO {
    [DisplayName("avatarLink")] [Required] public String AvatarLink { get; set; }
}