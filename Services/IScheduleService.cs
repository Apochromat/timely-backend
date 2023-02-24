using timely_backend.Models.DTO;

namespace timely_backend.Services


{
    public interface IScheduleService
    {
        Task <IList<TeacherDTO>> GetTeacher();
        Task <IList<ClassroomDTO>> GetClassroom();
        Task <IList<GroupDTO>> GetGroup();
        Task <IList<DomainDTO>> GetDomain();
        Task <IList<LessonTagDTO>> GetLessonTag();
        Task <IList<LessonNameDTO>> GetLessonName();
        Task <IList<TimeIntervalDTO>> GetTimeInterval();
        Task <IList<LessonDTO>> GetLessonsClassroom(DateTime date, Guid id);
        Task<IList<LessonDTO>> GetLessonsProfessor(DateTime date, Guid id);
        Task<IList<LessonDTO>> GetLessonsGroup(DateTime date, Guid id);

    }
}
