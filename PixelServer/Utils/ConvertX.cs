namespace PixelServer.Utils;

public static class ConvertX
{
    public static string SafeString(object obj, string defaultValue = "Null") =>
        obj == DBNull.Value ? defaultValue : obj.ToString() ?? defaultValue;
}
