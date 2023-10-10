using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestDocker.Contants;
using TestDocker.Dto;
using TestDocker.Entities;

namespace TestDocker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DemoContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public LoginController(IConfiguration config, DemoContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _config = config;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [HttpPost("LoginJWT")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {          
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            var password = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (user != null && password)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var claims = new List<Claim>
           {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
                foreach (var item in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, item));
                }
                var token = GenerateToken(claims);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return NotFound("user not found");
        }  
        [HttpPost("Register-admin")]
        public async Task<IActionResult> CreateAdmin()
        {         
            var user = await _userManager.FindByEmailAsync("admin@gmail.com");
            if(user != null)
            {
                return BadRequest("Admin exist");
            }
            var data = new UserApp()
            {
                UserName = "admin",
                SecurityStamp = Guid.NewGuid().ToString(),
                Email = "admin@gmail.com",
            };
            string password = "Admin@@070300";
            var result = await _userManager.CreateAsync(data, password);
            if (!await _roleManager.RoleExistsAsync(UserRole.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRole.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRole.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRole.User));
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(data, UserRole.Admin);         
            }
            return Ok("Create admin success");
        }
        private JwtSecurityToken GenerateToken(List<Claim> authClaims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims: authClaims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return token;
        }
    }
}
