namespace TODOClient.Data.Contracts.Requests;

public class CreateTodoItemRequest
{
    public string Name { get; init; } = null!;
    public bool IsComplete { get; init; }
}