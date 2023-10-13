using bookstore.api.Models;
using bookstore.api.Repositories;

namespace bookstore.api.necessary.Repositories.IRepositories.Role;

public interface IRoleRepository : IRepository<RoleModel>
{
    void CreateRole(RoleModel role);
    RoleModel GetUserRole();
}