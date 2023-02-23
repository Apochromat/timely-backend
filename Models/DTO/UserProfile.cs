using System.ComponentModel;

namespace timely_backend.Models.DTO;

public class UserProfile {
    [DisplayName("fullName")] public String? FullName { get; set; } = "";
    [DisplayName("userName")] public String? UserName { get; set; } = "";
    [DisplayName("email")] public String? Email { get; set; } = "";
    [DisplayName("group")] public Group? Group { get; set; }
    [DisplayName("teacher")] public Teacher? Teacher { get; set; }
    [DisplayName("roles")] public List<String> Roles { get; set; }
}