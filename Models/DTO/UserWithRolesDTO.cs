using System.ComponentModel;

namespace timely_backend.Models.DTO; 

public class UserWithRolesDTO {
    [DisplayName("fullName")] public String FullName { get; set; } = "";
    [DisplayName("email")] public String Email { get; set; } = "";
    [DisplayName("roles")] public List<String> Roles { get; set; }
}