// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Reflection;
using CommunityToolkit.Maui;
using GraphMAUI.Services;
using GraphMAUI.ViewModels;
using GraphMAUI.Views;
using Microsoft.Extensions.Configuration;

namespace GraphMAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.AddAppSettings()
			.RegisterAppServices()
			.RegisterViewModels()
			.RegisterViews();

		return builder.Build();
	}

	private static MauiAppBuilder AddAppSettings(this MauiAppBuilder builder)
	{
		var assembly = Assembly.GetExecutingAssembly();
		var appName = assembly.GetName().Name;

		using var appSettingsStream = assembly.GetManifestResourceStream($"{appName}.appSettings.json");
		using var appSettingsDevStream = assembly.GetManifestResourceStream($"{appName}.appSettings.Development.json");

		// Add appSettings.json to configuration
		var configBuilder = new ConfigurationBuilder()
			.AddJsonStream(appSettingsStream);

        // Optionally use appSettings.Development.json to override values in
        // appSettings.json that shouldn't be committed to source control
        if (appSettingsDevStream != null) configBuilder.AddJsonStream(appSettingsDevStream);

		builder.Configuration.AddConfiguration(configBuilder.Build());

		return builder;
	}

	private static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
	{
		builder.Services.AddSingleton<ISettingsService, SettingsService>();
		builder.Services.AddSingleton<IAlertService, AlertService>();
		builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
		builder.Services.AddSingleton<IGraphService, GraphService>();
		builder.Services.AddSingleton<INavigationService, MauiNavigationService>();
		return builder;
	}

    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<AppShellViewModel>();
		builder.Services.AddSingleton<CalendarViewModel>();
		builder.Services.AddSingleton<NewEventViewModel>();
        return builder;
    }

    private static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
		builder.Services.AddTransient<FlyoutHeaderView>();
		builder.Services.AddTransient<WelcomeView>();
		builder.Services.AddTransient<CalendarView>();
		builder.Services.AddTransient<NewEventView>();
        return builder;
    }
}
