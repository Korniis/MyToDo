using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Service
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LoginService(IUnitOfWork unitOfWork, IMapper mapper)
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public  async Task<ApiResponse> LoginAsync(string account, string password)
        {
            try
            {
               // password = password.GetMD5();
                var model = await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(predicate: u => u.Account.Equals(account) &&
                u.PassWord.Equals(password));
                if (model == null)
                {
                    return new ApiResponse("账号密码错误，请重新输入");
                }
                return new ApiResponse(true, model);
            }
            catch (Exception ex)
            {

                return new ApiResponse(false, "登录失败");
            }
        }

        public async Task<ApiResponse> RegisterAsync(UserDto user)
        {
            try
            {
                var model = _mapper.Map<User>(user);
                var repository = _unitOfWork.GetRepository<User>();
                var isUser = await repository.GetFirstOrDefaultAsync(predicate: u => u.Account.Equals(model.Account));
                if (isUser != null)
                {
                    return new ApiResponse($"当前账号：{model.Account}已存在，请重新输入！");
                }

                model.CreateDate = DateTime.Now;
               // model.PassWord = model.PassWord.GetMD5();
                await repository.InsertAsync(model);

                if (await _unitOfWork.SaveChangesAsync() > 0)
                {
                    return new ApiResponse(true, model);
                }
                return new ApiResponse("注册账号失败，请稍后重试！");
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, "注册账号失败");
            }
        }
    }
}
