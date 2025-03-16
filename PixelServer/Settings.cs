namespace PixelServer;

public static class Settings
{
    public const string mySqlConnectionString = "Server=127.0.0.1;Port=3306;User ID=root;Password=;Database=pixel_server;";

    //Routes
    public const string mainRoute = "/";

    public const string pg3dConfig = "/pixelgun3d-config";

    // Etc settings

    ///<summary>Enable bad word/symbol filtering.</summary>
    public const bool badWordFiltering = true;

    ///<summary>Actions excluded from hash checking.</summary>
    public static readonly string[] excludeActionsFormHashing = ["check_version"];
}
