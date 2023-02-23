using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models.DTO;

public class UserPasswordEdit {
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9\-_!@#№$%^&?*+=(){}[\]<>~]+$")]
    [MinLength(8)]
    [MaxLength(64)]
    [DisplayName("currentPassword")]
    public String? CurrentPassword { get; set; } = "";

    [Required]
    [RegularExpression(@"^[a-zA-Z0-9\-_!@#№$%^&?*+=(){}[\]<>~]+$")]
    [MinLength(8)]
    [MaxLength(64)]
    [DisplayName("newPassword")]
    public String? NewPassword { get; set; } = "";
}