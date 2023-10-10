namespace TestDocker.Entities
{
    public class Employee
    {
        public int EmpNo { get; set; }

        public DateTime BirthDate { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public DateTime HireDate { get; set; }

        public virtual ICollection<DeptEmp> DeptEmps { get; set; } = new List<DeptEmp>();

        public virtual ICollection<DeptManager> DeptManagers { get; set; } = new List<DeptManager>();

        public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

        public virtual ICollection<Title> Titles { get; set; } = new List<Title>();
    }
}
