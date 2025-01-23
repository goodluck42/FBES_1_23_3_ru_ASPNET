using ToDoList.Models;

namespace ToDoList.Dtos;

public record ToDoItemDto(string Title, string? Description, ToDoItemPriority Priority);