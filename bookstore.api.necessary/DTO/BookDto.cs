using bookstore.api.Models;
using bookstore.api.necessary.DTO.Projections;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace bookstore.api.DTO;

public class CreateBookPriceDto
{
    [Required]
    public Guid BookId { get; set; }
    [Required]
    public int Price { get; set; }
    [Required]
    public int ForDay { get; set; }
    [Required]
    public bool ForAllDays { get; set; }
}

public class EditBookPriceDto : PriceProjection
{
}

public class CreateBookDto
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Author { get; set; }
    [Required]
    public string? ImgUri { get; set; }
    [Required]
    public string? Genre { get; set; }
}

public class EditBookDto : CreateBookDto 
{
    [Required]
    public Guid Id { get; set; }
}