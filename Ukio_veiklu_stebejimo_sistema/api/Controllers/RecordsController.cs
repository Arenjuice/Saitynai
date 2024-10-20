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
using api.Dtos.Record;
using api.Mappers;
using api.Repository;

namespace api.Controllers
{
    [Route("api/Farms/{farmId}/Fields/{fieldId}/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly IRecordRepository _recordRepo;
        private readonly IFieldRepository _fieldRepo;
        private readonly IFarmRepository _farmRepo;

        public RecordsController(IRecordRepository recordRepo, IFieldRepository fieldRepo, IFarmRepository farmRepo)
        {
            _recordRepo = recordRepo;
            _fieldRepo = fieldRepo;
            _farmRepo = farmRepo;
        }

        // GET: api/Farm/2/Fields
        [HttpGet]
        public async Task<ActionResult> GetRecords([FromRoute] int farmId, [FromRoute] int fieldId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _farmRepo.FarmExists(farmId))
                return BadRequest("Farm does not exist");

            if (!await _fieldRepo.FieldExists(fieldId))
                return BadRequest("Field does not exist");

            var records = await _recordRepo.GetAllAsync(fieldId, farmId);

            if (records == null)
                return NotFound();

            var recordDto = records.Select(s => s.ToRecordDto());

            return Ok(recordDto);
        }

        // GET: api/Fields/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetRecord([FromRoute] int id, [FromRoute] int farmId, [FromRoute] int fieldId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _farmRepo.FarmExists(farmId))
                return BadRequest("Farm does not exist");
            
            if (!await _fieldRepo.FieldExists(fieldId))
                return BadRequest("Field does not exist");

            var record = await _recordRepo.GetByIdAsync(id, fieldId, farmId);

            if (record == null)
                return NotFound();

            return Ok(record.ToRecordDto());
        }

        // PUT: api/Fields/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> PutRecord([FromRoute] int id, [FromBody] UpdateRecordRequestDto recordDto, [FromRoute] int farmId, [FromRoute] int fieldId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _farmRepo.FarmExists(farmId))
                return BadRequest("Farm does not exist");

            if (!await _fieldRepo.FieldExists(fieldId))
                return BadRequest("Field does not exist");
            
            var record = await _recordRepo.UpdateAsync(id, recordDto.ToRecordFromUpdate(), fieldId, farmId);
            
            if (record == null)
                return NotFound("Record not found");

            return Ok(record.ToRecordDto());
        }

        // POST: api/Fields
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostRecord([FromRoute] int fieldId, [FromBody] CreateRecordDto recordDto, [FromRoute] int farmId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _farmRepo.FarmExists(farmId))
                return BadRequest("Farm does not exist");

            if (!await _fieldRepo.FieldExists(fieldId))
                return BadRequest("Field does not exist");

            if (await _fieldRepo.GetByIdAsync(fieldId, farmId) == null)
                return BadRequest("Field does not exist");

            var record = recordDto.ToRecordFromCreate(fieldId);

            await _recordRepo.CreateAsync(record, fieldId, farmId);

            return CreatedAtAction("GetRecord", new { id = record.Id, farmId, fieldId }, record.ToRecordDto());
        }

        // DELETE: api/Fields/5
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteRecord([FromRoute] int id, [FromRoute] int fieldId, [FromRoute] int farmId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _farmRepo.FarmExists(farmId))
                return BadRequest("Farm does not exist");

            if (!await _fieldRepo.FieldExists(fieldId))
                return BadRequest("Field does not exist");

            var record = await _recordRepo.DeleteAsync(id, fieldId, farmId);

            if (record == null)
                return NotFound("Record does not exist");

            return Ok(record.ToRecordDto());
        }
    }
}
