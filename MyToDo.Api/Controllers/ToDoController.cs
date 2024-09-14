using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Context;
using Microsoft.EntityFrameworkCore;
using MyToDo.Api.Service;


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
        public async Task<ApiResponse> GetAll()=> await toDoService.GetAllAsync();

        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] ToDo toDo)=> await toDoService.AddAsync(toDo);

        [HttpPut]
        public async Task<ApiResponse> Update([FromBody] ToDo toDo) => await toDoService.UpdateAsync(toDo);

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
