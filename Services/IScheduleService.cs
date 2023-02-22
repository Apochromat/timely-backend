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
        Task <IList<LessonDTO>> GetLessonsClassroom(DateTime date, ClassroomDTO classroom);
        Task<IList<LessonDTO>> GetLessonsProfessor(DateTime date, TeacherDTO teacher);
        Task<IList<LessonDTO>> GetLessonsGroup(DateTime date, GroupDTO group);

    }
}
