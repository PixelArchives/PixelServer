using PixelServer.Objects;
using System.Security.Cryptography;
using System.Text;

namespace PixelServer.Helpers;

public static class HashHelper
{
    private const string pass = "pI}{eLG4nS()()P3R53CREt";

    private static HMAC hmac = new HMACSHA1(Encoding.UTF8.GetBytes(pass), true);

    public static bool IsValid(ActionForm form)
    {
        if (string.IsNullOrEmpty(form.auth) || string.IsNullOrEmpty(form.action)) return false;

        string? text2 = form.action == "get_player_online" ? "*:*.*.*"
                     : (form.platform != null ? $"{(int)form.platform}:{form.app_version}" : null);

        //string text2 = ((!form.action.Equals("get_player_online")) ? ((int)form.platform + ":" + form.app_version) : "*:*.*.*");

        string? text = form.token ?? form.uniq_id;

        if (text2 == null || text == null)
            return false;

        string s = text2 + text + form.action; //string.Concat(text2, text, form.action);
        DebugHelper.Log("Hashing: " + "\"" + s + "\"");
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(s));
        string text3 = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();

        DebugHelper.Log($"Input hash: {form.auth} Computed hash: {text3}");

        return form.auth == text3;
    }
}
