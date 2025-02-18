using Microsoft.AspNetCore.Mvc;
using PixelServer.Helpers;
using PixelServer.Objects;
using System.Text.Json;

namespace PixelServer.Controllers;

[ApiController]
[Route(Settings.mainRoute)]
public class MainController
{
    [Route("action.php")]
    public async Task<string> Action([FromForm] ActionForm form)
    {
        if (string.IsNullOrWhiteSpace(form.action)) return "fail";

        //if (!Settings.excludeActionsFormHashing.Contains(form.action) && !HashHelper.IsValid(form)) return "fail";

#if DEBUG
        DebugHelper.Log(form);
#endif

        switch (form.action)
        {
            // Game Version Checks
            case "check_version":
                return VersionHelper.IsValid(form.app_version) ? "valid" : "no";

            case "check_shop_version":
                return VersionHelper.IsValid(form.app_version) ? "valid" : "no";

            // Account creating and etc
            case "create_player_intent":
                return await AccountHelper.CreateAccountToken();

            case "create_player":
                long id = await AccountHelper.CreateAccount(form.token);
                return id.ToString();
            
                // Account info getting.
            case "start_check":
                return await AccountHelper.GetOrCreate(form.uniq_id);

            case "get_info_by_id":
                return JsonSerializer.Serialize(await AccountHelper.GetInfoById(form.uniq_id));

            case "get_users_info_by_param":
                return JsonSerializer.Serialize(await AccountHelper.Test2()); //ToDo

            case "get_all_short_info_by_id":
                return JsonSerializer.Serialize(await AccountHelper.GetShortInfoById());

            // Friends
            case "update_friends_info": 
                return JsonSerializer.Serialize(new Dictionary<string, string>()); //ToDo

            case "possible_friends_list": 
                return JsonSerializer.Serialize(new Dictionary<string, string>()); //ToDo

            case "time_in_match":
                await AccountHelper.UpdateInfo(form);
                return "ok";

            // Misc
            case "get_time": 
                return DateTime.UtcNow.ToFileTimeUtc().ToString();
        }

        return "fail";
    }
}
