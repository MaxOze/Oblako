using System.Windows;

namespace TODOClient.Presentation;

public partial class MainWindow
{
    private bool _isUpdate { get; set; }
    
    private readonly MainWindowViewModel _viewModel;
    
    public MainWindow(MainWindowViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = viewModel;
        InitializeComponent();
    }

    private async void MainWindow_OnInitialized(object? sender, EventArgs e) => await _viewModel.GetTodoListAsync();

    private async void GetAllItems_OnClick(object sender, RoutedEventArgs e) => await _viewModel.GetTodoListAsync();

    private async void GetItemById_OnClick(object sender, RoutedEventArgs e) => await _viewModel.GetTodoItemAsync();

    private void CreateItem_OnClick(object sender, RoutedEventArgs e) => _viewModel.IsChange = true;

    private void UpdateItem_OnClick(object sender, RoutedEventArgs e)
    {
        if (_viewModel.SelectedTodoItem is null)
        {
            MessageBox.Show("Выберите задание");
            return;
        }

        _viewModel.Id = _viewModel.SelectedTodoItem.Id;
        _viewModel.Name = _viewModel.SelectedTodoItem.Name;
        _viewModel.Checked = _viewModel.SelectedTodoItem.IsComplete;
        _viewModel.IsChange = true;
        _isUpdate = true;
    }

    private async void DeleteItem_OnClick(object sender, RoutedEventArgs e)
    {
        if (_viewModel.SelectedTodoItem is null)
        {
            MessageBox.Show("Выберите задание");
            return;
        }
        
        _viewModel.Id = _viewModel.SelectedTodoItem.Id;
        await _viewModel.DeleteTodoItemAsync();
    }
    
    private async void SaveChanges_OnClick(object sender, RoutedEventArgs e)
    {
        if (_isUpdate)
            await _viewModel.UpdateTodoItemAsync();
        else
            await _viewModel.CreateTodoItemAsync();
    }
}