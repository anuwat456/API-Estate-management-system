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
    public class HouseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HouseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        // GET: api/House/GetAllHouse
        public IActionResult GetAllHouse()
        {
            return Ok(_context.Houses.ToList());
        }

        [HttpPost("[action]")]
        // POST: api/House/CreateHouse
        public async Task<IActionResult> CreateHouse([FromBody] CreateHouseModel model)
        {
            if (model == null)
            {
                return NotFound();
            }
            else if (ModelState.IsValid)
            {
                var createHouse = new ApplicationHouse
                {
                    Id = model.Id,
                    LaneHouse = model.LaneHouse,
                    ColorHouse = model.ColorHouse,
                    AreaHouse = model.AreaHouse
                };

                _context.Houses.Add(createHouse);
                await _context.SaveChangesAsync();

                // Finally return the result to client
                return Ok(new JsonResult("The House was add Successfully"));
            }
            return NotFound();
        }

        [HttpPut("[action]/{id}")]
        // PUT: api/House/UpdateHouse/id
        public async Task<IActionResult> UpdateHouse([FromRoute] string id, [FromBody] CreateHouseModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var findHouse = _context.Houses.FirstOrDefault(h => h.Id == id);
            if (findHouse == null)
            {
                return NotFound();
            }

            // The House was found
            findHouse.Id = model.Id;
            findHouse.LaneHouse = model.LaneHouse;
            findHouse.ColorHouse = model.ColorHouse;
            findHouse.AreaHouse = model.AreaHouse;

            _context.Entry(findHouse).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HouseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Finally return the result to client
            return Ok(new JsonResult("The House with id " + id + " is Update."));
        }

        [HttpDelete("[action]/{id}")]
        // DELETE: api/House/DeleteHouse/id
        public async Task<IActionResult> DeleteHouse([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // The House was found
            var findHouse = await _context.Houses.FindAsync(id);
            if (findHouse == null)
            {
                return NotFound();
            }

            _context.Houses.Remove(findHouse);
            await _context.SaveChangesAsync();

            // Finally return the result to client
            return Ok(new JsonResult("The House with id : " + id + " is Delete."));
        }

        private bool HouseExists(string id)
        {
            return _context.Houses.Count(h => h.Id == id) > 0;
        }
    }
}
