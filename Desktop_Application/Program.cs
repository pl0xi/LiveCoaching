using System;
using System.Net.Http;
using Avalonia;
using Avalonia.ReactiveUI;
using LiveCoaching.Services.Api;
using LiveCoaching.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LiveCoaching;

internal sealed class Program
{
    private static IServiceProvider? _serviceProvider;

    [STAThread]
    public static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();

        ConfigureServices(serviceCollection);

        _serviceProvider = serviceCollection.BuildServiceProvider();

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // LeagueClientApiService
        services.AddSingleton<LeagueClientApiService>();
        HttpClientHandler handler = new();
        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        services.AddHttpClient<LeagueClientApiService>().ConfigurePrimaryHttpMessageHandler(() => handler);

        // HomeViewModel & MatchHistoryViewModel
        services.AddSingleton<HomeViewModel>();
        services.AddSingleton<MatchHistoryViewModel>();
    }

    public static T GetService<T>()
    {
        return _serviceProvider.GetRequiredService<T>();
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    }
}