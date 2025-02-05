using MySqlConnector;
using PixelServer.Helpers;
using System.Diagnostics;

namespace PixelServer;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.UseUrls("http://127.0.0.2");

        builder.Logging.ClearProviders();

        // makes it case sensetive because FilterBadWords.json was returning lowercase value names which broke the game.
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

        await InitDatabase();

        var app = builder.Build();

        app.MapControllers();

        AdminPanel.Run();

        app.Run();
    }

    private static async Task InitDatabase()
    {
        Stopwatch watch = Stopwatch.StartNew();
        DebugHelper.Log("Initing Databese", false);

        await Db.Init();

        watch.Stop();
        DebugHelper.Log($"Database Inited, time: {watch.Elapsed}", false);
    }
}
