using MyToDo.Api.Context;

namespace MyToDo.Api.Service
{
    public class ToDoService : IToDoService
    {
        public readonly IUnitOfWork work;
        public ToDoService(IUnitOfWork work)
        {
            this.work = work;

        }
        public async Task<ApiResponse> AddAsync(ToDo Model)
        {
            try
            {
                Model.CreatDate = DateTime.UtcNow;
                Model.UpdateDate = DateTime.UtcNow;
                await work.GetRepository<ToDo>().InsertAsync(Model);
                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, Model);
                return new ApiResponse("添加数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }

        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                var todo = await work.GetRepository<ToDo>().GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                work.GetRepository<ToDo>().Delete(todo);
                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, todo);
                return new ApiResponse("添加数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllAsync()
        {
            try
            {

                IList<ToDo> toDos = await work.GetRepository<ToDo>().GetAllAsync();

                return new ApiResponse(true, toDos);

            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            try
            {

                ToDo value = await work.GetRepository<ToDo>().GetFirstOrDefaultAsync(predicate: t => t.Id == id);


                return new ApiResponse(true, value);

            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(ToDo Model)
        {
            try
            {
                var todo = await work.GetRepository<ToDo>().GetFirstOrDefaultAsync(predicate: t => t.Id == Model.Id); ;
                todo.UpdateDate = DateTime.Now;
                todo.Status = Model.Status;
                todo.Title = Model.Title;
                todo.Content = Model.Content;
                work.GetRepository<ToDo>().Update(todo);
                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, todo);
                return new ApiResponse("修改数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }
}
