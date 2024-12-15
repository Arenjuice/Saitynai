using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;
using api.Mappers;
using Microsoft.AspNetCore.Http;
using api.Dtos.Farm;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using api.Auth.Model;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmsController : ControllerBase
    {
        private readonly IFarmRepository _farmRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FarmsController(IFarmRepository farmRepo, IHttpContextAccessor httpContextAccessor)
        {
            _farmRepo = farmRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/Farms
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetFarms()
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var farms = await _farmRepo.GetAllAsync();

            var farmsDto = farms.Select(s => s.ToFarmDto());

            return Ok(farmsDto);
        }

        // GET: api/Farms/5
        [HttpGet]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetFarm([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userId == null)
                return Unauthorized();

            var farm = await _farmRepo.GetByIdAsync(id, userId);

            if (farm == null)
            {
                return NotFound();
            }

            return Ok(farm.ToFarmDto());
        }

        // PUT: api/Farms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = SystemRoles.Farmer)]
        public async Task<IActionResult> PutFarm([FromRoute] int id, [FromBody] UpdateFarmRequestDto farmDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userId == null)
                return Unauthorized();

            var farm = await _farmRepo.UpdateAsync(id, farmDto, userId);


            if (farm == null)
            {
                return BadRequest();
            }

            return Ok(farm.ToFarmDto());
        }

        // POST: api/Farms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = SystemRoles.Farmer)]
        public async Task<IActionResult> PostFarm([FromBody] CreateFarmRequestDto farmDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userId == null)
                return Unauthorized();

            var farm = farmDto.ToFarmFromCreateDto(userId);
            await _farmRepo.CreateAsync(farm);

            return CreatedAtAction("GetFarm", new { id = farm.Id }, farm.ToFarmDto());
        }

        // DELETE: api/Farms/5
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = SystemRoles.Farmer)]
        public async Task<IActionResult> DeleteFarm([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userId == null)
                return Unauthorized();

            var farm = await _farmRepo.DeleteAsync(id, userId);
            
            if (farm == null)
            {
                return NotFound("Farms does not exist or it contains fields and can not be deleted");
            }

            return NoContent();
        }
    }
}
