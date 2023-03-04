using timely_backend.Models.DTO;

namespace timely_backend.Services


{
    public interface IScheduleService
    {
        Task <IList<TeacherDTO>> GetTeacher(string filter);
        Task <IList<ClassroomDTO>> GetClassroom(string filter);
        Task <IList<GroupDTO>> GetGroup(string filter);
        Task <IList<DomainDTO>> GetDomains(string filter);
        Task <IList<LessonTagDTO>> GetLessonTag(string filter);
        Task <IList<LessonNameDTO>> GetLessonName(string filter);
        Task <IList<TimeIntervalDTO>> GetTimeInterval();
        Task <IList<LessonDTO>> GetLessonsClassroom(DateTime date, Guid id);
        Task<IList<LessonDTO>> GetLessonsProfessor(DateTime date, Guid id);
        Task<IList<LessonDTO>> GetLessonsGroup(DateTime date, Guid id);

    }
}
