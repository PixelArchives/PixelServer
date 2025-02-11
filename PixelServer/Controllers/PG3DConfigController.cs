using Microsoft.AspNetCore.Mvc;
using PixelServer.Helpers;
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
    public async Task<JsonResult> FilterBadWord()
    {
        return new JsonResult(await BadWordHelper.GetOrCreate());
    }
}
