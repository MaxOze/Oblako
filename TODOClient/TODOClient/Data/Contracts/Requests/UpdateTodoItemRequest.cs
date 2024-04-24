namespace TODOClient.Data.Contracts.Requests;

public class UpdateTodoItemRequest
{
    public long Id { get; set; }
    public string Name { get; init; } = null!;
    public bool IsComplete { get; init; }
}