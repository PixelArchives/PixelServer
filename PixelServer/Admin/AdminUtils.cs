using PixelServer.Controllers;
using PixelServer.Helpers;
using System.Text;

namespace PixelServer.Admin;

///<summary>Separate class for AdminPanel's commands</summary>
public static class AdminUtils
{
    public static async Task BanOrUnban(string fullCommand)
    {
        string[] vals = fullCommand.Split(' ');

        if (!ArgumentCheck(vals.Length, 2)) return;

        try
        {
            await AccountHelper.BanPlayer(long.Parse(vals[1]), vals[0] == AdminConsts.unban);
        }
        catch (Exception ex)
        {
            DebugHelper.LogError($"Couldn't execute command. Ex: {ex.Message}");
            return;
        }

        DebugHelper.Log("Executed command succesfully");
    }

    public static async Task BadFilter(string fullCommand)
    {
        string[] vals = fullCommand.Split(' ');

        if (vals.Length == 2 && vals[1] == "log")
        {
            await BadFilterLog();
            return;
        }

        if (!ArgumentCheck(vals.Length, 3)) return;

        try
        {
            if (vals[1] == "add") await BadFilterHelper.TryAddValue(vals[2]);
            else if (vals[1] == "remove") await BadFilterHelper.TryRemoveValue(vals[2]);
            else DebugHelper.LogWarning("Unable to parse parameter [1]");
        }
        catch (Exception ex)
        {
            DebugHelper.LogError($"Couldn't add value. Ex: {ex}");
        }
    }

    public static async Task BadFilterLog()
    {
        var container = await BadFilterHelper.GetOrCreate();

        StringBuilder builder = new();

        builder.Append("Words: ");
        foreach (string v in container.Words)
        {
            builder.Append(v);
            builder.Append(", ");
        }

        builder.AppendLine("Symbols: ");
        foreach (char v in container.Symbols)
        {
            builder.Append(v);
            builder.Append(", ");
        }

        DebugHelper.Log(builder.ToString());
    }

    public static async Task ModGameVer(string fullCommand)
    {
        string[] vals = fullCommand.Split(' ');

        if (!ArgumentCheck(vals.Length, 3)) return;

        if (vals[1] != "add" && vals[1] != "remove")
        {
            DebugHelper.LogWarning("Unable to parse second argument");
        }

        bool isAdd = vals[1] == "add";

        if (isAdd) await VersionHelper.AddVersion(vals[2]);
        else await VersionHelper.RemoveVersion(vals[2]);
    }

    static bool ArgumentCheck(int ammount, int max) => ArgumentCheck(ammount, max, max);

    static bool ArgumentCheck(int ammount, int min, int max)
    {
        if (ammount > max)
        {
            DebugHelper.LogWarning("Too much arguments");
            return false;
        }
        else if (ammount < min)
        {
            DebugHelper.LogWarning("Too few arguments");
            return false;
        }

        return true;
    }
}
