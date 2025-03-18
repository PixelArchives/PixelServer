using Microsoft.AspNetCore.Mvc;
using PixelServer.Helpers;
using PixelServer.Objects;
using System.Text.Json;

namespace PixelServer.Controllers;

[ApiController]
[Route(Settings.mainRoute)]
public class MainController
{
    const string fail = "fail";

    [Route("action.php")]
    public async Task<string> Action([FromForm] ActionForm form)
    {
        //return await FriendsHelper.UpdateFriendsInfo(7); //for debugging requests trough browser

        if (string.IsNullOrWhiteSpace(form.action)) return fail;

        // IN DEVELOPMENT
        //if (!Settings.excludeActionsFormHashing.Contains(form.action) && !HashHelper.IsValid(form)) return "fail";

#if DEBUG
        DebugHelper.Log(form);
#endif

        switch (form.action)
        {
            case "check_version": // Leave it like that.
            case "check_shop_version": return VersionHelper.IsValid(form.app_version) ? "yes" : "no";

            // Account creating and etc
            case "create_player_intent": return await AccountHelper.CreateAccountToken();

            case "create_player":
                long id = await AccountHelper.CreateAccount(form.token);
                return id.ToString();

#region Account info getting
            case "start_check":
                return await AccountHelper.GetOrCreate(form.uniq_id);

            case "get_info_by_id":
                //ToDo - breaks profile, when using JsonSerializer.Serialize breaks other player's profiles
                PlayerData? data = await AccountHelper.GetInfoById(form.uniq_id);
                if (data == null) return fail;
                return JSONHelper.GetInfoByIDSerializer(data);

            case "update_player": await AccountHelper.UpdateInfo(form); return string.Empty;

            case "get_users_info_by_param":
                return JSONHelper.GetInfoByParamSerializer(await AccountHelper.GetByParam(form.param));

            case "get_all_short_info_by_id":
                if (string.IsNullOrEmpty(form.ids)) return fail;
                return JSONHelper.WrapPlayerDataToIdList(await AccountHelper.GetShortInfoByIds(form.ids));
            #endregion

            #region friends
            case "update_friends_info": 
                return await FriendsHelper.UpdateFriendsInfo(form.uniq_id); //ToDo

            case "possible_friends_list":
                return fail; //ToDo
                //return JsonSerializer.Serialize(new Dictionary<string, string>()); //ToDo

            case "friend_request":
                return await FriendsHelper.TrySendFriendRequest(form.id, form.whom) ? "ok" : "fail";
#endregion
            case "time_in_match":
                //await AccountHelper.UpdateInfo(form);
                return fail; // ToDo

            // Misc
            case "get_time": return DateTime.UtcNow.ToFileTimeUtc().ToString();
        }

        return fail;
    }
}
