using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Service
{
    public class ToDoService : IToDoService
    {
        public readonly IUnitOfWork work;
        private readonly IMapper mapper;
        public ToDoService(IUnitOfWork work,IMapper mapper)
        {
            this.mapper = mapper;
            this.work = work;

        }
        public async Task<ApiResponse> AddAsync(ToDoDto Model)
        {
            try
            {
                var todo = mapper.Map<ToDo>(Model);
                todo.CreatDate = DateTime.UtcNow;
                todo.UpdateDate = DateTime.UtcNow;
                await work.GetRepository<ToDo>().InsertAsync(todo);
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

        public async Task<ApiResponse> UpdateAsync(ToDoDto Model)
        {
            try
            {
                var dbtodo = mapper.Map<ToDo>(Model);
                var todo = await work.GetRepository<ToDo>().GetFirstOrDefaultAsync(predicate: t => t.Id == Model.Id); ;
                todo.UpdateDate = DateTime.Now;
                todo.Status = dbtodo.Status;
                todo.Title = dbtodo.Title;
                todo.Content = dbtodo.Content;
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
