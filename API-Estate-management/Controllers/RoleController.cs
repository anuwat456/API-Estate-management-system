using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Estate_management.Models.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Estate_management.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleController(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        [HttpGet("[action]")]
        [Authorize(Policy = "RequireAdministratorRole")]
        // GET: api/Role/GetRoles
        public IActionResult GetRoles()
        {
            return Ok(_context.Roles.ToList());
        }

        [HttpPost("[action]")]
        [Authorize(Policy = "RequireAdministratorRole")]
        // POST: api/Role/AddRole
        public async Task<IActionResult> AddRole([FromBody] ApplicationRole role)
        {
            var newrole = new ApplicationRole
            {
                Name = role.Name
            };

            await _context.Roles.AddAsync(newrole);
            await _context.SaveChangesAsync();

            return Ok(new JsonResult("The Role was add Successfully"));
        }

        [HttpPut("action/{id}")]
        [Authorize(Policy = "RequireAdministratorRole")]
        // PUT: api/Role/UpdateRole/id
        public async Task<IActionResult> UpdateRole([FromRoute] string id, [FromBody] ApplicationRole role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var findRole = _context.Roles.FirstOrDefault(r => r.Id == id);
            if (findRole == null)
            {
                return NotFound();
            }

            // The Role was found
            findRole.Name = role.Name;


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

        // Check Roles Exisists
        private bool RolesExists(string id)
        {
            return _context.Roles.Any(r => r.Id == id);
        }

        [HttpDelete("[action]/{id}")]
        [Authorize(Policy = "RequireAdministratorRole")]
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
    }
}
