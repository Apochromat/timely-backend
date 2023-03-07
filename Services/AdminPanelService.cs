using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using timely_backend.Models;
using timely_backend.Models.DTO;
using timely_backend.Models.Enum;

namespace timely_backend.Services; 

public class AdminPanelService : IAdminPanelService {
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<AdminPanelService> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IAdminService _adminService;
    private readonly IConfiguration _configuration;

    public AdminPanelService(ILogger<AdminPanelService> logger, ApplicationDbContext context,
        UserManager<User> userManager, RoleManager<Role> roleManager, IAdminService adminService, IConfiguration configuration) {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _adminService = adminService;
        _configuration = configuration;
    }

    public List<string> GetRoles() {
        return _context.Roles.Select(x => x.Type.ToString()).ToList();;
    }

    public async Task<List<UserWithRolesDTO>> GetUsersWithRoles() {

        var users = _userManager.Users.Include(u => u.Roles).ThenInclude(r => r.Role).Select(u => new UserWithRolesDTO() {
            FullName = u.FullName,
            Email = u.Email,
            Roles = u.Roles.Select(r => r.Role).Select(role => role.Name.ToString()).ToList()
        }).ToList();

        return users;
    }

    public async Task<Response> SetUserRoles(UserWithRolesEditDTO userWithRolesEditDto) {
        if (userWithRolesEditDto.Email == _configuration.GetSection("DefaultUsersConfig")["AdminEmail"]) {
            throw new InvalidOperationException("Impossible to edit main administrator account");
        }
        
        var user = await _userManager.FindByEmailAsync(userWithRolesEditDto.Email);
        if (user == null) throw new KeyNotFoundException("User not found");
        user = _userManager.Users.Where(x => x.Email == userWithRolesEditDto.Email).Include(x => x.Group)
            .Include(x => x.Teacher).First();

        foreach (var role in userWithRolesEditDto.Roles) {
            if (await _roleManager.RoleExistsAsync(role) == false) throw new ArgumentException($"Role {role} does not exist");
        }

        if (!(await _userManager.GetRolesAsync(user)).Contains(RoleType.Teacher.ToString()) &&
            userWithRolesEditDto.Roles.Contains(RoleType.Teacher.ToString()) && user.Teacher == null) {
            
            if (_context.Teachers.Where(t => t.Name == user.FullName).IsNullOrEmpty()) {
                await _adminService.CreateTeacher(
                    new TeacherDTO {
                        Name = user.FullName
                    });
            }

            user.Teacher = _context.Teachers.Where(t => t.Name == user.FullName).FirstOrDefault();
        }

        var removeResult = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
        if (!removeResult.Succeeded) {
            throw new ArgumentException(string.Join(", ", removeResult.Errors.Select(x => x.Description)));
        }
        
        var addResult = await _userManager.AddToRolesAsync(user, userWithRolesEditDto.Roles);
        if (addResult.Succeeded) {
            _logger.LogInformation("Successful roles editing");

            return new Response {
                Status = "Ok",
                Message = "Successful roles editing"
            };
        }

        var errors = string.Join(", ", addResult.Errors.Select(x => x.Description));
        throw new ArgumentException(errors);
    }
}