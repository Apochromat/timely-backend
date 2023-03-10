using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using timely_backend.Models.DTO;
using timely_backend.Models.Enum;
using timely_backend.Services;

namespace timely_backend.Controllers {
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Administrator)]
    [Route("api/admin")]
    public class AdminController : ControllerBase {
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminPanelService _adminPanelService;

        public AdminController(ILogger<AdminController> logger,
            IAdminPanelService adminPanelService) {
            _logger = logger;
            _adminPanelService = adminPanelService;
        }

        [HttpGet]
        [Route("roles")]
        public async Task<ActionResult<List<String>>> GetRoles() {
            try {
                var roles = _adminPanelService.GetRoles();
                return roles;
            }
            catch (Exception e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }

        [HttpGet]
        [Route("users")]
        public async Task<ActionResult<List<UserWithRolesDTO>>> GetUsers() {
            try {
                var roles = await _adminPanelService.GetUsersWithRoles();
                return roles;
            }
            catch (Exception e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }

        [HttpPut]
        [Route("user-roles")]
        public async Task<ActionResult<Response>> SetUserRoles([FromBody] UserWithRolesEditDTO userWithRolesEditDto) {
            try {
                return await _adminPanelService.SetUserRoles(userWithRolesEditDto);
            }
            catch (ArgumentException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 400, title: e.Message);
            }
            catch (KeyNotFoundException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 404, title: e.Message);
            }
            catch (InvalidOperationException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 405, title: e.Message);
            }
            catch (Exception e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }
    }
}