namespace bookstore.api.Models;

public class PriceModel : BaseModel
{
    public PriceModel()
    {
        Price = 0;
        ForDay = 0;
        ForAllDays = false;
    }

    public virtual int Price { get; set; }
    public virtual int ForDay { get; set; }
    public virtual bool ForAllDays { get; set; }

    public virtual BookModel Book { get; set; }

}
