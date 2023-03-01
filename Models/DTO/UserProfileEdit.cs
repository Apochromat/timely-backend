using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models.DTO;

public class UserProfileEdit {
    [Required]
    [DisplayName("fullName")]
    [RegularExpression(@"^([A-ZА-ЯЁ][a-zа-яё]+[\s]?){2,3}$",
        ErrorMessage =
            "ФИО должно состоять из 2-3 слов, начинаться с заглавной буквы и содержать только латинские, кириллические символы, пробелы")]
    public String? FullName { get; set; } = "";
}