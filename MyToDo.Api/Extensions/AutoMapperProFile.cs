using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Extensions
{
    public class AutoMapperProFile : Profile
    {
        public AutoMapperProFile()
        {
            base.CreateMap<ToDo, ToDoDto>().ReverseMap();
            base.CreateMap<Memo, MemoDto>().ReverseMap();
            base.CreateMap<User, UserDto>().ReverseMap();
        }

    }
}
