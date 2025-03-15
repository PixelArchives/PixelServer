using PixelServer.Objects;
using System.Security.Cryptography;
using System.Text;

namespace PixelServer.Helpers;

public static class HashHelper
{
    private const string pass = "pI}{eLG4nS()()P3R53CREt";

    private readonly static HMAC hmac = new HMACSHA1(Encoding.UTF8.GetBytes(pass));

    public static bool IsValid(ActionForm form)
    {
        //return true;

        if (string.IsNullOrEmpty(form.auth) || string.IsNullOrEmpty(form.action)) return false;

        string? text2 = form.action == "get_player_online" ? "*:*.*.*"
                     : (form.platform != null ? $"{(int)form.platform}:{form.app_version}" : null);

        //string text2 = ((!form.action.Equals("get_player_online")) ? ((int)form.platform + ":" + form.app_version) : "*:*.*.*");

        string? text = form.token ?? form.uniq_id.ToString();

        if (text2 == null || text == null)
            return false;

        //string s = text2 + text + form.action; //string.Concat(text2, text, form.action);
        string s = string.Concat(text2, text, form.action);
        DebugHelper.Log("Hashing: " + "\"" + s + "\"");
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(s));
        string text3 = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();

        DebugHelper.Log($"Input hash: {form.auth} Computed hash: {text3}");

        return form.auth == text3;
        // Rewrite this shit cuz PG3D request logic is fucking retarded
    }

    /* OG code from 10.3.1
     * 	private static HMAC _hmac;
     * 	_hmac = new HMACSHA1(Encoding.UTF8.GetBytes(secret), true);
     * 	
    public static string Hash(string action, string token = null)
	{
		if (action == null)
		{
			Debug.LogWarning("Hash: action is null");
			return string.Empty;
		}
		string text = token ?? ((!(sharedController != null)) ? null : sharedController.id);
		if (text == null)
		{
			Debug.LogWarning("Hash: Token is null");
			return string.Empty;
		}
		string text2 = ((!action.Equals("get_player_online")) ? (ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion) : "*:*.*.*");
		string s = text2 + text + action;
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		byte[] array = _hmac.ComputeHash(bytes);
		string text3 = BitConverter.ToString(array);
		Debug.LogError("Hashed: " + "\"" + s + "\"");
		return text3.Replace("-", string.Empty).ToLower();
	    }
    */

}
