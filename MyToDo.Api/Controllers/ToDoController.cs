using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Context;
using Microsoft.EntityFrameworkCore;


namespace MyToDo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[acgion]")]
    public class ToDoController:Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public ToDoController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
          var repository=  unitOfWork.GetRepository<ToDo>();
           var todo= await  repository.GetFirstOrDefaultAsync(predicate: t=>t.Id.Equals(id));

            return todo; 
         }
    }
}
