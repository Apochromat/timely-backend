using Microsoft.EntityFrameworkCore;
using timely_backend.Models.DTO;

namespace timely_backend.Services
{
    public class ScheduleService:IScheduleService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminService> _logger;
        public ScheduleService(ILogger<AdminService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IList<TeacherDTO>> GetTeacher()
        {
            var Teachers = await _context.Teachers.Select(x => new TeacherDTO
            {   Id= x.Id,
                Name = x.Name
            }).ToListAsync();
            return Teachers;
        }
        public async Task<IList<ClassroomDTO>> GetClassroom()
        {
            var Classrooms = await _context.Classrooms.Select(x => new ClassroomDTO
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            return Classrooms;
        }
        public async Task<IList<GroupDTO>> GetGroup()
        {
            var Groups = await _context.Groups.Select(x => new GroupDTO
            {   Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            return Groups;
        }
        public async Task<IList<DomainDTO>> GetDomain()
        {
            var Domains = await _context.Domains.Select(x => new DomainDTO
            {
                Id = x.Id,
                Url = x.Url
            }).ToListAsync();
            return Domains;
        }
        public async Task<IList<LessonTagDTO>> GetLessonTag()
        {
            var LessonTags = await _context.LessonTags.Select(x => new LessonTagDTO
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            return LessonTags;
        }
        public async Task<IList<LessonNameDTO>> GetLessonName()
        {
            var LessonNames = await _context.LessonNames.Select(x => new LessonNameDTO
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            return LessonNames;
        }
        public async Task<IList<TimeIntervalDTO>> GetTimeInterval()
        {
            var TimeIntervals = await _context.TimeIntervals.Select(x => new TimeIntervalDTO
            {
                Id = x.Id,
                StartTime = x.StartTime,
                EndTime= x.EndTime 
            }).ToListAsync();
            return TimeIntervals;
        }
        public async Task<IList<LessonDTO>> GetLessonsGroup(DateTime date, GroupDTO group)
        {
            DayOfWeek day = date.DayOfWeek;

            DateTime startOfWeek = date.AddDays(-1 * (int)day);
            DateTime endOfWeek = startOfWeek.AddDays(6);
            var result = await _context.Lessons.Where(x => x.Date <= endOfWeek && x.Date >= startOfWeek && x.Group.Name == group.Name).Select(x => new LessonDTO
            {
                Id = x.Id,
                Name = x.Name,
                Tag= x.Tag,
                Group= x.Group,
                Teacher= x.Teacher,
                TimeInterval= x.TimeInterval,
                Classroom = x.Classroom,
                Date = x.Date

            }).ToListAsync();
            return result;
        }
        public async Task<IList<LessonDTO>> GetLessonsClassroom(DateTime date, ClassroomDTO classroom)
        {
            DayOfWeek day = date.DayOfWeek;

            DateTime startOfWeek = date.AddDays(-1 * (int)day);
            DateTime endOfWeek = startOfWeek.AddDays(6);
            var result = await _context.Lessons.Where(x => x.Date <= endOfWeek && x.Date >= startOfWeek && x.Classroom.Name == classroom.Name).Select(x => new LessonDTO
            {
                Id = x.Id,
                Name = x.Name,
                Tag = x.Tag,
                Group = x.Group,
                Teacher = x.Teacher,
                TimeInterval = x.TimeInterval,
                Classroom = x.Classroom,
                Date = x.Date

            }).ToListAsync();
            return result;
        }
        public async Task<IList<LessonDTO>> GetLessonsProfessor(DateTime date, TeacherDTO teacher)
        {
            DayOfWeek day = date.DayOfWeek;

            DateTime startOfWeek = date.AddDays(-1 * (int)day);
            DateTime endOfWeek = startOfWeek.AddDays(6);
            var result = await _context.Lessons.Where(x => x.Date <= endOfWeek && x.Date >= startOfWeek && x.Teacher.Name == teacher.Name).Select(x => new LessonDTO
            {
                Id = x.Id,
                Name = x.Name,
                Tag = x.Tag,
                Group = x.Group,
                Teacher = x.Teacher,
                TimeInterval = x.TimeInterval,
                Classroom = x.Classroom,
                Date = x.Date

            }).ToListAsync();
            return result;
        }
    }
}
