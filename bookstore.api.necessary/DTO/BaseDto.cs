using System;
using System.ComponentModel.DataAnnotations;

namespace bookstore.api.necessary.DTO;

public interface BaseDto
{
    [Key]
    Guid Id { get; set; }
    [Editable(false)]
    string Message { get; set; }
}
