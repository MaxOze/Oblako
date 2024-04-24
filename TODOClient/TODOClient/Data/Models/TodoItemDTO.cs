namespace TODOClient.Data.Models;

public class TodoItemDTO
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsComplete { get; set; }
}