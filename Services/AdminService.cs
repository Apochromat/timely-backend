using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Org.BouncyCastle.Crypto;
using Serilog;
using ServiceStack;
using timely_backend.Models;
using timely_backend.Models.DTO;
using timely_backend.Models.Enum;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Group = timely_backend.Models.Group;

namespace timely_backend.Services {
    public class AdminService : IAdminService {
        private readonly ApplicationDbContext _context;
        private readonly IScheduleService _scheduleService;
        private readonly ILogger<AdminService> _logger;
        public AdminService(ILogger<AdminService> logger, ApplicationDbContext context, IScheduleService scheduleService) {
            _scheduleService = scheduleService;
            _logger = logger;
            _context = context;
        }
        public async Task CreateLesson(LessonFromId lesson) {
            var teacher = await _context.Teachers.FindAsync(lesson.TeacherId);
            if (teacher == null) throw new KeyNotFoundException("this teacher with this id does not exist");

            var classroom = await _context.Classrooms.FindAsync(lesson.ClassroomId);
            if (classroom == null) throw new KeyNotFoundException("this classroom with this id does not exist");

            var groups = _context.Groups.Where(e => lesson.GroupId.Contains(e.Id)).ToList();
            /*var existGroup = _context.Groups.All(e => lesson.GroupId.Contains(e.Id));
            if (existGroup == false) throw new KeyNotFoundException("group with this id does not exist");*/

            var lessonName = await _context.LessonNames.FindAsync(lesson.NameId);
            if (lessonName == null) throw new KeyNotFoundException("lessonName with this id does not exist");

            var lessonTag = await _context.LessonTags.FindAsync(lesson.TagId);
            if (lessonTag == null) throw new KeyNotFoundException("this lessonTag does not exist");

            var timeInterval = await _context.TimeIntervals.FindAsync(lesson.TimeIntervalId);
            if (timeInterval == null) throw new KeyNotFoundException("this timeInterval does not exist");


            var sameLesson = await _context.Lessons.FirstOrDefaultAsync(x => x.Teacher == teacher && x.TimeInterval == timeInterval && x.Date.Date == lesson.Date.Date);
            if (sameLesson != null) {
                throw new ArgumentException("this lesson is already exist with " + teacher.Name);
            }
            sameLesson = await _context.Lessons.FirstOrDefaultAsync(x => x.Classroom == classroom && x.TimeInterval == timeInterval && x.Date.Date == lesson.Date.Date);
            if (sameLesson != null && sameLesson.Classroom.Name.ToLower() != "онлайн") {
                throw new ArgumentException("this lesson is already exist with " + classroom.Name);
            }

            /* bool interesct = false;
             foreach (var g in groups)
             {
                 if (_context.Lessons.Include(x=>x.Group).Where(l => l.Group.Contains(g) && l.TimeInterval == timeInterval && l.Date.Date == lesson.Date.Date).ToList().Count > 0)
                 {
                     interesct = true; break;
                 }
             }
             if (interesct)
             {
                 throw new ArgumentException("this lesson intersects some group");
             }*/
            foreach (var g in groups) {
                if (_context.Lessons.Include(x => x.Group).Where(l => l.Group.Contains(g) && l.TimeInterval == timeInterval && l.Date.Date == lesson.Date.Date).ToList().Count > 0) {
                    throw new ArgumentException("this lesson is already exist " + g.Name);
                }
            }


            await _context.Lessons.AddAsync(new Lesson {
                Name = lessonName,
                Tag = lessonTag,
                TimeInterval = timeInterval,
                Group = groups,
                Teacher = teacher,
                Classroom = classroom,
                Date = lesson.Date,
                ChainId = lesson.ChainId,
            });
            await _context.SaveChangesAsync();
        }
        public async Task EditLesson(LessonFromId lesson, Guid id) {
            var teacher = await _context.Teachers.FindAsync(lesson.TeacherId);
            if (teacher == null) throw new KeyNotFoundException("this teacher with this id does not exist");

            var classroom = await _context.Classrooms.FindAsync(lesson.ClassroomId);
            if (classroom == null) throw new KeyNotFoundException("this classroom with this id does not exist");

            var groups = _context.Groups.Where(e => lesson.GroupId.Contains(e.Id)).ToList();
            /*var existGroup = _context.Groups.All(e => lesson.GroupId.Contains(e.Id));
            if (existGroup == false) throw new KeyNotFoundException("group with this id does not exist");*/

            var LessonName = await _context.LessonNames.FindAsync(lesson.NameId);
            if (LessonName == null) throw new KeyNotFoundException("lessonName with this id does not exist");

            var lessonTag = await _context.LessonTags.FindAsync(lesson.TagId);
            if (lessonTag == null) throw new KeyNotFoundException("this lessonTag does not exist");

            var timeInterval = await _context.TimeIntervals.FindAsync(lesson.TimeIntervalId);
            if (timeInterval == null) throw new KeyNotFoundException("this timeInterval does not exist");

            var Lesson = await _context.Lessons.Include(x => x.Group).Include(x => x.Teacher).Include(x => x.Tag).Include(x => x.Name).Include(x => x.TimeInterval).Include(x => x.Classroom).FirstOrDefaultAsync(x=>x.Id == id);

            if (Lesson == null) throw new KeyNotFoundException("Lesson with this id does not exist");
            if (Lesson.IsReadOnly) throw new InvalidOperationException("Lesson is read-only");

            var sameLesson = await _context.Lessons.Include(x => x.Group).Include(x => x.Teacher).Include(x => x.Tag).Include(x => x.Name).Include(x => x.TimeInterval).Include(x => x.Classroom).FirstOrDefaultAsync(x => x.Teacher == teacher && x.TimeInterval == timeInterval && x.Date.Date == lesson.Date.Date);
            if (sameLesson != null && sameLesson.Id != id) {
                throw new ArgumentException("this lesson is already exist with " + teacher.Name);
            }
            sameLesson = await _context.Lessons.Include(x => x.Group).Include(x => x.Teacher).Include(x => x.Tag).Include(x => x.Name).Include(x => x.TimeInterval).Include(x => x.Classroom).FirstOrDefaultAsync(x => x.Classroom == classroom && x.TimeInterval == timeInterval && x.Date.Date == lesson.Date.Date);
            if (sameLesson != null && sameLesson.Classroom.Name.ToLower() != "онлайн" && sameLesson.Id != id) {
                throw new ArgumentException("this lesson is already exist with " + classroom.Name);
            }


            foreach (var g in groups) {
                if (_context.Lessons.Include(x => x.TimeInterval).Include(x => x.Group).Where(l => l.Group.Contains(g) && l.Id != Lesson.Id && l.TimeInterval == timeInterval && l.Date.Date == lesson.Date.Date).ToList().Count > 0) {
                     throw new ArgumentException("this lesson is already exist " + g.Name);
                }
            }


            Lesson.Name = LessonName;
            Lesson.Tag = lessonTag;
            Lesson.TimeInterval = timeInterval;
            Lesson.Group = groups;
            Lesson.Teacher = teacher;
            Lesson.Classroom = classroom;
            Lesson.Date = lesson.Date;
            Lesson.ChainId = lesson.ChainId;

            await _context.SaveChangesAsync();
        }

