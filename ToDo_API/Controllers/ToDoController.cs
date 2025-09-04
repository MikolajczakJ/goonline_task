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
        /// <summary>
        /// Retrieves a list of all tasks.
        /// </summary>
        /// <remarks>
        /// Fetches and returns the details of all tasks in the database. It returns an "OK" status 
        /// with a collection of data.
        /// </remarks>
        /// <returns>Returns Ok with a collection of ReadToDoDTO objects.</returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var toDos = _toDoService.GetAll();
            return Ok(toDos);
        }

        /// <summary>
        /// Retrieves a list of ToDo tasks within the specified time frame.
        /// </summary>
        /// <param name="startDate">The start date of the time frame. Only to-do items occurring on or after this date will be included.</param>
        /// <param name="endDate">The end date of the time frame. Only to-do items occurring on or before this date will be included.</param>
        /// <returns>An Ok containing the list of ToDo tasks within the specified time frame.</returns>
        [HttpGet("{startDate}-{endDate}")]
        public IActionResult GetSpecificTimeFrame(DateTime startDate, DateTime endDate)
        {
            var toDos = _toDoService.GetIncomingToDo(startDate, endDate);
            return Ok(toDos);
        }
        /// <summary>
        /// Retrieves the details of a ToDo task by its ID.
        /// </summary>
        /// <param name="id">The ID of the ToDo task to retrieve.</param>
        /// <remarks>
        /// Fetches and returns the details of a specific ToDo task. It returns an "OK" status 
        /// with the ToDo task data if found.
        /// </remarks>
        /// <response code= "200">If the item is found</response>
        /// <response code="404">If the item is null</response>
        /// <returns>Returns Ok with the details of the ToDo as a ReadToDoDTO.</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var toDo = _toDoService.GetById(id);
            return Ok(toDo);
        }
        [HttpPost]
        /// <summary>
        /// Creates a new ToDo task.
        /// </summary>
        /// <param name="toDoDTO"></param>
        /// <response code="200">If the item is created</response>
        /// <response code="400">If the item is doesn't meet the requirements</response>
        /// <returns>Ok status if ToDo entity was created successfully, otherwise returns Bad Request </returns>
        public IActionResult Create([FromBody]ToDoDTO toDoDTO)
        {
            var result = _toDoService.Create(toDoDTO);
            return Ok();
        }
        

        /// <summary>
        /// Updates an existing ToDo task.
        ///</summary>
        ///<param name="id">Id of the toDo task, that will be updated </param>
        ///<param name="toDoDTO">New values for the toDo task</param>
        ///<response code="200">If the item is successfully updated</response>
        ///<response code ="400">If the item is doesn't meet the requirements</response>
        ///<response code ="404">If there is no item with given id</response>
        ///<returns>
        ///Ok status if ToDo entity was successfully updated, Bad Request if updated 
        ///ToDo doesn't meet requirements and Not Found if tere is no ToDo with given Id
        ///</returns>
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute]int id, [FromBody]ToDoDTO toDoDTO)
        {
            var result = _toDoService.Update(id, toDoDTO);
            return Ok();
        }

        /// <summary>
        /// Changes the percentage done of a ToDo item.
        /// </summary>
        /// <param name="id">ID of the ToDo task that will have its percentage done changed</param>
        /// <param name="percentage"> New value of the percentage</param>
        /// <response code="200">If the percentag is sucesfully set</response>
        /// <response code="404">If the item could not be found</response>
        /// <returns></returns>
        [HttpPatch("{id}/percentage/{percentage}")]
        public IActionResult SetPercentageDone([FromRoute]int id, [FromRoute]byte percentage)
        {
            var result = _toDoService.SetPercentageDone(id, percentage);
            return Ok();
        }
        /// <summary>
        /// Marks a ToDo item as completed.
        /// </summary>
        /// <param name="id">ID of the ToDo task that will be marked as done.</param>
        /// <returns></returns>
        [HttpPatch("{id}/mark-completed")]
        public IActionResult MarkAsCompleted([FromRoute]int id)
        {
            var result = _toDoService.MarkAsCompleted(id);
            return Ok();
        }
        /// <summary>
        /// Deletes a ToDo item with the specified ID.
        /// </summary>
        /// <param name="id">ID of the ToDo task to delete.</param>
        /// <response code="200">If the item is successfully deleted</response>
        /// <response code="404">If the item could not be found</response>
        /// <returns>Returns Ok if the item was successfully deleted; otherwise, returns Not Found"/>.</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            var result = _toDoService.Delete(id);
            return Ok();
        }

    }
}
