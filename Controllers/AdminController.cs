﻿using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using timely_backend.Models.DTO;
using timely_backend.Models.Enum;
using timely_backend.Services;

namespace timely_backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Administrator)]
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
        
        //teacher 1
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
        //domain 2
        [HttpPost]
        [Route("domain/create")]
        public async Task<IActionResult> CreateDomain([FromBody] DomainDTO domain)
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
        //classroom 3
        [HttpPost]
        [Route("classroom/create")]
        public async Task<IActionResult> CreateClassroom([FromBody] ClassroomDTO classroom)
        {
            try
            {
                await _adminService.CreateClassroom(classroom);
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
        [Route("classroom/edit/{id}")]
        public async Task<IActionResult> EditClassroom([FromBody] ClassroomDTO classroom, Guid id)
        {
            try
            {
                await _adminService.EditClassroom(classroom, id);
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
        [Route("classroom/delete/{id}")]
        public async Task<IActionResult> DeleteClassroom(Guid id)
        {
            try
            {
                await _adminService.DeleteClassroom(id);
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
        //group 4
        [HttpPost]
        [Route("group/create")]
        public async Task<IActionResult> CreateGroup([FromBody] GroupDTO group)
        {
            try
            {
                await _adminService.CreateGroup(group);
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
        [Route("group/edit/{id}")]
        public async Task<IActionResult> EditGroup([FromBody] GroupDTO group, Guid id)
        {
            try
            {
                await _adminService.EditGroup(group, id);
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
        [Route("group/delete/{id}")]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            try
            {
                await _adminService.DeleteGroup(id);
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
        // lessonName 5
        [HttpPost]
        [Route("lessonName/create")]
        public async Task<IActionResult> CreateLessonName([FromBody] LessonNameDTO lessonName)
        {
            try
            {
                await _adminService.CreateLessonName(lessonName);
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
        [Route("lessonName/edit/{id}")]
        public async Task<IActionResult> EditLessonName([FromBody] LessonNameDTO lessonName, Guid id)
        {
            try
            {
                await _adminService.EditLessonName(lessonName, id);
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
        [Route("lessonName/delete/{id}")]
        public async Task<IActionResult> DeleteLessonName(Guid id)
        {
            try
            {
                await _adminService.DeleteLessonName(id);
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
        // lessonTag 6
        [HttpPost]
        [Route("lessonTag/create")]
        public async Task<IActionResult> CreateLessonTag([FromBody] LessonTagDTO lessonTag)
        {
            try
            {
                await _adminService.CreateLessonTag(lessonTag);
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
        [Route("lessonTag/edit/{id}")]
        public async Task<IActionResult> EditLessonTag([FromBody] LessonTagDTO lessonTag, Guid id)
        {
            try
            {
                await _adminService.EditLessonTag(lessonTag, id);
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
        [Route("lessonTag/delete/{id}")]
        public async Task<IActionResult> DeleteLessonTag(Guid id)
        {
            try
            {
                await _adminService.DeleteLessonTag(id);
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
