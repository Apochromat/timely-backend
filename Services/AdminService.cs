﻿using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using timely_backend.Models;
using timely_backend.Models.DTO;

namespace timely_backend.Services
{
    public class AdminService : IAdminService
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
                throw new KeyNotFoundException("this domain does not exist");
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
                throw new KeyNotFoundException("this teacher does not exist");
            }
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
        }
        // classroom
        public async Task CreateClassroom(ClassroomDTO classroom)
        {
            if (classroom.Name == null)
            {
                throw new ArgumentNullException(nameof(classroom));
            }
            var sameClassroom = await _context.Classrooms.FirstOrDefaultAsync(x => x.Name == classroom.Name);

            if (sameClassroom != null)
            {
                throw new ArgumentException("this classroom is already exist");
            }
            await _context.Classrooms.AddAsync(new Classroom
            {
                Name = classroom.Name
            }) ;
            await _context.SaveChangesAsync();
        }
        public async Task EditClassroom(ClassroomDTO classroom, Guid id)
        {
            if (classroom.Name == null)
            {
                throw new ArgumentNullException(nameof(classroom));
            }
            var sameClassroom = await _context.Classrooms.FirstOrDefaultAsync(x =>x.Name == classroom.Name);

            if (sameClassroom != null)
            {
                throw new ArgumentException("this classroom is already exist");
            }
            var Classroom = await _context.Classrooms.FindAsync(id);
            if (Classroom == null) throw new KeyNotFoundException("classroom with this id does not exist");
            Classroom.Name = classroom.Name;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteClassroom(Guid id)
        {
            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null)
            {
                throw new KeyNotFoundException("this classroom does not exist");
            }
            _context.Classrooms.Remove(classroom);
            await _context.SaveChangesAsync();
        }
        //group
        public async Task CreateGroup(GroupDTO group)
        {
            if (group.Name == null)
            {
                throw new ArgumentNullException(nameof(group));
            }
            var sameGroup = await _context.Groups.FirstOrDefaultAsync(x => x.Name == group.Name);

            if (sameGroup != null)
            {
                throw new ArgumentException("this group is already exist");
            }
            await _context.Groups.AddAsync(new Group
            {
                Name = group.Name
            });
            await _context.SaveChangesAsync();
        }
        public async Task EditGroup(GroupDTO group, Guid id)
        {
            if (group.Name == null)
            {
                throw new ArgumentNullException(nameof(group));
            }
            var sameGroup = await _context.Groups.FirstOrDefaultAsync(x => x.Name == group.Name);

            if (sameGroup != null)
            {
                throw new ArgumentException("this group is already exist");
            }
            var Group = await _context.Groups.FindAsync(id);
            if (Group == null) throw new KeyNotFoundException("group with this id does not exist");
            Group.Name = group.Name;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteGroup (Guid id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                throw new KeyNotFoundException("this group does not exist");
            }
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
        }
        //lessonName
        public async Task CreateLessonName(LessonNameDTO lessonName)
        {
            if (lessonName.Name == null)
            {
                throw new ArgumentNullException(nameof(lessonName));
            }
            var sameLessonName = await _context.LessonNames.FirstOrDefaultAsync(x => x.Name == lessonName.Name);

            if (sameLessonName != null)
            {
                throw new ArgumentException("this lessonName is already exist");
            }
            await _context.LessonNames.AddAsync(new LessonName
            {
                Name = lessonName.Name
            });
            await _context.SaveChangesAsync();
        }
        public async Task EditLessonName(LessonNameDTO lessonName, Guid id)
        {
            if (lessonName.Name == null)
            {
                throw new ArgumentNullException(nameof(lessonName));
            }
            var sameLessonName = await _context.Groups.FirstOrDefaultAsync(x => x.Name == lessonName.Name);

            if (sameLessonName != null)
            {
                throw new ArgumentException("this lessonName is already exist");
            }
            var LessonName = await _context.LessonNames.FindAsync(id);
            if (LessonName == null) throw new KeyNotFoundException("lessonName with this id does not exist");
            LessonName.Name = lessonName.Name;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteLessonName(Guid id)
        {
            var lessonName = await _context.LessonNames.FindAsync(id);
            if (lessonName == null)
            {
                throw new KeyNotFoundException("this lessonName does not exist");
            }
            _context.LessonNames.Remove(lessonName);
            await _context.SaveChangesAsync();
        }
        //lessonTag
        public async Task CreateLessonTag(LessonTagDTO lessonTag)
        {
            if (lessonTag.Name == null)
            {
                throw new ArgumentNullException(nameof(lessonTag));
            }
            var sameLessonTag = await _context.LessonTags.FirstOrDefaultAsync(x => x.Name == lessonTag.Name);

            if (sameLessonTag != null)
            {
                throw new ArgumentException("this lessonTag is already exist");
            }
            await _context.LessonTags.AddAsync(new LessonTag
            {
                Name = lessonTag.Name
            });
            await _context.SaveChangesAsync();
        }
        public async Task EditLessonTag(LessonTagDTO lessonTag, Guid id) 
        {
            if (lessonTag.Name == null)
            {
                throw new ArgumentNullException(nameof(lessonTag));
            }
            var sameLessonTag = await _context.LessonTags.FirstOrDefaultAsync(x => x.Name == lessonTag.Name);

            if (sameLessonTag != null)
            {
                throw new ArgumentException("this lessonTag is already exist");
            }
            var LessonTag = await _context.LessonTags.FindAsync(id);
            if (LessonTag == null) throw new KeyNotFoundException("lessonTag with this id does not exist");
            LessonTag.Name = lessonTag.Name;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteLessonTag(Guid id)
        {
            var lessonTag = await _context.LessonTags.FindAsync(id);
            if (lessonTag == null)
            {
                throw new KeyNotFoundException("this lessonTag does not exist");
            }
            _context.LessonTags.Remove(lessonTag);
            await _context.SaveChangesAsync();
        }
        //timeInterval
        public async Task CreateTimeInterval(TimeIntervalDTO timeInterval)
        {
            if (timeInterval.StartTime == null || timeInterval.EndTime == null)
            {
                throw new ArgumentNullException(nameof(timeInterval));
            }
            /*var sameInterval = await _context.LessonTags.FirstOrDefaultAsync(x => x.Name == lessonTag.Name);

            if (sameInterval != null)
            {
                throw new ArgumentException("this lessonTag is already exist");
            }*/
            await _context.TimeIntervals.AddAsync(new TimeInterval
            {
                StartTime = timeInterval.StartTime,
                EndTime = timeInterval.EndTime,
                Timezone = timeInterval.Timezone
            }) ;
            await _context.SaveChangesAsync();
        }
        public async Task EditTimeInterval(TimeIntervalDTO timeInterval, Guid id)
        {
            if (timeInterval.StartTime == null || timeInterval.EndTime == null)
            {
                throw new ArgumentNullException(nameof(timeInterval));
            }
            /*var sameInterval = await _context.LessonTags.FirstOrDefaultAsync(x => x.Name == lessonTag.Name);

            if (sameInterval != null)
            {
                throw new ArgumentException("this lessonTag is already exist");
            }*/
            var TimeInterval = await _context.TimeIntervals.FindAsync(id);
            if (TimeInterval == null) throw new KeyNotFoundException("timeInterval with this id does not exist");
            TimeInterval.StartTime = timeInterval.StartTime;
            TimeInterval.EndTime = timeInterval.EndTime;
            TimeInterval.Timezone = timeInterval.Timezone;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteTimeInterval(Guid id)
        {
            var timeInterval = await _context.TimeIntervals.FindAsync(id);
            if (timeInterval == null)
            {
                throw new KeyNotFoundException("this timeInterval does not exist");
            }
            _context.TimeIntervals.Remove(timeInterval);
            await _context.SaveChangesAsync();
        }
    }
}
