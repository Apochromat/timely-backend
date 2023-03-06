using timely_backend.Models.Enum;
using System.ComponentModel;

namespace timely_backend.Models.DTO;

public class TokenResponse {
    [DisplayName("token")] public string? Token { get; set; }
    [DisplayName("name")] public string? Email { get; set; }
    [DisplayName("role")] public IList<string>? Role { get; set; }
    [DisplayName("isEmailConfirmed")] public Boolean IsEmailConfirmed { get; set; }
    [DisplayName("group")] public GroupDTO? Group { get; set; }
    [DisplayName("teacher")] public TeacherDTO? Teacher { get; set; }
}