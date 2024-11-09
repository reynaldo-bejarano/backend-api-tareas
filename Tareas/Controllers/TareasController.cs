using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tareas.Application.Services;
using Tareas.Domain.Entitites;

namespace Tareas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TareasController : ControllerBase
    {

        private readonly TareaService _tareaService;
        public TareasController(TareaService tareaService)
        {
            _tareaService = tareaService;
        }

        [HttpGet("list")]
        [Authorize]
        public async Task<ActionResult> GetAllTask() {
            try
            {
                var tareas = await _tareaService.GetAllTaskAsync();

                return Ok(tareas);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get")]
        [Authorize]
        public async Task<ActionResult> GetTaskById(string id)
        {
            try
            {
                var tareas = await _tareaService.GetTaskAsync(id) ?? throw new Exception("Id not found");
              
                return Ok(tareas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<ActionResult> PostTask(Tarea tarea)
        {
            try
            {
                if (tarea == null) throw new Exception("Tarea is null");

                var response = await _tareaService.CreateTaskAsync(tarea);

                if (response != "200") throw new Exception(response);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update")]
        [Authorize]
        public async Task<ActionResult> PostUpdateTaskById(string id, Tarea tarea)
        {
            try
            {
                if (tarea == null || id == null) throw new Exception("Tarea is null");

                var response = await _tareaService.UpdateTaskAsync(id, tarea);

                if (response != "200") throw new Exception(response);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("delete")]
        [Authorize]
        public async Task<ActionResult> PostDeleteTask(string id)
        {
            try
            {
                if (id == null) throw new Exception("Id cannot be null");

                var response = await _tareaService.DeleteTaskAsync(id);

                if (response != "200") throw new Exception(response);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
