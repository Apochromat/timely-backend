using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace timely_backend.Models.Enums {
    public enum RoleType {
        [Display(Name = ApplicationRoleNames.Administrator)]
        Administrator,
        [Display(Name = ApplicationRoleNames.Student)]
        Student,
        [Display(Name = ApplicationRoleNames.Teacher)]
        Teacher
    }

    public class ApplicationRoleNames {
        public const string Administrator = "Администратор";
        public const string Student = "Студент";
        public const string Teacher = "Преподаватель";
    }
}
