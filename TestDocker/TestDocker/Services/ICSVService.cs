namespace TestDocker.Services
{
    public interface ICSVService
    {
        void WriteCSV<T>(List<T> records);
    }
}
