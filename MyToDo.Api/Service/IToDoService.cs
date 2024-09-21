using MyToDo.Api.Context;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;


namespace MyToDo.Api.Service
{
    public interface  IToDoService:IBaseService<ToDoDto>
    {
        public  Task<ApiResponse> GetAllAsync(ToDoParameter queryParameter);
    }
}
