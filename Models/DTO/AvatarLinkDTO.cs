using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models.DTO;

public class AvatarLinkDTO {
    [DisplayName("avatarLink")] [Required] [RegularExpression(@"\bhttps?:\/\/\S+\.(?:jpg|jpeg|gif|png|bmp)\b")] public String AvatarLink { get; set; }
}