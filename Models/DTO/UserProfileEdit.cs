using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models.DTO;

public class UserProfileEdit {
    [Required] [DisplayName("fullName")] public String? FullName { get; set; } = "";
}