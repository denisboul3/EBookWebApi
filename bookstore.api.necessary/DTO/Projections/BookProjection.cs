using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookstore.api.necessary.DTO.Projections;

public class BookProjection
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Author { get; set; }
    public string? ImgUri { get; set; }
    public string? Genre { get; set; }
    public List<PriceProjection>? Price { get; set; }
}