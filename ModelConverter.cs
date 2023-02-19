using System.Text.RegularExpressions;
using timely_backend.Models;
using timely_backend.Models.DTO;

namespace timely_backend {
    /// <summary>
    /// Static class for model converting
    /// </summary>
    public static class ModelConverter {
        /// <summary>
        /// Convert UserRegisterModel to User
        /// </summary>
        public static User ToUser(UserRegisterModel userRegisterModel) {
            if (userRegisterModel.FullName == null) {
                throw new ArgumentNullException(nameof(userRegisterModel.FullName));
            }
            if (userRegisterModel.Email == null) {
                throw new ArgumentNullException(nameof(userRegisterModel.Email));
            }
            
            var temp = new User {
                UserName = Misc.TransliterateNameAndEmail(userRegisterModel.FullName, userRegisterModel.Email),
                Email = userRegisterModel.Email,
                FullName = Regex.Replace(userRegisterModel.FullName, @"\s+", " ")
            };
            return temp;
        }

        /// <summary>
        /// Convert User to UserProfile
        /// </summary>
        public static UserProfile ToUserProfile(User user) {
            var temp = new UserProfile {
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Roles = user.Roles.Select(r => r.Role).Select(role => role.Name.ToString()).ToList()
            };
            return temp;
        }
    }
}