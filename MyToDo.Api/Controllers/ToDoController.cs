using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Context;
using Microsoft.EntityFrameworkCore;
using MyToDo.Api.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;


namespace MyToDo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ToDoController : Controller
    {
        private readonly IToDoService toDoService;
        public ToDoController(IToDoService toDoService)
        {

            this.toDoService = toDoService;
        }
        [HttpGet]
        public async Task<ApiResponse> Get(int id)=> await toDoService.GetSingleAsync(id);


        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] QueryParameter queryParameter)=> await toDoService.GetAllAsync(queryParameter);

        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] ToDoDto toDo)=> await toDoService.AddAsync(toDo);

        [HttpPut]
        public async Task<ApiResponse> Update([FromBody] ToDoDto toDo) => await toDoService.UpdateAsync(toDo);

        [HttpDelete]
        public async Task<ApiResponse> Delete(int id) => await toDoService.DeleteAsync(id);


        //[HttpPost]
        //public async Task<IActionResult> Post(int id)
        //{
        //    var repository = unitOfWork.GetRepository<ToDo>();
        //    var todo = new ToDo { Content = "21123", CreatDate=DateTime.UtcNow, Status=1,  Title="213", UpdateDate=DateTime.UtcNow };
        //    var result = repository.InsertAsync(todo);
        //    unitOfWork.SaveChanges();
        //    return Ok(todo);
        //}


    }
}
