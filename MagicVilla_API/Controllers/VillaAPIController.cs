using System;
using System.Xml.Linq;
using MagicVilla_API.Data;
using MagicVilla_API.Logging;
using MagicVilla_API.Models;
using MagicVilla_API.Models.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MagicVilla_API.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogging _logger;
        private readonly AppDbContext _db;

        public VillaAPIController(ILogging logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            _logger.Log("Getting all the villas...", LogType.INFORMATION);
            var villas = await _db.Villas.ToListAsync();
            return Ok(villas);
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<VillaDTO>> GetVillaById(int id)
        {
            if (id < 1)
            {
                _logger.Log("bad villa request: requested id less than one", LogType.ERROR);
                return BadRequest();
            }

            var villa = await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);

            if (villa is not null)
                return Ok(villa);
            else
                return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaDTO villaDTO)
        {
            
            if (villaDTO == null)
                return BadRequest(villaDTO);

            if (villaDTO.Id > 0)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if (_db.Villas.FirstOrDefault(x => x.Name.ToLower() ==
                villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("SameNameError", "Villa name already exists!");
                return BadRequest(ModelState);
            }

            if (ModelState.IsValid)
            {
                Villa villa = new()
                {
                    Id = villaDTO.Id,
                    Amenity = villaDTO.Amenity,
                    Name = villaDTO.Name,
                    ImageUrl = villaDTO.ImageUrl,
                    Occupancy = villaDTO.Occupancy,
                    Rate = villaDTO.Rate,
                    SquareFt = villaDTO.SquareFt,
                    Details = villaDTO.Details

                };

                await _db.Villas.AddAsync(villa);
                await _db.SaveChangesAsync();

                return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteVilla(int id)
        {
            if (id < 1)
                return BadRequest();

            var villa = _db.Villas.FirstOrDefault(x => x.Id == id);

            if (villa is null)
                return NotFound();

            _db.Villas.Remove(villa);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody]VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
                return BadRequest();

            //var villa = _db.Villas.FirstOrDefault(x => x.Id == id);

            if (villaDTO is not null)
            {
                Villa villa = new()
                {
                    Id = villaDTO.Id,
                    Amenity = villaDTO.Amenity,
                    Name = villaDTO.Name,
                    ImageUrl = villaDTO.ImageUrl,
                    Occupancy = villaDTO.Occupancy,
                    Rate = villaDTO.Rate,
                    SquareFt = villaDTO.SquareFt,
                    Details = villaDTO.Details

                };

                _db.Villas.Update(villa);
                _db.SaveChanges();
            }

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PatchVilla(int id, JsonPatchDocument<VillaDTO> patchDto)
        {
            if (patchDto is null || id < 1)
                return BadRequest();

            var villa = _db.Villas.AsNoTracking().FirstOrDefault(x => x.Id == id);

            if (villa is null)
                return NotFound();

            if (villa is not null)
            {
                VillaDTO villaDTO = new()
                {
                    Id = villa.Id,
                    Amenity = villa.Amenity,
                    Name = villa.Name,
                    ImageUrl = villa.ImageUrl,
                    Occupancy = villa.Occupancy,
                    Rate = villa.Rate,
                    SquareFt = villa.SquareFt,
                    Details = villa.Details

                };

                patchDto.ApplyTo(villaDTO, ModelState);

                Villa model = new()
                {
                    Id = villaDTO.Id,
                    Amenity = villaDTO.Amenity,
                    Name = villaDTO.Name,
                    ImageUrl = villaDTO.ImageUrl,
                    Occupancy = villaDTO.Occupancy,
                    Rate = villaDTO.Rate,
                    SquareFt = villaDTO.SquareFt,
                    Details = villaDTO.Details

                };

                _db.Villas.Update(model);
                _db.SaveChanges();
            }

            

            if (!ModelState.IsValid)
                return BadRequest();

            return NoContent();
        }
    }
}

