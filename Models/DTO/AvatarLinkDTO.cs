using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models.DTO;

public class AvatarLinkDTO {
    [DisplayName("avatarLink")] [Required] [RegularExpression(@"\bhttps?:\/\/\S+\.(?:jpg|jpeg|png|bmp)\b", ErrorMessage = "Неправильный тип ссылки")] public String AvatarLink { get; set; }
}