namespace MyToDo.Api.Service
{
    public interface IBaseService<T>
    {
        public Task<ApiResponse> GetAllAsync();
        public Task<ApiResponse> GetSingleAsync(int id);
        public Task<ApiResponse> AddAsync(T Model);
        public Task<ApiResponse> UpdateAsync(T Model);
        public Task<ApiResponse> DeleteAsync(int id);


    }
}
