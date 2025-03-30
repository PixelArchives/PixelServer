namespace PixelServer.Utils;

public static class ConvertV2
{
    public static string SafeString(object obj, string defaultValue = "Null") =>
        obj == DBNull.Value ? defaultValue : obj.ToString() ?? defaultValue;
}
