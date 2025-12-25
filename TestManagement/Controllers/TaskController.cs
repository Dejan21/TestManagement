using Microsoft.AspNetCore.Mvc;
using TaskManagement.Repositories;
using TestManagement.Dtos;
using TestManagement.Repositories;

namespace TestManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _repo;

        public TaskController(ITaskRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll([FromQuery] string status = "all")
        {
            var res = await _repo.GetAll(status);
            return Ok(res);
        }

        [HttpGet("{id:int}/getById")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _repo.GetById(id);
            return task is null ? NotFound() : Ok(task);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] TaskDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            if (dto.DueDate.HasValue && dto.DueDate.Value.ToUniversalTime() < DateTime.UtcNow.AddMinutes(-1))
                return BadRequest("DueDate cannot be in the past.");

            var id = await _repo.Create(dto.Title.Trim(),
                                       string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
                                       dto.Priority,
                                       dto.DueDate);

            var created = await _repo.GetById(id);
            return CreatedAtAction(nameof(GetById), new { id }, created);
        }

        [HttpPut("{id:int}/update")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var ok = await _repo.Update(id,
                dto.Title.Trim(),
                string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
                dto.Priority,
                dto.DueDate,
                dto.IsCompleted);

            return ok ? NoContent() : NotFound();
        }

        [HttpPatch("{id:int}/setComplete")]
        public async Task<IActionResult> SetComplete(int id, [FromQuery] bool value)
        {
            var ok = await _repo.SetComplete(id, value);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repo.Delete(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
