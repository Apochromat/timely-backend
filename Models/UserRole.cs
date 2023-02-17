using Microsoft.AspNetCore.Identity;

namespace timely_backend.Models {
    public class UserRole : IdentityUserRole<Guid> {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }

    }
}
