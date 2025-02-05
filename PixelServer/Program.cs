using MySqlConnector;
using PixelServer.Helpers;

namespace PixelServer;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.UseUrls("http://127.0.0.2");

        builder.Logging
            .ClearProviders()
            .SetMinimumLevel(LogLevel.Warning);

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null; // Keeps PascalCase
            });

        builder.Logging.AddSimpleConsole();

        DebugHelper.logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("Global");

        await Db.Init();

        var app = builder.Build();

        app.MapControllers();

        app.Run();
    }
}
