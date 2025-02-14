using Microsoft.AspNetCore.Mvc;
using PixelServer.Helpers;
using PixelServer.Objects;

namespace PixelServer.Controllers;

[ApiController]
[Route($"{Settings.mainRoute}{Settings.pg3dConfig}")]
public class PG3DConfigController
{
    [Route("getBanList.php")]
    public async Task<int> GetBanList([FromForm] BanListForm form)
    {
        return await AccountHelper.IsBanned(form.id);
    }

    [Route("FilterBadWord.json")]
    [HttpGet]
    public async Task<JsonResult> FilterBadWord()
    {
        return new JsonResult(await BadWordHelper.GetOrCreate());
    }
}
