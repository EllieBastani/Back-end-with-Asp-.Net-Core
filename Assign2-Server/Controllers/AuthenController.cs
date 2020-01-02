using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Assign2_Server.DataModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace Assign2_Server.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    [EnableCors("SantaPolicy")]
    public class AuthenController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }


        [Route("register")]
        [HttpPost]
        public async Task<ActionResult> InsertUser([FromBody] RegistrationInfo model)
        {
            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsNaughty = false,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                BirthDate = model.BirthDate,
                Street = model.Street,
                City = model.City,
                Country = model.Country,
                Province = model.Province,
                PostalCode = model.PostalCode,
                DateCreated = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Child");
                return Ok(new { Username = user.UserName });
            }
            else
            {
                return BadRequest(result.ToString());
            }
        }

        [Route("login")]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LogInCredential model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {/*
                var claim = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
                  };*/
                   var roles = await _userManager.GetRolesAsync(user);
                   var claims = new List<Claim>();
                    string firstRole = "";  

                   claims.Add(new Claim(ClaimTypes.Name, user.UserName));

                   foreach (var role in roles)
                   {
                    if (firstRole == "")
                        firstRole = role;
                       claims.Add(new Claim(ClaimTypes.Role, role));
                   }

                var signinKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

                int expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);

                var token = new JwtSecurityToken(
                  issuer: _configuration["Jwt:Site"],
                  audience: _configuration["Jwt:Site"],
                  expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                  signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256),
                  claims:claims
                );

                return Ok(
                  new
                  {
                      token = new JwtSecurityTokenHandler().WriteToken(token),
                      expiration = token.ValidTo,
                      id = user.Id,
                      firstName = user.FirstName,
                      role = firstRole,
                  });
            }
            return Unauthorized();
        }

        [Route("changePassword")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordInfo model)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.ToString());
            }
        }


    }
}