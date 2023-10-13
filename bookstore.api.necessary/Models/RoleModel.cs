using bookstore.api.Mediator.Queries;

namespace bookstore.api.Models;

public class RoleModel : BaseModel
{
    public RoleModel()
    {
        Name = String.Empty;
        CreatedDate = DateTime.UtcNow;
        CreatedBy = Guid.Empty;
    }

    public virtual string Name { get; set; }
    public virtual DateTime CreatedDate { get; set; }
    public virtual Guid CreatedBy { get; set; }

    public virtual void SetAudit(Guid createdBy)
    {
        this.CreatedBy = createdBy;
        this.CreatedDate = DateTime.UtcNow;
    }

}