using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models.Entities;

public partial class Authentication
{
    public int Id { get; set; }

    public int LoginUserId { get; set; }

    public DateTime? LoginTime { get; set; }

    public string? AuthKey { get; set; }

    public DateTime? LogoutTime { get; set; }

    public bool? IsActive { get; set; }
    public virtual User LoginUser { get; set; } = null!;
}
