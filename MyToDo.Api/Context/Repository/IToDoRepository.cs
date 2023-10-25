using Microsoft.EntityFrameworkCore;

namespace MyToDo.Api.Context.Repository
{
    public interface IToDoRepository
    {
        public Task<bool> Add(ToDo toDo);
        public Task<bool> Update(ToDo toDo);
        public Task<bool> Delete(int id);



    }
    public class ToDoRepository : IToDoRepository
    {
        private MyToDoContext doContext;
        public ToDoRepository(MyToDoContext doContext)
        {

            this.doContext = doContext;
        }

        async Task<bool> Add(ToDo toDo)
        {
            await doContext.ToDo.AddAsync(toDo);
            return await doContext.SaveChangesAsync() > 0;

        }

        Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        Task<bool> Update(ToDo toDo)
        {
            throw new NotImplementedException();
        }
    }
}
