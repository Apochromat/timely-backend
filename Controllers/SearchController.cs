using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using timely_backend.Models.DTO;
using timely_backend.Services;

namespace timely_backend.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;
        private readonly IScheduleService _scheduleService;

        public SearchController(ILogger<SearchController> logger, IScheduleService scheduleService)
        {
            _logger = logger;
            _scheduleService = scheduleService;

        }
        [HttpGet]
        [Route("teachers")]
        public async Task<ActionResult<IList<TeacherDTO>>> GetTeachers()
        {
            try
            {
              return Ok(await _scheduleService.GetTeacher());
                
                
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong while getting teachers");
            }
        }
        [HttpGet]
        [Route("groups")]
        public async Task<ActionResult<IList<GroupDTO>>> GetGroups()
        {
            try
            {
               return Ok(await _scheduleService.GetGroup());
                
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong while getting groups");
            }
        }
        [HttpGet]
        [Route("lessonNames")]
        public async Task<ActionResult<IList<LessonNameDTO>>> GetLessonNames()
        {
            try
            {
               return Ok(await _scheduleService.GetLessonName());
                
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong while getting lessonNames");
            }
        }
        [HttpGet]
        [Route("lessonTags")]
        public async Task<ActionResult<IList<LessonTagDTO>>> GetLessonTags()
        {
            try
            {
                return Ok(await _scheduleService.GetLessonTag());
                
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong while getting lessonTags");
            }
        }
        [HttpGet]
        [Route("classrooms")]
        public async Task<ActionResult<IList<ClassroomDTO>>> GetClassrooms()
        {
            try
            {
                return Ok(await _scheduleService.GetClassroom());
                
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong while getting classrooms");
            }
        }
        [HttpGet]
        [Route("timeIntervals")]
        public async Task<ActionResult<IList<ClassroomDTO>>> GetTimeIntervals()
        {
            try
            {
                return Ok(await _scheduleService.GetTimeInterval());
                
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong while getting timeInterval");
            }
        }
    }
}
