using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models.DTO;

public class LoginCredentials {
    [Required]
    [EmailAddress]
    [MinLength(1)]
    [DisplayName("email")]
    public String Email { get; set; } = "";

    [Required]
    [RegularExpression(@"^[a-zA-Z0-9\-_!@#№$%^&?*+=(){}[\]<>~]+$")]
    [MinLength(8)]
    [MaxLength(64)]
    [DisplayName("password")]
    public String Password { get; set; } = "";
}