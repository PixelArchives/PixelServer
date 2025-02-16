using PixelServer.Controllers;
using PixelServer.Helpers;

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

    public static async Task AddBadWord(string fullCommand)
    {
        string[] vals = fullCommand.Split(' ');

        if (!ArgumentCheck(vals.Length, 3)) return;

        try
        {
            if (bool.TryParse(vals[2], out bool is_symbol))
            {
                await BadWordHelper.TryAddValue(vals[1], is_symbol);
            }
            else Console.WriteLine("Couldn't parse param [2]");
        }
        catch (Exception ex)
        {
            DebugHelper.LogError($"Couldn't add value. Ex: {ex}");
        }
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
