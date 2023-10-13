
namespace bookstore.api.Models;

public class UserModel : BaseModel
{
    public UserModel()
    {
        Login = string.Empty;
        Password = string.Empty;
        Email = string.Empty;
        USDTBalance = 0;
        WalletAddress = string.Empty;
    }

    public virtual string Login { get; set; }
    public virtual string Password { get; set; }
    public virtual string Email { get; set; }
    public virtual int USDTBalance { get; set; }
    public virtual string WalletAddress { get; set; }

    public virtual RoleModel Role { get; set; }

    public virtual void SetRole(RoleModel userRoleToBeInjected)
    {
        Role = userRoleToBeInjected;
    }
}
