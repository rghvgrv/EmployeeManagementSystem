namespace EmployeeManagementSystem.Models.DTOs
{
    public class EmployeeDTO
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

        public EmployeeDTO(int id,string firstName,string lastName,string role,string department,DateTime createdDate, DateTime? modifiedDate,int? modifiedBy,int? createdBy)
        {
            EmpId = id;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
            Department = department;
            CreatedDate = createdDate;
            ModifiedDate = modifiedDate;
            ModifiedBy = modifiedBy;    
            CreatedBy = createdBy;
        }
    }
    public class EmployeeCreateDTO
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Department { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public int? CreatedBy { get; set; }
    }
}
