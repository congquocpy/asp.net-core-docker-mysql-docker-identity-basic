namespace TestDocker.Entities
{
    public class DeptManager
    {
        public int EmpNo { get; set; }

        public string DeptNo { get; set; } = null!;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public virtual Department DeptNoNavigation { get; set; } = null!;

        public virtual Employee EmpNoNavigation { get; set; } = null!;
    }
}
