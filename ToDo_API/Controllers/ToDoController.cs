using Microsoft.AspNetCore.Mvc;
using ToDo_API.Entities;
using ToDo_API.Models;
using ToDo_API.Services;

namespace ToDo_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;
        public ToDoController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var toDos = _toDoService.GetAll();
            return Ok(toDos);
        }
        [HttpGet("{startDate}-{endDate}")]
        public IActionResult GetSpecificTimeFrame(DateTime startDate, DateTime endDate)
        {
            var toDos = _toDoService.GetIncomingToDo(startDate, endDate);
            return Ok(toDos);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var toDo = _toDoService.GetById(id);
            return Ok(toDo);
        }
        [HttpPost]
        public IActionResult Create([FromBody]ToDoDTO toDoDTO)
        {
            var result = _toDoService.Create(toDoDTO);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute]int id, [FromBody]ToDoDTO toDoDTO)
        {
            var result = _toDoService.Update(id, toDoDTO);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPatch("{id}/percentage/{percentage}")]
        public IActionResult SetPercentageDone([FromRoute]int id, [FromRoute]byte percentage)
        {
            var result = _toDoService.SetPercentageDone(id, percentage);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPatch("{id}/mark-completed")]
        public IActionResult MarkAsCompleted([FromRoute]int id)
        {
            var result = _toDoService.MarkAsCompleted(id);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            var result = _toDoService.Delete(id);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

    }
}
