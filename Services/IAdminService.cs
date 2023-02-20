using timely_backend.Models;
using timely_backend.Models.DTO;

namespace timely_backend.Services
{

    public interface IAdminService
    {
        //domain
        Task CreateDomain(DomainDTO domain);
        Task EditDomain(DomainDTO domain, Guid id);
        Task DeleteDomain(Guid id);
       
        // teacher
        Task CreateTeacher(TeacherDTO teacher);
        Task EditTeacher(TeacherDTO teacher, Guid id);
        Task DeleteTeacher(Guid id);
        //classroom
        Task CreateClassroom(ClassroomDTO classroom);
        Task EditClassroom(ClassroomDTO classroom, Guid id);
        Task DeleteClassroom(Guid id);
        //lessonName
        Task CreateLessonName(LessonNameDTO lessonName);
        Task EditLessonName(LessonNameDTO lessonName, Guid id);
        Task DeleteLessonName(Guid id);
        // lessonTag
        Task CreateLessonTag(LessonTagDTO lessonTag);
        Task EditLessonTag(LessonTagDTO lessonTag, Guid id);
        Task DeleteLessonTag(Guid id);
        //timeinterval
        Task CreateTimeInterval(TimeIntervalDTO timeInterval);
        Task EditTimeInterval(TimeIntervalDTO timeInterval, Guid id);
        Task DeleteTimeInterval(Guid id);
        //group
        Task CreateGroup(GroupDTO group);
        Task EditGroup(GroupDTO group, Guid id);
        Task DeleteGroup(Guid id);
    }

    
}
