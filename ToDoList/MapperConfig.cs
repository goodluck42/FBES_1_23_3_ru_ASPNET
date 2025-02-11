using AutoMapper;
using ToDoList.Dtos;
using ToDoList.Entity;

namespace ToDoList;

public class MapperConfig
{
	public static Mapper Init()
	{
		var config = new MapperConfiguration(cfg =>
		{
			cfg.CreateMap<ToDoItem, ToDoItemDto>().ReverseMap();
		});
		
		return new Mapper(config);
	}
}