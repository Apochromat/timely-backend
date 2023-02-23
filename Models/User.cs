using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using timely_backend.Models.Enums;

namespace timely_backend.Models;

public class User : IdentityUser<Guid> {

    public String FullName { get; set; } = "";

    public ICollection<UserRole> Roles { get; set; }
}