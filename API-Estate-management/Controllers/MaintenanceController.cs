using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Estate_management.Models.Model;
using API_Estate_management.Models.ModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Estate_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MaintenanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        // GET: api/Maintenance/GetAllMaintenance
        public IActionResult GetAllMaintenance()
        {
            return Ok(_context.Maintenances.ToList());
        }

        [HttpPost("[action]")]
        // POST: api/Maintenance/CreateMaintenance
        public async Task<IActionResult> CreateMaintenance([FromBody] CreateMaintenanceModel model)
        {
            if (model == null)
            {
                return NotFound();
            }
            else if (ModelState.IsValid)
            {
                var createMaintenance = new ApplicationMaintenance
                {
                    Location = model.Location,
                    ImageMainTen = model.ImageMainTen,
                    StatusMainten = model.StatusMainten
                };

                _context.Maintenances.Add(createMaintenance);
                await _context.SaveChangesAsync();

                // Finally return the result to client
                return Ok(new JsonResult("The Maintenance was add Successfully"));
            }
            return NotFound();
        }

        [HttpPut("[action]/{id}")]
        // PUT: api/Maintenance/UpdateMaintenance/id
        public async Task<IActionResult> UpdateMaintenance([FromRoute] string id,[FromBody] CreateMaintenanceModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var findMaintenance = _context.Maintenances.FirstOrDefault(m => m.Id == id);
            if (findMaintenance == null)
            {
                return NotFound();
            }

            // The Maintenance was found
            findMaintenance.Detail = model.Detail;
            findMaintenance.Location = model.Location;
            findMaintenance.ImageMainTen = model.ImageMainTen;
            findMaintenance.StatusMainten = model.StatusMainten;

            _context.Entry(findMaintenance).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaintenanceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Finally return the result to client
            return Ok(new JsonResult("The Maintenance with id " + id + " is Update."));
        }

        [HttpDelete("[action]/{id}")]
        // DELETE: api/Maintenance/DeleteMaintenance/id
        public async Task<IActionResult> DeleteMaintenance([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // The News was found
            var findMaintenance = await _context.Maintenances.FindAsync(id);
            if (findMaintenance == null)
            {
                return NotFound();
            }

            _context.Maintenances.Remove(findMaintenance);
            await _context.SaveChangesAsync();

            // Finally return the result to client
            return Ok(new JsonResult("The Maintenance with id : " + id + " is Delete."));
        }

        private bool MaintenanceExists(string id)
        {
            return _context.Maintenances.Count(m => m.Id == id) > 0;
        }
    }
}
