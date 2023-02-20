using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using timely_backend.Models.DTO;
using timely_backend.Services;

namespace timely_backend.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminService _adminService;
       
        public AdminController(ILogger<AdminController> logger,IAdminService adminService)
        {
            _logger = logger;
            _adminService = adminService;
            
        }
        

        [HttpPost]
        [Route("teacher/create")]
        public async Task<IActionResult> CreateTeacher([FromBody] TeacherDTO teacher)
        {
            try
            {
                await _adminService.CreateTeacher(teacher);
                return Ok();
            }
            catch(ArgumentNullException e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 400, title: e.Message);
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 409, title: e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }

        [HttpPut]
        [Route("teacher/edit/{id}")]
        public async Task<IActionResult> EditTeacher([FromBody] TeacherDTO teacher, Guid id)
        {
            try
            {
                await _adminService.EditTeacher(teacher,id);
                return Ok();
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 400, title: e.Message);
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 400, title: e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }
        [HttpDelete]
        [Route("teacher/delete/{id}")]
        public async Task<IActionResult> DeleteTeacher(Guid id)
        {
            try
            {
                await _adminService.DeleteTeacher(id);
                return Ok();
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 400, title: e.Message);
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 400, title: e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }
        [HttpPost]
        [Route("domain/create")]
        public async Task<IActionResult> CreateDomain(DomainDTO domain)
        {
            try
            {
                await _adminService.CreateDomain(domain);
                return Ok();
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 400, title: e.Message);
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 400, title: e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }

        [HttpPut]
        [Route("domain/edit/{id}")]
        public async Task<IActionResult> EditDomain([FromBody] DomainDTO domain, Guid id)
        {
            try
            {
                await _adminService.EditDomain(domain, id);
                return Ok();
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 400, title: e.Message);
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 400, title: e.Message);
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 409, title: e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }
        [HttpDelete]
        [Route("domain/delete/{id}")]
        public async Task<IActionResult> EditDomain(Guid id)
        {
            try
            {
                await _adminService.DeleteDomain(id);
                return Ok();
            }
           
            catch (KeyNotFoundException e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 400, title: e.Message);
            }
            
            catch (Exception e)
            {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }
    }
}
