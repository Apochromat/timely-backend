using static ServiceStack.LicenseUtils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace timely_backend.Models.DTO
{
    public class TeacherDTO
    {
        [Required(ErrorMessage = "Необходимо указать ФИО")]
        [MinLength(1)]
        [MaxLength(64)]
        [DisplayName("fullName")]
        [RegularExpression(@"^([А-ЯЁ][а-яё]+[\-\s]?){2,3}$",
        ErrorMessage = "ФИО должно состоять из 2-3 слов, начинаться с заглавной буквы и содержать только кириллические символы, дефисы и пробелы")]
        public string Name { get; set; }
        public Guid? Id { get; set; }
    }
}
