using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Service
{
    public class MemoService : IMemoService
    {
        public readonly IUnitOfWork work;
        private readonly IMapper mapper;
        public MemoService(IUnitOfWork work, IMapper mapper)
        {
            this.mapper = mapper;
            this.work = work;

        }
        public async Task<ApiResponse> AddAsync(MemoDto Model)
        {
            try
            {
                var todo = mapper.Map<Memo>(Model);
                todo.CreateDate = DateTime.UtcNow;
                todo.UpdateDate = DateTime.UtcNow;
                await work.GetRepository<Memo>().InsertAsync(todo);
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
                var todo = await work.GetRepository<Memo>().GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                work.GetRepository<Memo>().Delete(todo);
                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, todo);
                return new ApiResponse("添加数据失败");
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

                var memos = await work.GetRepository<Memo>().GetPagedListAsync(predicate: x => string.IsNullOrWhiteSpace(queryParameter.Search) ? true : x.Title.Equals(queryParameter.Search),
                    pageIndex: queryParameter.PageIndex,
                    pageSize: queryParameter.PageSize,
                    orderBy: source => source.OrderByDescending(t => t.CreateDate)

                    );


                return new ApiResponse(true, memos);

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

                Memo value = await work.GetRepository<Memo>().GetFirstOrDefaultAsync(predicate: t => t.Id == id);


                return new ApiResponse(true, value);

            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(MemoDto Model)
        {
            try
            {
                var dbtodo = mapper.Map<Memo>(Model);
                var todo = await work.GetRepository<Memo>().GetFirstOrDefaultAsync(predicate: t => t.Id == Model.Id); ;
                todo.UpdateDate = DateTime.Now;
                todo.Title = dbtodo.Title;
                todo.Content = dbtodo.Content;
                work.GetRepository<Memo>().Update(todo);
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
