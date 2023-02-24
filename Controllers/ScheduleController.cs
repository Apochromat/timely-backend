using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using timely_backend.Models;
using timely_backend.Models.DTO;
using timely_backend.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace timely_backend.Controllers
{
    [Route("api")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IScheduleService _scheduleService;

        public ScheduleController(ILogger<ScheduleController> logger, IScheduleService scheduleService)
        {
            _logger = logger;
            _scheduleService = scheduleService;

        }
        [Route("schedule/group/{groupId}")]
        [HttpGet]
        public async Task<ActionResult<IList<LessonDTO>>> GetLessonsGroup(DateTime date,Guid groupId)
        {
            try
            {
               return Ok(await _scheduleService.GetLessonsGroup(date , groupId));
                
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 404, title: e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong while getting LessonsByGroup");
            }

        }
        [Route("schedule/classroom/{classroomId}")]
        [HttpGet]
        public async Task<ActionResult<IList<LessonDTO>>> GetLessonsClassroom(DateTime date,Guid classroomId)
        {
            try
            {
                return Ok(await _scheduleService.GetLessonsClassroom(date, classroomId));
                
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 404, title: e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong while getting LessonsByaudience");
            }
        }
        [Route("schedule/teacher/{teacherId}")]
        [HttpGet]
        public async Task<ActionResult<IList<LessonDTO>>> GetLessonsProfessor(DateTime date, Guid teacherId)
        {
            try
            {
                return Ok(await _scheduleService.GetLessonsProfessor(date, teacherId));
                
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 404, title: e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong while getting LessonsByTeacher");
            }
        }


       
    }
}
