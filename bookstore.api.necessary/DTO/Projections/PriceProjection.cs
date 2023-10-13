using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookstore.api.necessary.DTO.Projections;

public class PriceProjection
{
    public Guid Id { get; set; }
    public int Price { get; set; }
    public int ForDay { get; set; }
    public bool ForAllDays { get; set; }

}

