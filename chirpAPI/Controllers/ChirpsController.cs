using chirpApi.Services.Model.DTOs;
using chirpApi.Services.Model.Filters;
using chirpApi.Services.Model.ViewModel;
using chirpApi.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chirpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChirpsController : ControllerBase
    {
        private readonly IChirpsService _chirpsService;
        private readonly ILogger<ChirpsController> _logger;

        public ChirpsController(IChirpsService chirpsService, ILogger<ChirpsController> logger)
        {

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _chirpsService = chirpsService ?? throw new ArgumentNullException(nameof(chirpsService));
        }

        // GET: api/Chirps

        [HttpGet]
        public async Task<IActionResult> GetChirpsByFilter([FromQuery] ChirpFilter filter)
        {
            _logger.LogInformation("ChirpsController.GetChirpsByFilter called with filter: {@Filter}", filter);

            List<ChirpViewModel> result = await _chirpsService.GetChirpsByFilter(filter);



            if (result == null || !result.Any())
            {
                _logger.LogInformation("No chirps found for the given filter: {@Filter}", filter);
                return NoContent();
            }
            else
            {
                _logger.LogInformation("Chirps found: {@Chirps}", result);
                return Ok(result);
            }
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAllChirps()
        {
            _logger.LogInformation("ChirpsController.GetAllChirps called");

            var chirps = await _chirpsService.GetAllChirps();

            if (chirps == null || !chirps.Any())
            {
                _logger.LogInformation("No chirps found in database");
                return NoContent();
            }

            _logger.LogInformation("Returning {Count} chirps", chirps.Count);
            return Ok(chirps);
        }

        // GET: api/Chirps/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChirpById([FromRoute] int id)

        {
            _logger.LogInformation("ChirpsController.GetChirpById called with id: {Id}", id);

            var chirp = await _chirpsService.GetChirpById(id);

            if (chirp == null)
            {
                _logger.LogInformation("Chirp with id {Id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Returning chirp with id {Id}", id);
            return Ok(chirp);
        }

        // PUT: api/Chirps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChirp([FromRoute] int id, [FromBody] ChirpUpdateDTO chirp)
        {
            var result = await _chirpsService.UpdateChirp(id, chirp);
            if (result == null)
            {

                return BadRequest("Invalid chirp data or chirp not found");
            }

            return NoContent();
        }

        // POST: api/Chirps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostChirp([FromBody] ChirpCreateDTO chirp)
        {
            var chirpId = await _chirpsService.CreateChirp(chirp);
            if (chirpId == null)
            {

                return BadRequest("Invalid chirp data");
            }
            return Created($"/api/Chirp/{chirp}", chirpId);
        }

        // DELETE: api/Chirps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChirp([FromRoute] int id)
        {
            int? result = await _chirpsService.DeleteChirp(id);

            if (result == null)
            {

                return BadRequest("Chirp non esiste!");
            }
            if (result == -1)
            {
                return BadRequest("Attenzione eliminare prima tutti i commenti associati alla Chirp!");
            }
            return Ok(result);
        }
    } 
}

    

