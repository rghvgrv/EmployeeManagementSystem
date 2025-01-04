using System;
using System.Collections.Generic;

namespace EmployeeManagementSystem.Models.Entities;

public partial class Authentication
{
    public int Id { get; set; }

    public int LoginUserId { get; set; }

    public DateTime? LoginTime { get; set; }

    public virtual User LoginUser { get; set; } = null!;
}
