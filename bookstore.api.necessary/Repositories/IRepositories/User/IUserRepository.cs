using bookstore.api.Models;
using bookstore.api.Repositories;

namespace bookstore.api.necessary.Repositories.IRepositories.User;

public interface IUserRepository : IRepository<UserModel>
{
    void CreateUser(UserModel user);

    UserModel FindByLoginPassword(string login, string password);

}