        public async Task EditChainLesson(LessonFromId lesson, Guid id) {
            var teacher = await _context.Teachers.FindAsync(lesson.TeacherId);
            if (teacher == null) throw new KeyNotFoundException("this teacher with this id does not exist");

            var classroom = await _context.Classrooms.FindAsync(lesson.ClassroomId);
            if (classroom == null) throw new KeyNotFoundException("this classroom with this id does not exist");

            var groups = _context.Groups.Where(e => lesson.GroupId.Contains(e.Id)).ToList();
            /*var existGroup = _context.Groups.All(e => lesson.GroupId.Contains(e.Id));
            if (existGroup == false) throw new KeyNotFoundException("group with this id does not exist");*/

            var LessonName = await _context.LessonNames.FindAsync(lesson.NameId);
            if (LessonName == null) throw new KeyNotFoundException("lessonName with this id does not exist");

            var lessonTag = await _context.LessonTags.FindAsync(lesson.TagId);
            if (lessonTag == null) throw new KeyNotFoundException("this lessonTag does not exist");

            var timeInterval = await _context.TimeIntervals.FindAsync(lesson.TimeIntervalId);
            if (timeInterval == null) throw new KeyNotFoundException("this timeInterval does not exist");

            var Lesson = await _context.Lessons.Include(x => x.Group).Include(x => x.Teacher).Include(x => x.Tag).Include(x => x.Name).Include(x => x.TimeInterval).Include(x => x.Classroom).FirstOrDefaultAsync(x => x.Id == id);

            if (Lesson == null) throw new KeyNotFoundException("Lesson with this id does not exist");
            if (Lesson.IsReadOnly) throw new InvalidOperationException("Lesson is read-only");

            var chainLessons = await _context.Lessons.Include(x => x.Group).Include(x => x.Teacher).Include(x => x.Tag).Include(x => x.Name).Include(x => x.TimeInterval).Include(x => x.Classroom).Where(x => x.ChainId == Lesson.ChainId && x.Date.Date >= Lesson.Date.Date).ToListAsync();

            Lesson sameLesson = null;
            foreach (var chain in chainLessons) {

                chain.Name = LessonName;
                chain.Tag = lessonTag;
                chain.TimeInterval = timeInterval;
                chain.Group = groups;
                chain.Teacher = teacher;
                chain.Classroom = classroom;
                chain.Date = chain.Date.AddDays((lesson.Date.Date - Lesson.Date.Date).TotalDays);
                chain.ChainId = lesson.ChainId;

                sameLesson = await _context.Lessons.Include(x => x.Group).Include(x => x.Teacher).Include(x => x.Tag).Include(x => x.Name).Include(x => x.TimeInterval).Include(x => x.Classroom).FirstOrDefaultAsync(x => x.Teacher == teacher && x.TimeInterval == timeInterval && x.Date.Date == chain.Date.Date);
                if (sameLesson != null && sameLesson.Id != chain.Id) {
                    throw new ArgumentException("this lesson is already exist with " + teacher.Name);
                }
                sameLesson = await _context.Lessons.Include(x => x.Group).Include(x => x.Teacher).Include(x => x.Tag).Include(x => x.Name).Include(x => x.TimeInterval).Include(x => x.Classroom).FirstOrDefaultAsync(x => x.Classroom == classroom && x.TimeInterval == timeInterval && x.Date.Date == chain.Date.Date);
                if (sameLesson != null && sameLesson.Classroom.Name.ToLower() != "онлайн" && sameLesson.Id != chain.Id) {
                    throw new ArgumentException("this lesson is already exist with " + classroom.Name);
                }
                foreach (var g in groups) {
                    if (_context.Lessons.Include(x => x.TimeInterval).Include(x => x.Group).Where(l => l.Group.Contains(g) && l.Id != chain.Id && l.TimeInterval == timeInterval && l.Date.Date == chain.Date.Date).ToList().Count > 0) {
                        throw new ArgumentException("this lesson is already exist " + g.Name);
                    }
                }

            }

            await _context.SaveChangesAsync();
        }
        public async Task DeleteLesson(Guid id) {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) {
                throw new KeyNotFoundException("this teacher does not exist");
            }
            if (lesson.IsReadOnly) throw new InvalidOperationException("Lesson is read-only");

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChainLesson(Guid id) {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) {
                throw new KeyNotFoundException("this teacher does not exist");
            }
            if (lesson.IsReadOnly) throw new InvalidOperationException("Lesson is read-only");

