using AutoMapper;
using TODOClient.Data.Models;

namespace TODOClient.Domain.Mapper;

public class TodoMappingProfile : Profile
{
    public TodoMappingProfile()
    {
        CreateMap<TodoItemDTO, TodoItem>();
        CreateMap<TodoItem, TodoItemDTO>();
    }
}