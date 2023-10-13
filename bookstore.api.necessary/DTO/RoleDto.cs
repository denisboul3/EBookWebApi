using bookstore.api.necessary.DTO;
using System;
using System.ComponentModel.DataAnnotations;
namespace bookstore.api.DTO;

public class CreateRoleDto 
{
    [Required]
    public string? Name { get; set; }
}

public class AssignRoleDto
{
    [Required]
    public Guid RoleId { get; set; }
    public Guid AssignedTo { get; set; }
}