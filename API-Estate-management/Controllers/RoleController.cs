using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Estate_management.Models.Model;
using API_Estate_management.Models.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Estate_management.Controllers
{
    [Authorize(Policy = "RequireAdministratorRole")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet("[action]")]
        // GET: api/Role/GetAllRoles
        public IActionResult GetAllRoles()
        {
            return Ok(_context.Roles.ToList());
        }

        [HttpPost("[action]")]
        // POST: api/Role/CreateRole
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleModel model)
        {
            // Will hold all the errors related to registation
            List<string> errorList = new List<string>();

            if (model == null)
            {
                return NotFound();
            }
            else if (ModelState.IsValid)
            {
                if (RoleExistes(model.Name))
                {
                    return BadRequest("Role already exists in the database.");
                }

                var newRole = new ApplicationRole
                {
                    Name = model.Name
                };

                var result = await _roleManager.CreateAsync(newRole);
                if (result.Succeeded)
                {
                    return Ok(new JsonResult("The User was add Successfully"));
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return BadRequest(new JsonResult(errorList));
        }

        [HttpPut("[action]/{id}")]
        // PUT: api/Role/UpdateRole/id
        public async Task<IActionResult> UpdateRole([FromRoute] string id, [FromBody] UpdateRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (RoleExistes(model.Name))
            {
                return BadRequest("Role already exists in the database.");
            }

            var findRole = _context.Roles.FirstOrDefault(r => r.Id == id);
            if (findRole == null)
            {
                return NotFound();
            }

            // The Role was found
            findRole.Name = model.Name;
            findRole.NormalizedName = model.Name.ToUpper();

            _context.Entry(findRole).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Finally return the result to client
            return Ok(new JsonResult("The Role with id " + id + " is Update."));
        }
        

        [HttpDelete("[action]/{id}")]
        // DELETE: api/Role/DeleteRole/id
        public async Task<IActionResult> DeleteRole([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // The Role was found
            var findRole = await _context.Roles.FindAsync(id);
            if (findRole == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(findRole);
            await _context.SaveChangesAsync();

            // Finally return the result to client
            return Ok(new JsonResult("The Role with id " + id + " is Delete."));
        }


        // Checking existes Role
        private bool RoleExistes(string name)
        {
            return _context.Roles.Any(x => x.Name == name);
        }

        private bool RolesExists(string id)
        {
            return _context.Roles.Any(r => r.Id == id);
        }
    }
}
