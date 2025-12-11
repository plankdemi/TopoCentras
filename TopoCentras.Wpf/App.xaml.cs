using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TopoCentras.Core.Interfaces;
using TopoCentras.Core.Services;
using TopoCentras.Data;
using TopoCentras.Data.Repositories;
using TopoCentras.Wpf.ViewModels;
using TopoCentras.Wpf.Views;

namespace TopoCentras.Wpf;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider = null!;

    public App()
    {
        var services = new ServiceCollection();
        ConfigurationServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }

    private void ConfigurationServices(IServiceCollection services)
    {
        const string connectionString =
            "Server=127.0.0.1;Port=3306;Database=TopoCentrasDb;User=user;Password=1234;";

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        services.AddScoped<IKlientasRepository, KlientasRepository>();
        services.AddScoped<IPrekeRepository, PrekeRepository>();
        services.AddScoped<IUzsakymasPrekeRepository, UzsakymasPrekeRepository>();
        services.AddScoped<IUzsakymasRepository, UzsakymasRepository>();


        services.AddScoped<IKlientasService, KlientasService>();
        services.AddScoped<IPrekeService, PrekeService>();
        services.AddScoped<IUzsakymasPrekeService, UzsakymasPrekeService>();
        services.AddScoped<IUzsakymasService, UzsakymasService>();


        services.AddScoped<MainWindowViewModel>();

        services.AddScoped<UzsakymaiViewModel>();
        services.AddScoped<CreateUzsakymasViewModel>();
        services.AddScoped<PrekesViewModel>();
        services.AddScoped<KlientaiViewModel>();
        services.AddTransient<MainWindow>();
    }

    private void OnStartup(object sender, StartupEventArgs e)
    {
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}