using Microsoft.AspNetCore.Mvc;
using PixelServer.Helpers;
using PixelServer.Objects;

namespace PixelServer.Controllers;

[ApiController]
[Route(Settings.mainRoute)]
public class MainController
{
    [Route("action.php")]
    public async Task<string> Action([FromForm] ActionForm form)
    {
        await Task.Run(() => { });

        DebugHelper.Log(form);

        switch (form.action)
        {
            case "check_version":
                return VersionHelper.IsValid(form.app_version) ? "valid" : "no";

            case "check_shop_version":
                return VersionHelper.IsValid(form.app_version) ? "valid" : "no";

            case "create_player_intent":
                return await AccountHelper.CreateAccountToken();

            case "create_player":
                long id = await AccountHelper.CreateAccount(form.token);
                return id.ToString();

            case "start_check": 
                return await AccountHelper.AccountExists(form.uniq_id) ? "exists" : "fail";
        }

        return "fail";
    }
}
