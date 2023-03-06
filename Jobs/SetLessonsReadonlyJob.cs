using Microsoft.EntityFrameworkCore;
using Quartz;

namespace timely_backend.Jobs;

public class SetLessonsReadonlyJob : IJob {
    private readonly ILogger<SetLessonsReadonlyJob> _logger;
    private readonly ApplicationDbContext _dbcontext;

    public SetLessonsReadonlyJob(ILogger<SetLessonsReadonlyJob> logger, ApplicationDbContext dbcontext) {
        _dbcontext = dbcontext;
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context) {
        var time = TimeOnly.FromDateTime(DateTime.Now);
        var lessons = _dbcontext.Lessons.Include(l => l.TimeInterval).Where(l => l.IsReadOnly == false && DateOnly.FromDateTime(l.Date) <= DateOnly.FromDateTime(DateTime.Now) && l.TimeInterval.EndTime < time);
        foreach (var lesson in lessons) {
            lesson.IsReadOnly = true;
        }

        _dbcontext.SaveChanges();
        return Task.FromResult(true);
    }
}