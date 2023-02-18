using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models.DTO;

public class UserRegisterModel {
    /// <summary>
    /// Users Surname, Name and optional Patronymic
    /// </summary>
    [Required]
    [RegularExpression(@"^[а-яА-ЯёЁa-zA-Z ]+$")]
    [MinLength(1)]
    [MaxLength(64)]
    [DisplayName("fullName")]
    public String? FullName { get; set; }

    /// <summary>
    /// Users Password. Must Contain at least one non alphanumeric character, at least one digit ('0'-'9'),
    /// at least one uppercase ('A'-'Z')
    /// </summary>
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9\-_!@#№$%^&?*+=(){}[\]<>~]+$")]
    [MinLength(8)]
    [MaxLength(64)]
    [DisplayName("password")]
    public String? Password { get; set; }

    /// <summary>
    /// Users Email
    /// </summary>
    [Required]
    [EmailAddress]
    [DisplayName("email")]
    public String? Email { get; set; }
}