namespace MyToDo.Api.Context.Repository
{
    public interface IToDoRepository
    {
        Task<bool> Add(ToDo toDo);
        Task<bool> Update(ToDo toDo);
        Task<bool> Delete(int id);
    }
    public interface IMemoRepository
    {
        Task<Memo> Get(int id);
        Task<Memo> Get(string id);
    }
    public class ToDoRepository : IToDoRepository
    {
        private MyToDoContext doContext;
        public ToDoRepository(MyToDoContext doContext)
        {
            this.doContext = doContext;
        }
        public async Task<bool> Add(ToDo toDo)
        {
            await doContext.ToDo.AddAsync(toDo);
            return await doContext.SaveChangesAsync() > 0;
        }
        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
        public Task<bool> Update(ToDo toDo)
        {
            throw new NotImplementedException();
        }
    }
}
