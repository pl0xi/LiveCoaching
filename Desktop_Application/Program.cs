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
        // TODO: Create global for patch version: https://ddragon.leagueoflegends.com/api/versions.json
        SetupServices();

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    private static void SetupServices()
    {
        var services = new ServiceCollection();

        // LeagueClientApiService
        services.AddSingleton<LeagueClientApiService>();
        HttpClientHandler handler = new();
        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        services.AddHttpClient<LeagueClientApiService>().ConfigurePrimaryHttpMessageHandler(() => handler);

        // LeagueWebApiService
        services.AddSingleton<LeagueWebApiService>();
        services.AddHttpClient<LeagueWebApiService>();

        // HomeViewModel & MatchHistoryViewModel
        services.AddSingleton<HomeViewModel>();
        services.AddSingleton<MatchHistoryViewModel>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public static T GetService<T>() where T : notnull
    {
        // TODO: Only compile at development 
        // For development purpose only
        if (_serviceProvider == null) SetupServices();

        return _serviceProvider.GetRequiredService<T>();
    }

    private static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    }
}