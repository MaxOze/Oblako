using System.Net.Http;
using System.Text;
using AutoMapper;
using Newtonsoft.Json;
using TODOClient.Data.Contracts.Requests;
using TODOClient.Data.Models;
using TODOClient.Domain;

namespace TODOClient.Data;

public class TodoService : ITodoService
{
    private const string Uri = "http://localhost:5043/";
    private readonly HttpClient _httpClient = new(new HttpClientHandler { AllowAutoRedirect = true });

    private readonly IMapper _mapper;

    public TodoService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<List<TodoItem>> GetAllTodoItemsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync(Uri + "todo");
            
            response.EnsureSuccessStatusCode();
            var list = JsonConvert.DeserializeObject<List<TodoItemDTO>>(await response.Content.ReadAsStringAsync());

            return _mapper.Map<List<TodoItem>>(list);
        }
        catch
        {
            return [];
        }
    }

    public async Task<TodoItem?> GetTodoItemByIdAsync(long id)
    {
        try
        {
            var response = await _httpClient.GetAsync(Uri + "todo/" + id);
            
            response.EnsureSuccessStatusCode();
            var item = JsonConvert.DeserializeObject<TodoItemDTO>(await response.Content.ReadAsStringAsync());

            return _mapper.Map<TodoItem>(item);
        }
        catch
        {
            return null;
        }
    }

    public async Task<TodoItem?> CreateTodoItemAsync(CreateTodoItemRequest request)
    {
        try
        {
            var requestString = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Uri + "todo/", requestString);
            
            response.EnsureSuccessStatusCode();

            if (response.Headers.Location is null)
                return null;
            
            var getResponse = await _httpClient.GetAsync(response.Headers.Location);
            
            getResponse.EnsureSuccessStatusCode();
            var item = JsonConvert.DeserializeObject<TodoItemDTO>(await getResponse.Content.ReadAsStringAsync());

            return _mapper.Map<TodoItem>(item);
        }
        catch
        {
            return null;
        }
    }

    public async Task UpdateTodoItemAsync(long id, UpdateTodoItemRequest request)
    {
        try
        {
            var requestString = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(Uri + "todo/" + id, requestString);
            
            response.EnsureSuccessStatusCode();
        }
        catch
        {
            // ignored
        }
    }

    public async Task DeleteTodoItemAsync(long id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(Uri + "todo/" + id);
            
            response.EnsureSuccessStatusCode();
        }
        catch
        {
            // ignored
        }
    }
}