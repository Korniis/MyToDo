namespace MyToDo.Api.Service
{
    public interface IBaseService<T>
    {
        public Task<ApiResponse> GetAll();
        public Task<ApiResponse> GetSingle(int id);
        public Task<ApiResponse> Add(T Model);
        public Task<ApiResponse> Update(T Model);
        public Task<ApiResponse> Delete(int id);


    }
}
