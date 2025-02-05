using Microsoft.AspNetCore.Mvc;
using PixelServer.Objects;

namespace PixelServer.Controllers;

[ApiController]
[Route($"{Settings.mainRoute}{Settings.pg3dConfig}")]
public class PG3DConfigController
{
    [Route("getBanList.php")]
    public int GetBanList([FromForm] ActionForm form)
    {
        return -1; // -1 dev, 0 not banned, 1 BAN THIS RETARD
    }

    [Route("FilterBadWord.json")]
    [HttpGet]
    public JsonResult FilterBadWord()
    {
        BadWordContainer result = new();

        return new JsonResult(result);
    }
}
