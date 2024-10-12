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

namespace api.Controllers
{
    [Route("api/Farms/{farmId}/[controller]")]
    [ApiController]
    public class FieldsController : ControllerBase
    {
        private readonly IFieldRepository _fieldRepo;
        private readonly IFarmRepository _farmRepo;

        public FieldsController(IFieldRepository fieldRepo, IFarmRepository farmRepo)
        {
            _fieldRepo = fieldRepo;
            _farmRepo = farmRepo;
        }

        // GET: api/Farm/2/Fields
        [HttpGet]
        public async Task<ActionResult> GetFields([FromRoute] int farmId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _farmRepo.FarmExists(farmId))
            {
                return BadRequest("Farm does not exist");
            }

            var fields = await _fieldRepo.GetAllAsync(farmId);

            var fieldDto = fields.Select(s => s.ToFieldDto());

            return Ok(fieldDto);
        }

        // GET: api/Fields/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Field>> GetField([FromRoute] int id, [FromRoute] int farmId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _farmRepo.FarmExists(farmId))
            {
                return BadRequest("Farm does not exist");
            }

            var field = await _fieldRepo.GetByIdAsync(id, farmId);

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
        public async Task<IActionResult> PutField([FromRoute] int id, [FromBody] UpdateFieldRequestDto fieldDto, [FromRoute] int farmId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _farmRepo.FarmExists(farmId))
            {
                return BadRequest("Farm does not exist");
            }

            var field = await _fieldRepo.UpdateAsync(id, fieldDto.ToFieldFromUpdate(), farmId);
            
            if (field == null)
            {
                return NotFound("Field not found");
            }

            return Ok(field.ToFieldDto());
        }

        // POST: api/Fields
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostField([FromRoute] int farmId, [FromBody] CreateFieldDto fieldDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _farmRepo.FarmExists(farmId))
            {
                return BadRequest("Farm does not exist");
            }

            var field = fieldDto.ToFieldFromCreate(farmId);

            await _fieldRepo.CreateAsync(field);

            return CreatedAtAction("GetField", new { id = field.Id, farmId = field.FarmId }, field.ToFieldDto());
        }

        // DELETE: api/Fields/5
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteField([FromRoute] int id, [FromRoute] int farmId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _farmRepo.FarmExists(farmId))
            {
                return BadRequest("Farm does not exist");
            }

            var field = await _fieldRepo.DeleteAsync(id, farmId);

            if (field == null)
            {
                return NotFound("Field does not exist");
            }

            return Ok(field);
        }
    }
}
