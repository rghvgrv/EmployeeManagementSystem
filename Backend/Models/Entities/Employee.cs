using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models.Entities;

public partial class Employee
{
    public int EmpId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Department { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public int? CreatedBy { get; set; }
    [NotMapped]
    public virtual User? CreatedByNavigation { get; set; }
    [NotMapped]
    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
