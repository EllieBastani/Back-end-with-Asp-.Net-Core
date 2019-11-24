using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Assign2_Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Assign2_Server.DataModels;
using Microsoft.AspNetCore.Cors;

namespace Assign2_Server.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("SantaPolicy")]
    [ApiController]
    public class ChildrenController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChildrenController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Children
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChildInfo>>> GetChildrenList()
        {
            List<ChildInfo> childList= new List<ChildInfo>();
            foreach (var item in await _userManager.Users.ToListAsync())
            {
                var roles = await _userManager.GetRolesAsync(item);

                bool isAdmin = false;
                foreach (var role in roles)
                {
                    if (role == "Admin")
                        isAdmin = true;
                        
                }

                if (isAdmin == false)
                {
                    ChildInfo child = new ChildInfo(item.Id,item.UserName, item.FirstName, item.LastName,item.Email, item.BirthDate, item.Street, item.City, item.Province, item.PostalCode, item.Country,
                        item.Latitude, item.Longitude, item.IsNaughty, item.DateCreated);

                    childList.Add(child);
                }
            }
            return childList;
        }

        // GET: api/children/5
        [Authorize]
        [EnableCors("SantaPolicy")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ChildInfo>> GetChildInfo(string id)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(i => i.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            ChildInfo child = new ChildInfo(user.Id,user.UserName,user.FirstName, user.LastName,user.Email, user.BirthDate, user.Street, user.City, user.Province, user.PostalCode, user.Country,
                user.Latitude, user.Longitude, user.IsNaughty, user.DateCreated);

            return child;
        }


        // PUT: api/children/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditChildInfo(string id, ChildInfo child)
        {
            if (id != child.Id)
            {
                return BadRequest();
            }

            if (!ChildExists(id))
            {
                return NotFound();
            }
            var user = await _userManager.Users
                .FirstOrDefaultAsync(i => i.Id == id);

            user.FirstName = child.FirstName;
            user.LastName = child.LastName;
            user.Email = child.Email;
            user.UserName = child.UserName;
            user.BirthDate = child.BirthDate;
            user.Country = child.Country;
            user.Province = child.Province;
            user.City = child.City;
            user.Street = child.Street;
            user.PostalCode = child.PostalCode;
            user.Latitude = child.Latitude;
            user.Longitude = child.Longitude;
            user.IsNaughty = child.IsNaughty;

            try
            {
                await _userManager.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok();
        }



        // DELETE: api/children/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApplicationUser>> DeleteChildInfo(string id)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(i => i.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            //var user = await _userManager.Users
            await _userManager.RemoveFromRoleAsync(user, "Child");
            await _userManager.DeleteAsync(user);
            //_context.Children.Remove(child);
            //await _context.SaveChangesAsync();

            return user;
        }
        private bool ChildExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        
    }
}