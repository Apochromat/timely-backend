using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using timely_backend.Models.DTO;
using timely_backend.Models.Enum;
using timely_backend.Services;

namespace timely_backend.Controllers {
    /// <summary>
    /// Controller for auth and profile
    /// </summary>
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _account;

        /// <summary>
        /// Controller for auth and profile
        /// </summary>
        public AccountController(ILogger<AccountController> logger, IAccountService accountService) {
            _logger = logger;
            _account = accountService;
        }

        /// <summary>
        /// Register new user.
        /// </summary>
        /// <returns></returns>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "409" > If there is user with same email</response>
        /// <response code = "500" > Internal Server Error</response>
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<TokenResponse>> Register([FromBody] UserRegisterModel userRegisterModel) {
            try {
                return await _account.Register(userRegisterModel);
            }
            catch (InvalidOperationException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 400, title: e.Message);
            }
            catch (ArgumentException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 409, title: e.Message);
            }
            catch (Exception e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }
        
        /// <summary>
        /// Send Email confirmation letter
        /// </summary>
        /// <returns></returns>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "409" > If Email already confirmed</response>
        /// <response code = "500" > Internal Server Error</response>
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("send-email-confirmation")]
        public async Task<ActionResult<Response>> SendConfirmationEmailLetter() {
            try {
                return await _account.SendEmailConfirmationLetter(User.Identity.Name);
            }
            catch (KeyNotFoundException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 404, title: e.Message);
            }
            catch (InvalidOperationException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 409, title: e.Message);
            }
            catch (Exception e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }
        
        /// <summary>
        /// Confirm user`s Email.
        /// </summary>
        /// <returns></returns>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "409" > If Email already confirmed</response>
        /// <response code = "500" > Internal Server Error</response>
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("confirm-email")]
        public async Task<ActionResult<Response>> ConfirmEmail([FromBody] EmailComfirmationToken token) {
            try {
                return await _account.ConfirmEmail(User.Identity.Name, token.Token);
            }
            catch (InvalidOperationException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 409, title: e.Message);
            }
            catch (Exception e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }

        /// <summary>
        /// Log in to the system.
        /// </summary>
        /// <returns></returns>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "401" > If email or password are incorrect</response>
        /// <response code = "500" > Internal Server Error</response>
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginCredentials loginCredentials) {
            try {
                return await _account.Login(loginCredentials);
            }
            catch (ArgumentException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 401, title: e.Message);
            }
            catch (Exception e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }

        /// <summary>
        /// Log out system user.
        /// </summary>
        /// <returns></returns>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "401" > If user unauthorized</response>
        /// <response code = "500" > Internal Server Error</response>
        [HttpPost]
        [Route("logout")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response>> Logout() {
            try {
                return await _account.Logout(Request.Headers.Authorization);
            }
            catch (Exception e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }

        /// <summary>
        /// Get user profile.
        /// </summary>
        /// <returns></returns>
        /// <response code = "401" > Unauthorized</response>
        /// <response code = "500" > Internal Server Error</response>
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("profile")]
        public async Task<ActionResult<UserProfile>> GetAccountProfile() {
            try {
                return await _account.GetProfile(User.Identity.Name);
            }
            catch (KeyNotFoundException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 404, title: e.Message);
            }
            catch (Exception e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }


        /// <summary>
        /// Edit user profile.
        /// </summary>
        /// <returns></returns>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "401" > Unauthorized</response>
        /// <response code = "403" > Forbidden</response>
        /// <response code = "500" > Internal Server Error</response>
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("profile")]
        public async Task<ActionResult<Response>> EditAccountProfile([FromBody] UserProfileEdit userProfileEdit) {
            try {
                return await _account.EditProfile(User.Identity.Name, userProfileEdit);
            }
            catch (KeyNotFoundException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext?.TraceIdentifier}");
                return Problem(statusCode: 404, title: e.Message);
            }
            catch (Exception e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext?.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }
        
        /// <summary>
        /// Change user profile password.
        /// </summary>
        /// <returns></returns>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "401" > Unauthorized</response>
        /// <response code = "500" > Internal Server Error</response>
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("password-change")]
        public async Task<ActionResult<Response>> EditAccountPassword([FromBody] UserPasswordEdit userPasswordEdit) {
            try {
                return await _account.EditPassword(User.Identity.Name, userPasswordEdit);
            }
            catch (InvalidOperationException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext?.TraceIdentifier}");
                return Problem(statusCode: 400, title: e.Message);
            }
            catch (Exception e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext?.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }
        
        /// <summary>
        /// Set user`s group.
        /// </summary>
        /// <returns></returns>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "401" > Unauthorized</response>
        /// <response code = "403" > This endpoint only for Students</response>
        /// <response code = "500" > Internal Server Error</response>
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Student)]
        [Route("group/set")]
        public async Task<ActionResult<Response>> SetGroup([FromBody] GroupChangeDTO groupChangeDto) {
            try {
                return await _account.SetGroup(User.Identity.Name, groupChangeDto.GroupId);
            }
            catch (InvalidOperationException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext?.TraceIdentifier}");
                return Problem(statusCode: 400, title: e.Message);
            }
            catch (Exception e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext?.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }
        
        /// <summary>
        /// Remove user`s group.
        /// </summary>
        /// <returns></returns>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "401" > Unauthorized</response>
        /// <response code = "403" > This endpoint only for Students</response>
        /// <response code = "500" > Internal Server Error</response>
        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Student)]
        [Route("group/remove")]
        public async Task<ActionResult<Response>> RemoveGroup() {
            try {
                return await _account.RemoveGroup(User.Identity.Name);
            }
            catch (InvalidOperationException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext?.TraceIdentifier}");
                return Problem(statusCode: 400, title: e.Message);
            }
            catch (Exception e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext?.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }
        
    }
}