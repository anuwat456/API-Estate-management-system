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

namespace API_Estate_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseTypeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HouseTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        // GET: api/HouseType/GetAllHouseType
        public IActionResult GetAllHouseType()
        {
            return Ok(_context.HouseTypes.ToList());
        }

        [HttpPost("[action]")]
        // POST: api/HouseType/CreateHouseType
        public async Task<IActionResult> CreateHouseType([FromBody] CreateHouseTypeModel model)
        {
            if (model == null)
            {
                return NotFound();
            }
            else if (ModelState.IsValid)
            {
                var createHouseType = new ApplicationHouseType
                {
                    Type = model.Type
                };

                _context.HouseTypes.Add(createHouseType);
                await _context.SaveChangesAsync();

                // Finally return the result to client
                return Ok(new JsonResult("The News was add Successfully"));
            }
            return NotFound();
        }

        [HttpPut("[action]/{id}")]
        // PUT: api/HouseType/UpdateHouseType/id
        public async Task<IActionResult> UpdateHouseType([FromRoute] string id, [FromBody] CreateHouseTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var findHouseType = _context.HouseTypes.FirstOrDefault(ht => ht.Id == id);
            if (findHouseType == null)
            {
                return NotFound();
            }

            // The HouseType was found
            findHouseType.Type = model.Type;

            _context.Entry(findHouseType).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HouseTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Finally return the result to client
            return Ok(new JsonResult("The HouseType with id " + id + " is Update."));
        }

        [HttpDelete("[action]/{id}")]
        // DELETE: api/HouseType/DeleteHouseType/id
        public async Task<IActionResult> DeleteHouseType([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // The HouseType was found
            var findHouseType = await _context.HouseTypes.FindAsync(id);
            if (findHouseType == null)
            {
                return NotFound();
            }

            _context.HouseTypes.Remove(findHouseType);
            await _context.SaveChangesAsync();

            // Finally return the result to client
            return Ok(new JsonResult("The HouseType with id : " + id + " is Delete."));
        }

        private bool HouseTypeExists(string id)
        {
            return _context.HouseTypes.Count(ht => ht.Id == id) > 0;
        }
    }
}
