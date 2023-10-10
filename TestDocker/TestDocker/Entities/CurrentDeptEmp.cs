namespace TestDocker.Entities
{
    public class CurrentDeptEmp
    {
        public int EmpNo { get; set; }

        public string DeptNo { get; set; } = null!;

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
