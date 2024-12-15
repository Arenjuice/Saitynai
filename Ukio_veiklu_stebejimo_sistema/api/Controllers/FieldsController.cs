using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;
using api.Interfaces;
using api.Mappers;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using api.Dtos.Field;
using api.Dtos.Farm;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using api.Auth.Model;

namespace api.Controllers
{
    [Route("api/Farms/{farmId}/[controller]")]
    [ApiController]
    public class FieldsController : ControllerBase
    {
        private readonly IFieldRepository _fieldRepo;
        private readonly IFarmRepository _farmRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FieldsController(IFieldRepository fieldRepo, IFarmRepository farmRepo, IHttpContextAccessor httpContextAccessor)
        {
            _fieldRepo = fieldRepo;
            _farmRepo = farmRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/Farm/2/Fields
        [HttpGet]
        [Authorize(Roles = $"{SystemRoles.Farmer},{SystemRoles.Worker}")]
        public async Task<ActionResult> GetFields([FromRoute] int farmId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _farmRepo.FarmExists(farmId))
            {
                return BadRequest("Farm does not exist");
            }

            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userId == null)
                return Unauthorized();

            var fields = await _fieldRepo.GetAllAsync(farmId, userId);
            if (fields == null)
                return NotFound();

            var fieldDto = fields.Select(s => s.ToFieldDto());

            return Ok(fieldDto);
        }

        // GET: api/Fields/5
        [HttpGet("{id:int}")]
        [Authorize(Roles = $"{SystemRoles.Farmer},{SystemRoles.Worker}")]
        public async Task<ActionResult<Field>> GetField([FromRoute] int id, [FromRoute] int farmId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _farmRepo.FarmExists(farmId))
            {
                return BadRequest("Farm does not exist");
            }

            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userId == null)
                return Unauthorized();

            var field = await _fieldRepo.GetByIdAsync(id, farmId, userId);

            if (field == null)
            {
                return NotFound();
            }

            return Ok(field.ToFieldDto());
        }

        // PUT: api/Fields/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = SystemRoles.Farmer)]
        public async Task<IActionResult> PutField([FromRoute] int id, [FromBody] UpdateFieldRequestDto fieldDto, [FromRoute] int farmId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _farmRepo.FarmExists(farmId))
            {
                return BadRequest("Farm does not exist");
            }

            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userId == null)
                return Unauthorized();

            var field = await _fieldRepo.UpdateAsync(id, fieldDto.ToFieldFromUpdate(userId), farmId, userId);
            
            if (field == null)
            {
                return NotFound("Field not found");
            }

            return Ok(field.ToFieldDto());
        }

        // POST: api/Fields
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = SystemRoles.Farmer)]
        public async Task<IActionResult> PostField([FromRoute] int farmId, [FromBody] CreateFieldDto fieldDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _farmRepo.FarmExists(farmId))
            {
                return BadRequest("Farm does not exist");
            }

            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userId == null)
                return Unauthorized();

            var field = fieldDto.ToFieldFromCreate(farmId, userId);


            if (await _fieldRepo.CreateAsync(field, userId) == null)
                return NotFound();

            return CreatedAtAction("GetField", new { id = field.Id, farmId = field.FarmId }, field.ToFieldDto());
        }

        // DELETE: api/Fields/5
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = SystemRoles.Farmer)]
        public async Task<IActionResult> DeleteField([FromRoute] int id, [FromRoute] int farmId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _farmRepo.FarmExists(farmId))
            {
                return BadRequest("Farm does not exist");
            }

            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userId == null)
                return Unauthorized();

            var field = await _fieldRepo.DeleteAsync(id, farmId, userId);

            if (field == null)
            {
                return NotFound("Field does not exist or it contains records and can not be deleted");
            }

            return Ok();
        }
    }
}
