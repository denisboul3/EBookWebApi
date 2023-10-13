using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace bookstore.api.Models;

public abstract class BaseModel
{
    public virtual Guid Id { get; set; }
}