using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TODOClient.Data;
using TODOClient.Presentation;

namespace TODOClient;

public partial class App : Application
{
    public static IHost AppHost { get; private set; }
    
    public static T GetService<T>() where T : class => AppHost.Services.GetService<T>()!;
    
    public App()
    {
        AppHost = Host
            .CreateDefaultBuilder()
            .UseContentRoot(AppContext.BaseDirectory)
            .UseDefaultServiceProvider((context, options) => { options.ValidateOnBuild = true; })
            .ConfigureServices((context, services) =>
            {
                services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                services.AddTransient<MainWindow>();
                services.AddTransient<MainWindowViewModel>();
                services.AddTransient<ITodoService, TodoService>();
            })
            .Build();
        
        
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var vm = GetService<MainWindowViewModel>();
        var window = new MainWindow(vm);
        window.ShowDialog();
    }
}