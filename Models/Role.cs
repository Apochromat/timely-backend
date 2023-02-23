using timely_backend.Models.Enum;
using Microsoft.AspNetCore.Identity;

namespace timely_backend.Models {
    public class Role : IdentityRole<Guid> {
        public RoleType Type { get; set; }
        public ICollection<UserRole> Users { get; set; }
    }
}
