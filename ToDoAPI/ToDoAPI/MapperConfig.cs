using AutoMapper;
using ToDoAPI.Dtos;
using ToDoAPI.Entity;

namespace ToDoAPI;

public class MapperConfig
{
	public static Mapper Init()
	{
		var config = new MapperConfiguration(cfg =>
		{
			cfg.CreateMap<ToDoItem, ToDoItemDto>().ReverseMap();
			cfg.CreateMap<Account, AccountDto>().ReverseMap();
		});

		return new Mapper(config);
	}
}