            var chainLessons = await _context.Lessons.Include(x => x.Group).Include(x => x.Teacher).Include(x => x.Tag).Include(x => x.Name).Include(x => x.TimeInterval).Include(x => x.Classroom).Where(x => x.ChainId == lesson.ChainId && x.Date.Date >= lesson.Date.Date).ToListAsync();

            _context.Lessons.RemoveRange(chainLessons);
            await _context.SaveChangesAsync();
        }

        public async Task DuplicateLesson(DuplicateDTO duplicateDTO) {

            IList<Lesson> Lessons = null;


            if (duplicateDTO.SchedulleType == TypeSchedulleEnum.Teacher) {
                Lessons = await _scheduleService.GetLessonsProfessorDb(duplicateDTO.DateToCopy, duplicateDTO.Id);
            }
            else if (duplicateDTO.SchedulleType == TypeSchedulleEnum.Classroom) {

                Lessons = await _scheduleService.GetLessonsClassroomDb(duplicateDTO.DateToCopy, duplicateDTO.Id);
            }
            else if (duplicateDTO.SchedulleType == TypeSchedulleEnum.Group) {
                Lessons = await _scheduleService.GetLessonsGroupDb(duplicateDTO.DateToCopy, duplicateDTO.Id);
            }
            if (Lessons == null) throw new InvalidOperationException("На этой неделе нет пар для дублирования");


            foreach (var lesson in Lessons) {
                lesson.ChainId = Guid.NewGuid();
                for (int i = 0; i < duplicateDTO.AmountOfWeeks; i++) {    
                    var l = new Lesson() {
                        Name = lesson.Name,
                        Tag = lesson.Tag,
                        Group = lesson.Group,
                        Teacher = lesson.Teacher,
                        TimeInterval = lesson.TimeInterval,
                        Classroom = lesson.Classroom,
                        Date = lesson.Date,
                        ChainId = lesson.ChainId
                    };
                    l.Date = lesson.Date.AddDays((i+1)*7);
                    if (IsLesssonIntersect(l)) {
                        throw new ArgumentException("lesson with " + l.Group.Count + " groups " + l.Name.Name + " " + l.Teacher.Name + " " + l.Classroom.Name + " " + l.Date + " intersect someting");
                    }
                    await _context.Lessons.AddAsync(l);
                }
            }

            await _context.SaveChangesAsync();
        }
        public bool IsLesssonIntersect(Lesson lesson) {
            var check = false;
            var teacher = _context.Teachers.AsNoTracking().FirstOrDefault(x => x.Id == lesson.Teacher.Id);
            if (teacher == null) throw new KeyNotFoundException("this teacher with this id does not exist");

            var classroom = _context.Classrooms.AsNoTracking().FirstOrDefault(x => x.Id == lesson.Classroom.Id);
            if (classroom == null) throw new KeyNotFoundException("this classroom with this id does not exist");

            var gNames = lesson.Group.Select(g => g.Name);
            var groups = _context.Groups.Select(x => x).Where(e => gNames.Contains(e.Name)).OrderBy(x => x.Name).AsNoTracking().ToList();

            var LessonName = _context.LessonNames.AsNoTracking().FirstOrDefault(x => x.Id == lesson.Name.Id);
            if (LessonName == null) throw new KeyNotFoundException("lessonName with this id does not exist");

            var lessonTag = _context.LessonTags.AsNoTracking().FirstOrDefault(x => x.Id == lesson.Tag.Id);
            if (lessonTag == null) throw new KeyNotFoundException("this lessonTag does not exist");

            var timeInterval = _context.TimeIntervals.AsNoTracking().FirstOrDefault(x => x.Id == lesson.TimeInterval.Id);
            if (timeInterval == null) throw new KeyNotFoundException("this timeInterval does not exist");

            var sameLesson = _context.Lessons.AsNoTracking().Include(x => x.Classroom).Include(x => x.TimeInterval).FirstOrDefault(x => x.Teacher == teacher && x.TimeInterval == timeInterval && x.Date.Date == lesson.Date.Date);
            if (sameLesson != null) {
                throw new ArgumentException("Дублирование невозможно из-за наложения на существующую пару: " + sameLesson.Name.Name + " " + sameLesson.Teacher.Name + " " + sameLesson.Classroom.Name + " " + sameLesson.Date.ToShortDateString() + " " + sameLesson.TimeInterval.StartTime);

            }
            sameLesson = _context.Lessons.AsNoTracking().Include(x => x.Classroom).Include(x => x.TimeInterval).FirstOrDefault(x => x.Classroom == classroom && x.TimeInterval == timeInterval && x.Date.Date == lesson.Date.Date);
            if (sameLesson != null) {
                if (sameLesson.Classroom.Name.ToLower() != "онлайн")
                    throw new ArgumentException( "Дублирование невозможно из-за наложения на существующую пару: " + sameLesson.Name.Name + " " + sameLesson.Teacher.Name + " " + sameLesson.Classroom.Name + " " + sameLesson.Date.ToShortDateString() + " " +sameLesson.TimeInterval.StartTime);
            }
            foreach (var g in groups) {
                if (_context.Lessons.Include(x => x.Group).Where(l => l.Group.Contains(g) && l.TimeInterval == timeInterval && l.Date.Date == lesson.Date.Date).AsNoTracking().ToList().Count > 0) {
                    throw new ArgumentException("У группы " + g.Name + " уже существует пара на дату " + lesson.Date.ToShortDateString() + " "+ lesson.TimeInterval.StartTime);

                }
            }

            return check;
        }
        public async Task DeleteLessonWeek(DuplicateDTO duplicateDTO) {
            IList<Lesson> Lessons = null;
            if (duplicateDTO.SchedulleType == TypeSchedulleEnum.Teacher) {
                Lessons = await _scheduleService.GetLessonsProfessorDb(duplicateDTO.DateToCopy, duplicateDTO.Id, duplicateDTO.AmountOfWeeks);
            }
            else if (duplicateDTO.SchedulleType == TypeSchedulleEnum.Classroom) {

                Lessons = await _scheduleService.GetLessonsClassroomDb(duplicateDTO.DateToCopy, duplicateDTO.Id, duplicateDTO.AmountOfWeeks);
            }
            else if (duplicateDTO.SchedulleType == TypeSchedulleEnum.Group) {
                Lessons = await _scheduleService.GetLessonsGroupDb(duplicateDTO.DateToCopy, duplicateDTO.Id, duplicateDTO.AmountOfWeeks);
            }
            if (Lessons == null) throw new InvalidOperationException("На этой неделе нет пар для удаления");

            var error = Lessons.FirstOrDefault(x => x.IsReadOnly);
            if (error != null) {
                throw new InvalidOperationException("Пару, c датой " +error.Date.ToShortDateString() + " невозможно отредактировать, т.к она уже прошла");
            }
            _context.Lessons.RemoveRange(Lessons);
            await _context.SaveChangesAsync();
        }
        //domain
        public async Task CreateDomain(DomainDTO domain) {
            if (domain.Url == null) {
                throw new ArgumentNullException(nameof(domain));
            }
            var sameDomain = await _context.Domains.FirstOrDefaultAsync(x => x.Url == domain.Url);

            if (sameDomain != null) {
                throw new ArgumentException("this domain is already exist");
            }
            await _context.Domains.AddAsync(new Domain {
                Url = domain.Url
            });
            await _context.SaveChangesAsync();
        }
        public async Task EditDomain(DomainDTO domain, Guid id) {
            if (domain.Url == null) {
                throw new ArgumentNullException(nameof(domain));
            }
            var sameDomain = await _context.Domains.FirstOrDefaultAsync(x => x.Url == domain.Url);

            if (sameDomain != null) {
                throw new ArgumentException("this domain is already exist");
            }
            var Domain = await _context.Domains.FindAsync(id);
            if (Domain == null) throw new KeyNotFoundException("domain with this id does not exist");
            Domain.Url = domain.Url;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDomain(Guid id) {
            var domain = await _context.Domains.FindAsync(id);
            if (domain == null) {
                throw new KeyNotFoundException("this domain does not exist");
            }
            _context.Domains.Remove(domain);
            await _context.SaveChangesAsync();

        }
        //teacher
        public async Task CreateTeacher(TeacherDTO teacher) {
            if (teacher.Name == null) {
                throw new ArgumentNullException(nameof(teacher));
            }
            //var sameTeacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Name == teacher.Name);

            /*if (sameTeacher != null)
            {
                throw new ArgumentException("this Teacher is already exist");
            }*/
            await _context.Teachers.AddAsync(new Teacher {
                Name = Regex.Replace(teacher.Name, @"\s+", " ")
            });
            await _context.SaveChangesAsync();
        }
        public async Task EditTeacher(TeacherDTO teacher, Guid id) {
            if (teacher.Name == null) {
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
        public async Task DeleteTeacher(Guid id) {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) {
                throw new KeyNotFoundException("this teacher does not exist");
            }
            teacher.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
        // classroom
        public async Task CreateClassroom(ClassroomDTO classroom) {
            if (classroom.Name == null) {
                throw new ArgumentNullException(nameof(classroom));
            }
            var sameClassroom = await _context.Classrooms.FirstOrDefaultAsync(x => x.Name == classroom.Name);

            if (sameClassroom != null) {
                throw new ArgumentException("this classroom is already exist");
            }
            await _context.Classrooms.AddAsync(new Classroom {
                Name = classroom.Name
            });
            await _context.SaveChangesAsync();
        }
        public async Task EditClassroom(ClassroomDTO classroom, Guid id) {
            if (classroom.Name == null) {
                throw new ArgumentNullException(nameof(classroom));
            }
            var sameClassroom = await _context.Classrooms.FirstOrDefaultAsync(x => x.Name == classroom.Name);

            if (sameClassroom != null) {
                throw new ArgumentException("this classroom is already exist");
            }
            var Classroom = await _context.Classrooms.FindAsync(id);
            if (Classroom == null) throw new KeyNotFoundException("classroom with this id does not exist");
            Classroom.Name = classroom.Name;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteClassroom(Guid id) {
            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null) {
                throw new KeyNotFoundException("this classroom does not exist");
            }
            classroom.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
        //group
        public async Task CreateGroup(GroupDTO group) {
            if (group.Name == null) {
                throw new ArgumentNullException(nameof(group));
            }
            var sameGroup = await _context.Groups.FirstOrDefaultAsync(x => x.Name == group.Name);

            if (sameGroup != null) {
                throw new ArgumentException("this group is already exist");
            }
            await _context.Groups.AddAsync(new Group {
                Name = group.Name
            });
            await _context.SaveChangesAsync();
        }
        public async Task EditGroup(GroupDTO group, Guid id) {
            if (group.Name == null) {
                throw new ArgumentNullException(nameof(group));
            }
            var sameGroup = await _context.Groups.FirstOrDefaultAsync(x => x.Name == group.Name);

            if (sameGroup != null) {
                throw new ArgumentException("this group is already exist");
            }
            var Group = await _context.Groups.FindAsync(id);
            if (Group == null) throw new KeyNotFoundException("group with this id does not exist");
            Group.Name = group.Name;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteGroup(Guid id) {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) {
                throw new KeyNotFoundException("this group does not exist");
            }
            group.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
        //lessonName
        public async Task CreateLessonName(LessonNameDTO lessonName) {
            if (lessonName.Name == null) {
                throw new ArgumentNullException(nameof(lessonName));
            }
            var sameLessonName = await _context.LessonNames.FirstOrDefaultAsync(x => x.Name == lessonName.Name);

            if (sameLessonName != null) {
                throw new ArgumentException("this lessonName is already exist");
            }
            await _context.LessonNames.AddAsync(new LessonName {
                Name = lessonName.Name
            });
            await _context.SaveChangesAsync();
        }
        public async Task EditLessonName(LessonNameDTO lessonName, Guid id) {
            if (lessonName.Name == null) {
                throw new ArgumentNullException(nameof(lessonName));
            }
            var sameLessonName = await _context.Groups.FirstOrDefaultAsync(x => x.Name == lessonName.Name);

            if (sameLessonName != null) {
                throw new ArgumentException("this lessonName is already exist");
            }
            var LessonName = await _context.LessonNames.FindAsync(id);
            if (LessonName == null) throw new KeyNotFoundException("lessonName with this id does not exist");
            LessonName.Name = lessonName.Name;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteLessonName(Guid id) {
            var lessonName = await _context.LessonNames.FindAsync(id);
            if (lessonName == null) {
                throw new KeyNotFoundException("this lessonName does not exist");
            }
            lessonName.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
        //lessonTag
        public async Task CreateLessonTag(LessonTagDTO lessonTag) {
            if (lessonTag.Name == null) {
                throw new ArgumentNullException(nameof(lessonTag));
            }
            var sameLessonTag = await _context.LessonTags.FirstOrDefaultAsync(x => x.Name == lessonTag.Name);

            if (sameLessonTag != null) {
                throw new ArgumentException("this lessonTag is already exist");
            }
            await _context.LessonTags.AddAsync(new LessonTag {
                Name = lessonTag.Name
            });
            await _context.SaveChangesAsync();
        }
        public async Task EditLessonTag(LessonTagDTO lessonTag, Guid id) {
            if (lessonTag.Name == null) {
                throw new ArgumentNullException(nameof(lessonTag));
            }
            var sameLessonTag = await _context.LessonTags.FirstOrDefaultAsync(x => x.Name == lessonTag.Name);

            if (sameLessonTag != null) {
                throw new ArgumentException("this lessonTag is already exist");
            }
            var LessonTag = await _context.LessonTags.FindAsync(id);
            if (LessonTag == null) throw new KeyNotFoundException("lessonTag with this id does not exist");
            LessonTag.Name = lessonTag.Name;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteLessonTag(Guid id) {
            var lessonTag = await _context.LessonTags.FindAsync(id);
            if (lessonTag == null) {
                throw new KeyNotFoundException("this lessonTag does not exist");
            }
            lessonTag.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
        //timeInterval
        public async Task CreateTimeInterval(TimeIntervalDTO timeInterval) {
            if (timeInterval.StartTime == null || timeInterval.EndTime == null || timeInterval.StartTime >= timeInterval.EndTime) {
                throw new ArgumentNullException(nameof(timeInterval));
            }
            var sameInterval = await _context.TimeIntervals.FirstOrDefaultAsync(x => x.StartTime == timeInterval.StartTime && x.EndTime == timeInterval.EndTime);

            if (sameInterval != null) {
                throw new ArgumentException("this timeInterval is already exist");
            }
            var error = await _context.TimeIntervals.FirstOrDefaultAsync(x => x.StartTime <= timeInterval.EndTime && x.EndTime >= timeInterval.StartTime || x.StartTime <= timeInterval.StartTime && x.EndTime >= timeInterval.EndTime || x.StartTime >= timeInterval.StartTime && x.EndTime <= timeInterval.EndTime);
            if (error != null) {
                throw new ArgumentException("this timeInterval intersects another one");
            }
            await _context.TimeIntervals.AddAsync(new TimeInterval {
                StartTime = timeInterval.StartTime,
                EndTime = timeInterval.EndTime,

            });
            await _context.SaveChangesAsync();
        }
        public async Task EditTimeInterval(TimeIntervalDTO timeInterval, Guid id) {
            if (timeInterval.StartTime == null || timeInterval.EndTime == null || timeInterval.StartTime >= timeInterval.EndTime) {
                throw new ArgumentNullException(nameof(timeInterval));
            }
            var sameInterval = await _context.TimeIntervals.FirstOrDefaultAsync(x => x.StartTime == timeInterval.StartTime && x.EndTime == timeInterval.EndTime);

            if (sameInterval != null && sameInterval.Id != id) {
                throw new ArgumentException("this timeInterval is already exist");
            }
            var error = await _context.TimeIntervals.FirstOrDefaultAsync(x => x.StartTime <= timeInterval.EndTime && x.EndTime >= timeInterval.StartTime || x.StartTime <= timeInterval.StartTime && x.EndTime >= timeInterval.EndTime || x.StartTime >= timeInterval.StartTime && x.EndTime <= timeInterval.EndTime);

            if (error != null && error.Id != id) {
                throw new ArgumentException("this timeInterval intersects another one");
            }

            var TimeInterval = await _context.TimeIntervals.FindAsync(id);
            if (TimeInterval == null) throw new KeyNotFoundException("timeInterval with this id does not exist");
            TimeInterval.StartTime = timeInterval.StartTime;
            TimeInterval.EndTime = timeInterval.EndTime;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteTimeInterval(Guid id) {
            var timeInterval = await _context.TimeIntervals.FindAsync(id);
            if (timeInterval == null) {
                throw new KeyNotFoundException("this timeInterval does not exist");
            }
            _context.TimeIntervals.Remove(timeInterval);
            await _context.SaveChangesAsync();
        }
    }
}
