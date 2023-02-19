using Microsoft.EntityFrameworkCore;
using timely_backend.Models;
using timely_backend.Models.DTO;

namespace timely_backend.Services
{
    public class AdminService : IadminService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminService> _logger;
        public AdminService (ILogger<AdminService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        //domain
        public async Task CreateDomain(DomainDTO domain)
        {
            if (domain.Url == null)
            {
                throw new ArgumentNullException(nameof(domain));
            }
            var sameDomain = await _context.Domains.FirstOrDefaultAsync(x => x.Url == domain.Url);

            if (sameDomain != null) {
                throw new ArgumentException("this domain is already exist");
            }
            await _context.Domains.AddAsync(new Domain
            {
                Url = domain.Url
            });
            await _context.SaveChangesAsync();
        }
        public async Task EditDomain(DomainDTO domain, Guid id)
        {
            if (domain.Url == null)
            {
                throw new ArgumentNullException(nameof(domain));
            }
            var sameDomain = await _context.Domains.FirstOrDefaultAsync(x => x.Url == domain.Url);

            if (sameDomain != null)
            {
                throw new ArgumentException("this domain is already exist");
            }
            var Domain = await _context.Domains.FindAsync(id);
            if (Domain == null) throw new KeyNotFoundException("domain with this id does not exist");
            Domain.Url = domain.Url;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteDomain(Guid id)
        {
            var domain = await _context.Domains.FindAsync(id);
            if (domain == null)
            {
                throw new ArgumentNullException("this domain does not exist");
            }
            _context.Domains.Remove(domain);
            await _context.SaveChangesAsync();

        }
        //teacher
        public async Task CreateTeacher(TeacherDTO teacher)
        {
            if (teacher.Name == null)
            {
                throw new ArgumentNullException(nameof(teacher));
            }
            //var sameTeacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Name == teacher.Name);

            /*if (sameTeacher != null)
            {
                throw new ArgumentException("this Teacher is already exist");
            }*/
            await _context.Teachers.AddAsync(new Teacher
            {
                Name = teacher.Name
            }) ;
            await _context.SaveChangesAsync();
        }
        public async Task EditTeacher(TeacherDTO teacher, Guid id)
        {
            if (teacher.Name == null)
            {
                throw new ArgumentNullException(nameof(teacher));
            }
            /*var sameTeacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Name == teacher.Name);

            if (sameTeacher != null)
            {
                throw new ArgumentException("this domain is already exist");
            }*/
            var Teacher = await _context.Teachers.FindAsync(id);
            if (Teacher == null) throw new KeyNotFoundException("teacher with this id does not exist");
            Teacher.Name = teacher.Name;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteTeacher(Guid id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                throw new ArgumentNullException("this teacher does not exist");
            }
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
        }
        // classroom
        public async Task CreateClassroom(classroomDTO classroom)
        {

        }
        public async Task EditClassroom(classroomDTO classroom, Guid id)
        {

        }
        public async Task DeleteClassroom(Guid id)
        {

        }
        //group
        public async Task CreateGroup(GroupDTO group)
        {

        }
        public async Task EditGroup(GroupDTO group, Guid id)
        {

        }
        public async Task DeleteGroup (Guid id)
        {

        }
        //lessonName
        public async Task CreateLessonName(LessonNameDTO lessonName)
        {

        }
        public async Task EditLessonName(LessonNameDTO lessonName, Guid id)
        {

        }
        public async Task DeleteLessonName(Guid id)
        {

        }
        //lessonTag
        public async Task CreateLessonTag(LessonTagDTO lessonTag)
        {

        }
        public async Task EditLessonTag(LessonTagDTO lessonTag, Guid id) 
        {

        }
        public async Task DeleteLessonTag(Guid id)
        {

        }
        //timeInterval
        public async Task CreateTimeInterval(TimeIntervalDTO timeInterval)
        {

        }
        public async Task EditTimeInterval(TimeIntervalDTO timeInterval, Guid id)
        {

        }
        public async Task DeleteTimeInterval(Guid id)
        {

        }
    }
}
