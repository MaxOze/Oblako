using TODOClient.Data.Contracts.Requests;
using TODOClient.Domain;

namespace TODOClient.Data;

public interface ITodoService
{
    Task<List<TodoItem>> GetAllTodoItemsAsync();
    Task<TodoItem?> GetTodoItemByIdAsync(long id);
    Task<TodoItem?> CreateTodoItemAsync(CreateTodoItemRequest request);
    Task UpdateTodoItemAsync(long id, UpdateTodoItemRequest request);
    Task DeleteTodoItemAsync(long id);
}