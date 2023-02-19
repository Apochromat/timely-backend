using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using timely_backend.Models;

namespace timely_backend.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;

       
        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }
        

        [HttpPost]
        [Route("teacher/create/{id}")]
        public async Task<ActionResult> CreateTeacher(Guid id , [FromBody] Teacher teacher)
        {

        }
    }
}
