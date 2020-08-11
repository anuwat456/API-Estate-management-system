using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Estate_management.Models.Model;
using API_Estate_management.Models.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace API_Estate_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PermissionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        [Authorize(Policy = "RequireAdministratorRole")]
        // GET: api/Permission/GetAllPermissions
        public IActionResult GetAllPermissions()
        {
            return Ok(_context.Permissions.ToList());
        }

        [HttpPost("[action]")]
        [Authorize(Policy = "RequireAdministratorRole")]
        // POST: api/Permission/CreatePermission
        public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionModel model)
        {
            if (model == null)
            {
                return NotFound();
            }
            else if (ModelState.IsValid)
            {
                if (PermissionExistes(model.Name))
                {
                    return BadRequest("Permission Name already exists in the database.");
                }

                var newPermission = new ApplicationPermission
                {
                    Name = model.Name,
                    Level = model.Level,
                    ParentId = model.ParentId,
                    Title = model.Title
                };

                _context.Permissions.Add(newPermission);
                await _context.SaveChangesAsync();

                // Finally return the result to client
                return Ok(new JsonResult("The Permission was add Successfully"));
            }
            return NotFound();
        }

        [HttpPut("[action]/{id}")]
        [Authorize(Policy = "RequireAdministratorRole")]
        // PUT: api/Permission/UpdatePermission/id
        public async Task<IActionResult> UpdatePermission([FromRoute] string id, [FromBody] CreatePermissionModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var findPermission = _context.Permissions.FirstOrDefault(p => p.Id == id);
            if (findPermission == null)
            {
                return NotFound();
            }

            // The Permission was found
            findPermission.Name = model.Name;
            findPermission.Level = model.Level;
            findPermission.ParentId = model.ParentId;
            findPermission.Title = model.Title;

            _context.Entry(findPermission).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermissionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Finally return the result to client
            return Ok(new JsonResult("The User with id " + id + " is Update."));
        }

        [HttpDelete("[action]/{id}")]
        [Authorize(Policy = "RequireAdministratorRole")]
        // DELETE: api/Permission/DeletePermission/id
        public async Task<IActionResult> DeletePermission([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // The Permission was found
            var findPermission = await _context.Permissions.FindAsync(id);
            if (findPermission == null)
            {
                return NotFound();
            }

            _context.Permissions.Remove(findPermission);
            await _context.SaveChangesAsync();

            // Finally return the result to client
            return Ok(new JsonResult("The User with id : " + id + " is Delete."));
        }


        private bool PermissionExistes(string name)
        {
            return _context.Permissions.Any(x => x.Name == name);
        }

        private bool PermissionExists(string id)
        {
            return _context.Permissions.Count(p => p.Id == id) > 0;
        }
    }
}
