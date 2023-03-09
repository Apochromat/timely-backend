using Microsoft.EntityFrameworkCore;
using ServiceStack;
using timely_backend.Models;
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
        
        public async Task<IList<TeacherDTO>> GetTeacher(string filter)
        {
            
            if (filter != null)
            {
                filter = filter.ToLower();
                return await _context.Teachers.Where(x => x.Name.ToLower().Contains(filter) && x.IsDeleted == false).Select(x => new TeacherDTO
                {
                    Name = x.Name,
                    Id = x.Id,
                }).OrderBy(x => x.Name).ToListAsync();
            }
            else
            {
                return await _context.Teachers.Where(x=>x.IsDeleted == false).Select(x => new TeacherDTO
                {
                    Id = x.Id,
                    Name = x.Name
                }).OrderBy(x => x.Name).ToListAsync();
            } 
        }
        public async Task<IList<ClassroomDTO>> GetClassroom(string filter)
        {
            if (filter != null)
            {
                filter = filter.ToLower();
                return await _context.Classrooms.Where(x => x.Name.ToLower().Contains(filter) && x.IsDeleted == false).Select(x => new ClassroomDTO
                {
                    Name = x.Name,
                    Id = x.Id,
                }).OrderBy(x => x.Name).ToListAsync();
            }
            else
            {
                return await _context.Classrooms.Where(x=> x.IsDeleted == false).Select(x => new ClassroomDTO
                {
                    Id = x.Id,
                    Name = x.Name
                }).OrderBy(x => x.Name).ToListAsync();
            }
        }
        public async Task<IList<GroupDTO>> GetGroup(string filter)
        {
            if (filter != null)
            {
                filter = filter.ToLower();
                return await _context.Groups.Where(x => x.Name.ToLower().Contains(filter) && x.IsDeleted == false).Select(x => new GroupDTO
                {
                    Name = x.Name,
                    Id = x.Id,
                }).OrderBy(x => x.Name).ToListAsync();
            }
            else
            {
                return await _context.Groups.Where(x=> x.IsDeleted == false).Select(x => new GroupDTO
                {
                    Id = x.Id,
                    Name = x.Name
                }).OrderBy(x => x.Name).ToListAsync();
            }
        }
        public async Task<IList<DomainDTO>> GetDomains(string filter)
        {
            if (filter != null)
            {
                filter = filter.ToLower();
                return await _context.Domains.Where(x => x.Url.ToLower().Contains(filter)).Select(x => new DomainDTO
                {
                    Url = x.Url,
                    Id = x.Id,
                }).OrderBy(x => x.Url).ToListAsync();
            }
            else
            {
                return await _context.Domains.Select(x => new DomainDTO
                {
                    Id = x.Id,
                    Url = x.Url
                }).OrderBy(x => x.Url).ToListAsync();
            }
        }
        public async Task<IList<LessonTagDTO>> GetLessonTag(string filter)
        {
            if (filter != null)
            {
                filter = filter.ToLower();
                return await _context.LessonTags.Where(x => x.Name.ToLower().Contains(filter) && x.IsDeleted == false).Select(x => new LessonTagDTO
                {
                    Name = x.Name,
                    Id = x.Id,
                }).OrderBy(x => x.Name).ToListAsync();
            }
            else
            {
                return await _context.LessonTags.Where(x => x.IsDeleted == false).Select(x => new LessonTagDTO
                {
                    Id = x.Id,
                    Name = x.Name
                }).OrderBy(x => x.Name).ToListAsync();
            }
        }
        public async Task<IList<LessonNameDTO>> GetLessonName(string filter)
        {
            if (filter != null)
            {
                filter = filter.ToLower();
                return await _context.LessonNames.Where(x => x.Name.ToLower().Contains(filter) && x.IsDeleted == false).Select(x => new LessonNameDTO
                {
                    Name = x.Name,
                    Id = x.Id,
                }).OrderBy(x => x.Name).ToListAsync();
            }
            else
            {
                return await _context.LessonNames.Where(x => x.IsDeleted == false).Select(x => new LessonNameDTO
                {
                    Id = x.Id,
                    Name = x.Name
                }).OrderBy(x => x.Name).ToListAsync();
            }
        }
        public async Task<IList<TimeIntervalDTO>> GetTimeInterval()
        {
            var TimeIntervals = await _context.TimeIntervals.Where(x => x.IsDeleted == false).Select(x => new TimeIntervalDTO
            {
                Id = x.Id,
                StartTime = x.StartTime,
                EndTime= x.EndTime 
            }).ToListAsync();
            return TimeIntervals;
        }
        public async Task<IList<LessonDTO>> GetLessonsGroup(DateTime date, Guid id)
        {
            DayOfWeek day = date.DayOfWeek;

            DateTime startOfWeek = date.AddDays(-1 * (int)day);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            var group = await _context.Groups.FindAsync(id);
            if (group == null) throw new ArgumentException("group with this id does not exist");

            var result = await _context.Lessons.Where(x => x.Date <= endOfWeek && x.Date >= startOfWeek).Include(o => o.Group).Where(o => o.Group.Contains(group)).Select(x => new LessonDTO
            {
                Id = x.Id,
                Name = ModelConverter.ToLessonNameDTO(x.Name),
                Tag= ModelConverter.ToLessonTagDTO(x.Tag),
                Group= ModelConverter.ToGroupDTO(x.Group),
                Teacher= ModelConverter.ToTeacherDTO(x.Teacher),
                TimeInterval= ModelConverter.ToTimeIntervalDTO(x.TimeInterval),
                Classroom = ModelConverter.ToClassroomDTO(x.Classroom),
                ChainId = x.ChainId,
                Date = x.Date,
                IsReadOnly = x.IsReadOnly
            }).ToListAsync();
            return result;
        }
        public async Task<IList<LessonDTO>> GetLessonsClassroom(DateTime date, Guid id)
        {
            DayOfWeek day = date.DayOfWeek;

            DateTime startOfWeek = date.AddDays(-1 * (int)day);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            var error = await _context.Classrooms.FindAsync(id);
            if (error == null) throw new ArgumentException("classroom with this id does not exist");

            var result = await _context.Lessons.Include(x => x.Group).Where(x => x.Date <= endOfWeek && x.Date >= startOfWeek && x.Classroom.Id == id).Select(x => new LessonDTO
            {
                Id = x.Id,
                Name = ModelConverter.ToLessonNameDTO(x.Name),
                Tag = ModelConverter.ToLessonTagDTO(x.Tag),
                Group = ModelConverter.ToGroupDTO(x.Group),
                Teacher = ModelConverter.ToTeacherDTO(x.Teacher),
                TimeInterval = ModelConverter.ToTimeIntervalDTO(x.TimeInterval),
                Classroom = ModelConverter.ToClassroomDTO(x.Classroom),
                ChainId = x.ChainId,
                Date = x.Date,
                IsReadOnly = x.IsReadOnly

            }).ToListAsync();
            return result;
        }
        public async Task<IList<LessonDTO>> GetLessonsProfessor(DateTime date,Guid id)
        {
            DayOfWeek day = date.DayOfWeek;

            DateTime startOfWeek = date.AddDays(-1 * (int)day);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            var error = await _context.Teachers.FirstOrDefaultAsync(x=>x.Id == id);
            if (error == null) throw new ArgumentException("teacher with this id does not exist");

            var result = await _context.Lessons.Include(x=>x.Group).Where(x => x.Date <= endOfWeek && x.Date >= startOfWeek && x.Teacher.Id == id).Select(x => new LessonDTO
            {
                Id = x.Id,
                Name = ModelConverter.ToLessonNameDTO(x.Name),
                Tag = ModelConverter.ToLessonTagDTO(x.Tag),
                Group = ModelConverter.ToGroupDTO(x.Group),
                Teacher = ModelConverter.ToTeacherDTO(x.Teacher),
                TimeInterval = ModelConverter.ToTimeIntervalDTO(x.TimeInterval),
                Classroom = ModelConverter.ToClassroomDTO(x.Classroom),
                ChainId = x.ChainId,
                Date = x.Date,
                IsReadOnly = x.IsReadOnly

            }).ToListAsync();
            return result;
        }
        public async Task<IList<Lesson>> GetLessonsProfessorDb(DateTime date, Guid id)
        {
            DayOfWeek day = date.DayOfWeek;

            DateTime startOfWeek = date.AddDays(-1 * (int)day);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            var error = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == id);
            if (error == null) throw new ArgumentException("teacher with this id does not exist");

            var result = await _context.Lessons.Include(x => x.Group).Include(x => x.Teacher).Include(x => x.Tag).Include(x => x.Name).Include(x => x.TimeInterval).Include(x => x.Classroom).Where(x => x.Date <= endOfWeek && x.Date >= startOfWeek && x.Teacher.Id == id).Select(x => x).ToListAsync();
            return result;
        }
        public async Task<IList<Lesson>> GetLessonsClassroomDb(DateTime date, Guid id) {
            DayOfWeek day = date.DayOfWeek;

            DateTime startOfWeek = date.AddDays(-1 * (int)day);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            var error = await _context.Classrooms.FirstOrDefaultAsync(x => x.Id == id);
            if (error == null) throw new ArgumentException("classroom with this id does not exist");

            var result = await _context.Lessons.Include(x => x.Group).Include(x => x.Teacher).Include(x => x.Tag).Include(x => x.Name).Include(x => x.TimeInterval).Include(x => x.Classroom).Where(x => x.Date <= endOfWeek && x.Date >= startOfWeek && x.Classroom.Id == id).Select(x => x).ToListAsync();
            return result;
        }
        public async Task<IList<Lesson>> GetLessonsGroupDb(DateTime date, Guid id) {
            DayOfWeek day = date.DayOfWeek;

            DateTime startOfWeek = date.AddDays(-1 * (int)day);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            var group = await _context.Groups.FindAsync(id);
            if (group == null) throw new ArgumentException("group with this id does not exist");

            var result = await _context.Lessons.Include(x => x.Teacher).Include(x => x.Tag).Include(x => x.Name).Include(x => x.TimeInterval).Include(x => x.Classroom).Where(x => x.Date <= endOfWeek && x.Date >= startOfWeek).Include(o => o.Group).Where(o => o.Group.Contains(group)).Select(x => x).ToListAsync();
            return result;
        }
    }
}
