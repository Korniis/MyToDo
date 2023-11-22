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
        public async Task<ApiResponse> Add(ToDo Model)
        {
            try
            {
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

        public async Task<ApiResponse> Delete(int id)
        {
            try
            {
                var todo = await work.GetRepository<ToDo>().GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, Model);
                return new ApiResponse("添加数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public Task<ApiResponse> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetSingle(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> Update(ToDo Model)
        {
            throw new NotImplementedException();
        }
    }
}
