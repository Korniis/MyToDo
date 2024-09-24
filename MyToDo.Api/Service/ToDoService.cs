using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using MyToDo.ViewModels;
using System.Collections.ObjectModel;

namespace MyToDo.Api.Service
{
    public class ToDoService : IToDoService
    {
        public readonly IUnitOfWork work;
        private readonly IMapper mapper;
        public ToDoService(IUnitOfWork work, IMapper mapper)
        {
            this.mapper = mapper;
            this.work = work;

        }
        public async Task<ApiResponse> AddAsync(ToDoDto Model)
        {
            try
            {
                var todo = mapper.Map<ToDo>(Model);
                todo.CreateDate = DateTime.UtcNow;
                todo.UpdateDate = DateTime.UtcNow;
                await work.GetRepository<ToDo>().InsertAsync(todo);
                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, todo);
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

        public async Task<ApiResponse> GetAllAsync(ToDoParameter queryParameter)
        {
            try
            {

                var toDos = await work.GetRepository<ToDo>().GetPagedListAsync(predicate: x => (string.IsNullOrWhiteSpace(queryParameter.Search) ? true : x.Title.Contains(queryParameter.Search))
                && (queryParameter.Status == null ? true : x.Status.Equals(queryParameter.Status)),
                pageIndex: queryParameter.PageIndex,
                pageSize: queryParameter.PageSize,
                orderBy: source => source.OrderByDescending(t => t.CreateDate));


                return new ApiResponse(true, toDos);

            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllAsync(QueryParameter queryParameter)
        {
            try
            {

                var toDos = await work.GetRepository<ToDo>().GetPagedListAsync(predicate: x => string.IsNullOrWhiteSpace(queryParameter.Search) ? true : x.Title.Equals(queryParameter.Search)
             ,
                       pageIndex: queryParameter.PageIndex,
                       pageSize: queryParameter.PageSize,

                       orderBy: source => source.OrderByDescending(t => t.CreateDate)

                       );


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

        public async Task<ApiResponse> Summary()
        {
            try
            {
                //待办事项结果
                var todos = await work.GetRepository<ToDo>().GetAllAsync(
                    orderBy: source => source.OrderByDescending(t => t.CreateDate));

                //备忘录结果
                var memos = await work.GetRepository<Memo>().GetAllAsync(
                    orderBy: source => source.OrderByDescending(t => t.CreateDate));

                SummaryDto summary = new SummaryDto();
                summary.Sum = todos.Count(); //汇总待办事项数量
                summary.CompletedCount = todos.Where(t => t.Status == 1).Count(); //统计完成数量
                summary.CompletedRatio = (summary.CompletedCount / (double)summary.Sum).ToString("0%"); //统计完成率
                summary.MemoeCount = memos.Count();  //汇总备忘录数量
                summary.ToDoList = new ObservableCollection<ToDoDto>(mapper.Map<List<ToDoDto>>(todos.Where(t => t.Status == 0)));
                summary.MemoList = new ObservableCollection<MemoDto>(mapper.Map<List<MemoDto>>(memos));

                return new ApiResponse(true, summary);
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, "");
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
