using PixelServer.Admin;
using PixelServer.Helpers;
using System.Diagnostics;
using System.Text;

namespace PixelServer;

public class Program
{
    public static async Task Main(string[] args)
    {
        // basic console initing
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        // Logo
        if (File.Exists("Logo.txt")) Console.WriteLine(File.ReadAllText("Logo.txt"));

        // web app init
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.UseUrls("http://127.0.0.2");

        builder.Logging.ClearProviders();

        // makes it case sensetive because FilterBadWords.json was returning lowercase value names which broke the game.
        builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

        bool inited = false;

        // MySQL init

        while (!inited)
        {
            try
            {
                await InitDatabase();
                inited = true;
            }
            catch (Exception ex)
            {
                DebugHelper.LogException("Unable to connect to database", ex, false);
                DebugHelper.Log("Press any key to retry");
                Console.ReadKey(true);
            }
        }

        // Get game version confog from database
        await VersionHelper.CheckVersions();

        var app = builder.Build();

        app.MapControllers();

        _ = AdminPanel.Run();

        app.Run();
    }

    private static async Task InitDatabase()
    {
        Stopwatch watch = Stopwatch.StartNew();
        DebugHelper.Log("Initing Databese");

        await Db.Init();

        watch.Stop();
        DebugHelper.Log($"Database Inited, time: {watch.Elapsed}");
    }
}
