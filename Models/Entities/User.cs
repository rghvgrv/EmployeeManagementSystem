using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string PasswordHash { get; set; }

    public int EmpId { get; set; }
    public virtual ICollection<Authentication> Authentications { get; set; } = new List<Authentication>();
    public virtual Employee Emp { get; set; } = null!;

    [NotMapped]
    public virtual ICollection<Employee> EmployeeCreatedByNavigations { get; set; } = new List<Employee>();
    [NotMapped]
    public virtual ICollection<Employee> EmployeeModifiedByNavigations { get; set; } = new List<Employee>();
}
