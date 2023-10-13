using bookstore.api.Models;
using System.Reflection;
using System.Reflection.Metadata;

namespace bookstore.api.Models;

public class BookModel : BaseModel
{
    public BookModel()
    {
        Name = string.Empty;
        ImgUri = string.Empty;
        Author = string.Empty;
        Genre = string.Empty;

        Prices = new HashSet<PriceModel>();
    }

    public virtual string Name { get; set; }
    public virtual string ImgUri { get; set; }
    public virtual string Author { get; set; }
    public virtual string Genre { get; set; }

    public virtual ISet<PriceModel> Prices { get; set; }
}
