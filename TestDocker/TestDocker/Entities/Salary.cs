namespace TestDocker.Entities
{
    public class Salary
    {
        public int EmpNo { get; set; }

        public int Salary1 { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public virtual Employee EmpNoNavigation { get; set; } = null!;
    }
}
