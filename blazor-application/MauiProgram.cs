using BlazorBLE.Services;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace BlazorBLE;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        SetupSerilog();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<BLEScannerService>();
        builder.Services.AddSingleton<IPromptService, PromptService>();

        return builder.Build();
    }

    private static void SetupSerilog()
    {
        // How frequently logs are flushed - set to 1 second
        var flushInterval = new TimeSpan(0, 0, 1);

        var file = Path.Combine(FileSystem.AppDataDirectory, "BlazorBLELogs.log");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.File(file, flushToDiskInterval: flushInterval, encoding: System.Text.Encoding.UTF8, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 22)
            .CreateLogger();
    }

}
