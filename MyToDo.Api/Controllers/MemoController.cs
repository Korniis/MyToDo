using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyMemo.Api.Controllers
{
    /// <summary>
    /// 备忘录
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MemoController : ControllerBase
    {

        private readonly IMemoService meMoService;
        public MemoController(IMemoService meMoService)
        {

            this.meMoService = meMoService;
        }
        [HttpGet]
        public async Task<ApiResponse> Get(int id) => await meMoService.GetSingleAsync(id);


        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] QueryParameter queryParameter) => await meMoService.GetAllAsync(queryParameter);

        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] MemoDto meMo) => await meMoService.AddAsync(meMo);

        [HttpPut]
        public async Task<ApiResponse> Update([FromBody] MemoDto meMo) => await meMoService.UpdateAsync(meMo);

        [HttpDelete]
        public async Task<ApiResponse> Delete(int id) => await meMoService.DeleteAsync(id);



    }
}
