using System.ComponentModel;

namespace timely_backend.Models.DTO; 

public class UserWithRolesEditDTO {
    [DisplayName("email")] public String Email { get; set; } = "";
    [DisplayName("roles")] public List<String> Roles { get; set; }
}