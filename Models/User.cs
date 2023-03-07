using Microsoft.AspNetCore.Identity;

namespace timely_backend.Models;

public class User : IdentityUser<Guid> {
    public String? FullName { get; set; } = "";
    public Teacher? Teacher { get; set; }
    public Group? Group { get; set; }
    public String AvatarLink { get; set; }

    public ICollection<UserRole> Roles { get; set; }
}