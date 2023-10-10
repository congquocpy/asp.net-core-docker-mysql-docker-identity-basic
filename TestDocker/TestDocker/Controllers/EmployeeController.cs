using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestDocker.Contants;
using TestDocker.Dto;
using TestDocker.Entities;
using TestDocker.Services;

namespace TestDocker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly DemoContext _context;
        private readonly ICSVService _csvService;
        public EmployeeController(DemoContext context, ICSVService csvService)
        {
            _context = context;
            _csvService = csvService;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetEmployy([FromQuery] IEnumerable<string> departmentId)
        {
            var data = _context.Departments.Include(a => a.DeptEmps).ThenInclude(a => a.EmpNoNavigation)
                                                                   .ThenInclude(a => a.Titles)
                                                                   .AsNoTracking();
            if (data != null && departmentId.Any())
            {
                data = data.Where(a => departmentId.Contains(a.DeptNo));
            }
            var datetime = DateTime.Now;
            if (User.IsInRole(UserRole.Admin))
            {
                List<EmployeeAdminDto> result = await data.SelectMany(x => x.DeptEmps.Select(e => new EmployeeAdminDto
                {
                    Id = e.EmpNo,
                    Fullname = e.EmpNoNavigation.FirstName + " " + e.EmpNoNavigation.LastName,
                    BirthDay = e.EmpNoNavigation.BirthDate.ToString("dd/MM/yyyy"),
                    HireDate = e.EmpNoNavigation.HireDate.ToString("dd/MM/yyyy"),
                    Gender = e.EmpNoNavigation.Gender,
                    CurrentDepartment = e.DeptNoNavigation.DeptName,
                    CurrentPosition = e.EmpNoNavigation.Titles.FirstOrDefault().Title1,
                    CurrentSalaly = e.EmpNoNavigation.Salaries.Max(a => a.Salary1),
                    IsManager = e.DeptNoNavigation.DeptManagers.Where(a => a.FromDate < datetime && a.ToDate > datetime && a.EmpNo  == e.EmpNo).Any(),

                })).ToListAsync();
                _csvService.WriteCSV<EmployeeAdminDto>(result);
                if (result.Count > 0)
                {
                    return Ok("co data");

                }
                else
                {
                    return Ok("0 co data");
                }

            }
            else
            {
                List<EmployeeUserDto> result = await data.SelectMany(x => x.DeptEmps.Select(e => new EmployeeUserDto
                {
                    Id = e.EmpNo,
                    Fullname = e.EmpNoNavigation.FirstName + " " + e.EmpNoNavigation.LastName,
                    BirthDay = e.EmpNoNavigation.BirthDate.ToString("dd/MM/yyyy"),
                    Gender = e.EmpNoNavigation.Gender,
                    CurrentDepartment = e.DeptNoNavigation.DeptName,
                    CurrentPosition = e.EmpNoNavigation.Titles.FirstOrDefault().Title1,
                })).ToListAsync();
                _csvService.WriteCSV<EmployeeUserDto>(result);
            }
            return Ok();
        }
    }
}
