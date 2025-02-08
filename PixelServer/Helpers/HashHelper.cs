using PixelServer.Objects;
using System;
using System.Security.Cryptography;
using System.Text;

namespace PixelServer.Helpers;

public static class HashHelper
{
    private const string pass = "pI}{eLG4nS()()P3R53CREt";

    private static HMAC hmac = new HMACSHA1(Encoding.UTF8.GetBytes(pass));

    public static bool IsValid(ActionForm form)
    {
        if (string.IsNullOrEmpty(form.auth) || string.IsNullOrEmpty(form.action)) return false;

        string? first = form.action == "get_player_online" ? "*:*.*.*"
                     : (form.platform != null ? $"{(int)form.platform}:{form.app_version}" : null);

        string? second = form.token ?? form.uniq_id;

        if (first == null || second == null)
            return false;

        string input = string.Concat(first, second, form.action);
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
        return form.auth == BitConverter.ToString(hash);
    }
}
