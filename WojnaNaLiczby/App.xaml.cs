using System.Configuration;
using System.Windows;

using BoxOfficeGuesser.Windows;
using BoxOfficeGuesser.ViewModel;
using BoxOfficeGuesser.Stores;
using BoxOfficeGuesser.EntityModels;
using BoxOfficeGuesser.Factories;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using Microsoft.EntityFrameworkCore;

namespace BoxOfficeGuesser;

public partial class App : Application
{
    public IHost AppHost { get; private set; }

    public App()
    {
        string appSettingsFilePath = "Resources/appsettings.json";

        AppHost = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                _ = config.AddJsonFile(appSettingsFilePath, optional: false, reloadOnChange: true);
            })
            .ConfigureServices((hostContext, services) =>
            {
                IConfiguration config = hostContext.Configuration;

                _ = services.AddDbContext<AppDbContext>(options => 
                {
                    _ = options.UseSqlite(config.GetConnectionString("SqliteConn"));
                });

                _ = services.AddScoped<HighscoresStore>();
                _ = services.AddSingleton<NavigationStore>(sp =>
                {
                    return new(vmType => (ViewModelBase) sp.GetRequiredService(vmType));
                });

                _ = services.AddSingleton<MovieStore>(sp =>
                {
                    string filePath = config["MovieFilePath"] ?? throw new ConfigurationErrorsException($"Missing movies csv file path in '{appSettingsFilePath}'");
                    return new(filePath);
                });

                _ = services.AddSingleton<GameViewModelFactory>();
                _ = services.AddSingleton<StartGameCommandFactory>();

                _ = services.AddSingleton<MainWindow>();
                _ = services.AddSingleton<MainViewModel>();
                _ = services.AddTransient<HighscoresViewModel>();
                _ = services.AddTransient<GameCreationViewModel>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost.StartAsync();
        MainWindow startWindow = AppHost.Services.GetRequiredService<MainWindow>();
        startWindow.DataContext = AppHost.Services.GetRequiredService<MainViewModel>();
        NavigationStore navigationStore = AppHost.Services.GetRequiredService<NavigationStore>();
        navigationStore.WindowTarget = startWindow;
        navigationStore.NavigateTo<GameCreationViewModel>();
        startWindow.Show();
        
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost.StopAsync();
        base.OnExit(e);
    }
}

