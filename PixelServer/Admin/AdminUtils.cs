using PixelServer.Helpers;

namespace PixelServer.Admin;

///<summary>Separate class for AdminPanel's commands</summary>
public static class AdminUtils
{
    public static async Task BanOrUnban(string fullCommand)
    {
        string[] vals = fullCommand.Split(' ');

        if (vals.Length > 2)
        {
            DebugHelper.LogWarning("Too much arguments");
            return;
        }
        else if (vals.Length < 2)
        {
            DebugHelper.LogWarning("Too few arguments");
            return;
        }

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
        if (vals.Length > 3)
        {
            DebugHelper.LogWarning("Too much arguments");
            return;
        }
        else if (vals.Length < 3)
        {
            DebugHelper.LogWarning("Too few arguments");
            return;
        }

        try
        {
            if (bool.TryParse(vals[2], out bool is_symbol))
            {
                bool b = await BadWordHelper.AddValue(vals[1], is_symbol);
                if (!b)
                {
                    DebugHelper.LogWarning("Couldn't add value");
                    return;
                }
            }
            else Console.WriteLine("Couldn't parse 'is_symbol'");
        }
        catch (Exception ex)
        {
            DebugHelper.LogError($"Couldn't add value. Ex: {ex}");
        }

        DebugHelper.Log($"Added value \"{vals[1]}\" successfuly.");
    }
}
