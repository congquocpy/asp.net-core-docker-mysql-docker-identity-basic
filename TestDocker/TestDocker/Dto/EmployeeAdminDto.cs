namespace TestDocker.Dto
{
    public class EmployeeAdminDto
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string BirthDay { get; set; }
        public string Gender { get; set; }
        public string HireDate { get; set; }
        public string CurrentDepartment { get; set; }
        public bool IsManager { get; set; }
        public string CurrentPosition { get; set; }
        public int CurrentSalaly { get; set; }
    }
}
