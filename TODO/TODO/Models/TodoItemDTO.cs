﻿namespace TODO.Models;

public class TodoItemDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public bool IsComplete { get; set; }

    public TodoItemDTO() {}
    
    public TodoItemDTO(TodoItem todoItem)
    {
        Id = todoItem.Id;
        Name = todoItem.Name;
        IsComplete = todoItem.IsComplete;
    }
}