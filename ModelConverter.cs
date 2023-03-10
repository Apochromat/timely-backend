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
                Roles = user.Roles.Select(r => r.Role).Select(role => role.Name.ToString()).ToList(),
                Teacher = user.Teacher == null ? null : ToTeacherDTO(user.Teacher),
                Group = user.Group == null ? null : ToGroupDTO(user.Group),
                IsEmailConfirmed = user.EmailConfirmed,
                AvatarLink = user.AvatarLink
            };
            return temp;
        }

        public static Lesson ToLesson(LessonDTO model)
        {
            var temp = new Lesson
            {
                Name = new LessonName{ 
                    Name = model.Name.Name,
                    Id = (Guid)model.Name.Id
                },
                Tag = new LessonTag
                {
                    Name = model.Tag.Name,
                    Id = (Guid)model.Tag.Id
                },
                Teacher = new Teacher
                {
                    Name = model.Teacher.Name,
                    Id = (Guid)model.Teacher.Id
                },
                TimeInterval = new TimeInterval
                {
                    EndTime = model.TimeInterval.EndTime,
                    StartTime = model.TimeInterval.StartTime,
                    Id = (Guid)model.TimeInterval.Id
                },
                Group = (model.Group.Select(x => new Models.Group
                {
                    Id = (Guid)x.Id,
                    Name = x.Name,
                })).ToList(),
                Date= model.Date,
                ChainId= model.ChainId,
                Classroom = new Classroom
                {
                    Id= (Guid)model.Classroom.Id,
                    Name= model.Classroom.Name,
                }
        };
            return temp;
        }

        public static LessonNameDTO ToLessonNameDTO(LessonName model) {
            var temp = new LessonNameDTO {
                Id = model.Id,
                Name = model.Name
            };
            return temp;
        }

        public static LessonTagDTO ToLessonTagDTO(LessonTag model) {
            var temp = new LessonTagDTO {
                Id = model.Id,
                Name = model.Name
            };
            return temp;
        }

        public static IList<GroupDTO> ToGroupDTO(IList<Models.Group> model) {
            var temp = (model.Select(x=> new GroupDTO
            {
                Id = x.Id,
                Name = x.Name,
            })).ToList();
            return temp;
        }
        

        public static GroupDTO ToGroupDTO(Models.Group model) {
            var temp = new GroupDTO {
                Id = model.Id,
                Name = model.Name,
            };
            return temp;
        }

        public static TeacherDTO ToTeacherDTO(Teacher model) {
            var temp = new TeacherDTO {
                Id = model.Id,
                Name = model.Name
            };
            return temp;
        }

        public static TimeIntervalDTO ToTimeIntervalDTO(TimeInterval model) {
            var temp = new TimeIntervalDTO {
                Id = model.Id,
                StartTime = model.StartTime,
                EndTime = model.EndTime
            };
            return temp;
        }

        public static ClassroomDTO ToClassroomDTO(Classroom model) {
            var temp = new ClassroomDTO {
                Id = model.Id,
                Name = model.Name
            };
            return temp;
        }
    }
}