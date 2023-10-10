using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TestDocker.Contants;
using TestDocker.Dto;
using TestDocker.Entities;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    public class PasswordChange
    {
        public string PasswordNew { get; set; }
    }
    private readonly DemoContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;


    public UserController(DemoContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    [HttpPost("Register")]
    public async Task<IActionResult> CreateUser([FromBody] UserDto user)
    {
        var userExists = await _context.UserApp.Where(a => a.UserName == user.Gmail).FirstOrDefaultAsync();
        if (userExists != null)
        {
            return BadRequest("User exits");
        }
        var data = new UserApp()
        {
            UserName = user.Gmail,
            Email = user.Gmail,
            SecurityStamp = Guid.NewGuid().ToString(),

        };
        var result = await _userManager.CreateAsync(data, user.Password);

        if (result.Succeeded)
        {
            if (await _roleManager.RoleExistsAsync(UserRole.Admin))
            {
                await _userManager.AddToRoleAsync(data, UserRole.User);
            }
            return Ok("Add User Success");
        }
        else
        {
            return BadRequest("Add User Fail");
        }
    }
    [Authorize(Roles = UserRole.Admin)]
    [HttpGet]
    public async Task<IActionResult> GetListUser()
    {
        var data = await _context.UserApp.ToListAsync();
        return Ok(data);
    }
    [Authorize(Roles = UserRole.User)]
    [HttpGet("id")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var data = await _context.UserApp.Where(a => a.Id == id).FirstOrDefaultAsync();
        if (data == null)
        {
            return NotFound();
        }
        return Ok(data);
    }
    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangPassword(string id, [FromBody] PasswordChange passwordChange)
    {
        var user = await _context.UserApp.Where(a => a.Id == id).FirstOrDefaultAsync();
        if (user == null)
        {
            return NotFound();
        }
        await _userManager.RemovePasswordAsync(user);
        await _userManager.AddPasswordAsync(user, passwordChange.PasswordNew);
        return Ok("Update password succsecc");
    }
}