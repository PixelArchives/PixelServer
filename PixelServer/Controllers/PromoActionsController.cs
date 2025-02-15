using Microsoft.AspNetCore.Mvc;
using PixelServer.Objects;

namespace PixelServer.Controllers;

[ApiController]
[Route($"{Settings.mainRoute}{Settings.pg3dConfig}/PromoActions")]
public class PromoActionsController
{

    [HttpGet]
    [Route("promo_actions.json")]
    public JsonResult Default()
    {
        PromoActions actions = new();

        return new JsonResult(actions);
    }

    [HttpGet]
    [Route("promo_actions_android.json")]
    public JsonResult Android()
    {
        return Default();
    }

    [HttpGet]
    [Route("promo_actions_amazon.json")]
    public JsonResult Amazon()
    {
        return Default();
    }

    [HttpGet]
    [Route("promo_actions_wp8.json")]
    public JsonResult WindowsPhone()
    {
        return Default();
    }

    [HttpGet]
    [Route("promo_actions_test.json")]
    public JsonResult Test()
    {
        return Default();
    }
}
