using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Service;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService service;

        public LoginController(ILoginService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<ApiResponse> Login([FromBody] UserDto user) => await service.LoginAsync(user.Account, user.Password);

        [HttpPost]
        public async Task<ApiResponse> Register([FromBody] UserDto user) => await service.RegisterAsync(user);


    }

}
