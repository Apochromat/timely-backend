using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace timely_backend.Models.Enum {
    public enum RoleType {
        [Display(Name = ApplicationRoleNames.Administrator)]
        Administrator,
        [Display(Name = ApplicationRoleNames.Student)]
        Student,
        [Display(Name = ApplicationRoleNames.Teacher)]
        Teacher
    }

    public class ApplicationRoleNames {
        public const string Administrator = "Administrator";
        public const string Student = "Student";
        public const string Teacher = "Teacher";
    }
}
