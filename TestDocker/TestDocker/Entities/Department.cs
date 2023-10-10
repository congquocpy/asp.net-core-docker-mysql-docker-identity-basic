namespace TestDocker.Entities
{
    public class Department
    {
        public string DeptNo { get; set; } = null!;

        public string DeptName { get; set; } = null!;

        public virtual ICollection<DeptEmp> DeptEmps { get; set; } = new List<DeptEmp>();

        public virtual ICollection<DeptManager> DeptManagers { get; set; } = new List<DeptManager>();
    }
}
