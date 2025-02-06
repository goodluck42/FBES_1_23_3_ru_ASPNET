using ToDoAPI.Entity;

namespace ToDoAPI.Dtos;

public record ToDoItemDto(string Title, string? Description, ToDoItemPriority Priority);