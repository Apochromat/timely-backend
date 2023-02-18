using timely_backend.Models.Enums;
using System.ComponentModel;

namespace timely_backend.Models.DTO;

public class TokenResponse {

    public TokenResponse() { }
    public TokenResponse(string token, string email, RoleType? role) {
        this.Token = token;
        this.Email = email;
        this.Role = role;
    }

    [DisplayName("token")] public string? Token { get; set; }
    [DisplayName("name")] public string? Email { get; set; }
    [DisplayName("role")] public RoleType? Role { get; set; }
}