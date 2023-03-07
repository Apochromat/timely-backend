using timely_backend.Models.DTO;

namespace timely_backend.Services; 

public interface IAdminPanelService {
    List<String> GetRoles();
    Task<List<UserWithRolesDTO>> GetUsersWithRoles();
    Task<Response> SetUserRoles(UserWithRolesEditDTO userWithRolesDto);
}