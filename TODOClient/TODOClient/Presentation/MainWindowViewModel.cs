using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using TODOClient.Data;
using TODOClient.Data.Contracts.Requests;
using TODOClient.Domain;

namespace TODOClient.Presentation;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly ITodoService _todoService;

    [ObservableProperty] private ObservableCollection<TodoItem> _todoList = [];
    [ObservableProperty] private TodoItem? _selectedTodoItem;
    [ObservableProperty] private string _idString = "";
    [ObservableProperty] private long _id;
    [ObservableProperty] private string _name = "";
    [ObservableProperty] private bool _checked;
    [ObservableProperty] private bool _isChange;


    public MainWindowViewModel(ITodoService todoService) => _todoService = todoService;

    public async Task GetTodoListAsync()
    {
        var newList = await _todoService.GetAllTodoItemsAsync();
        TodoList.Clear();
        newList.ForEach(item => TodoList.Add(item));
    }

    public async Task GetTodoItemAsync()
    {
        if (!long.TryParse(IdString, out var longId))
        {
            MessageBox.Show("Введите корректный ID");
            return;
        }

        var todoItem = await _todoService.GetTodoItemByIdAsync(longId);

        if (todoItem is null)
        {
            MessageBox.Show("Задача с таким ID не найдена");
            return;
        }
        
        TodoList.Clear();
        TodoList.Add(todoItem);
    }

    public async Task CreateTodoItemAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            MessageBox.Show("Введите задание");
            return;
        }

        var request = new CreateTodoItemRequest
        {
            Name = Name,
            IsComplete = Checked
        };

        var newTodoItem = await _todoService.CreateTodoItemAsync(request);

        if (newTodoItem is not null)
            TodoList.Add(newTodoItem);

        Name = "";
        Checked = false;
    }

    public async Task UpdateTodoItemAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            MessageBox.Show("Введите задание");
            return;
        }

        var request = new UpdateTodoItemRequest
        {
            Id = Id,
            Name = Name,
            IsComplete = Checked
        };

        await _todoService.UpdateTodoItemAsync(request.Id, request);
        
        Name = "";
        Checked = false;
    }
    
    public async Task DeleteTodoItemAsync()
    {
        await _todoService.DeleteTodoItemAsync(Id);
        TodoList.Remove(TodoList.First(item => item.Id == Id));
    }
}