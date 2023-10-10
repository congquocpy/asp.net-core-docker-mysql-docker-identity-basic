namespace TestDocker.Entities
{
    public class Title
    {
        public int EmpNo { get; set; }

        public string Title1 { get; set; } = null!;

        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public virtual Employee EmpNoNavigation { get; set; } = null!;
    }
}
