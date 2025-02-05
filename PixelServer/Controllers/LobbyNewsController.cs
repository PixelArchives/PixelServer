using Microsoft.AspNetCore.Mvc;
using PixelServer.Objects;

namespace PixelServer.Controllers;

[ApiController]
[Route($"{Settings.mainRoute}{Settings.pg3dConfig}/lobbyNews")]
public class LobbyNewsController
{
    public JsonResult Default()
    {
        List<LobbyNews> result = new();

        return new JsonResult(result);
    }

    [HttpGet]
    [Route("LobbyNews_test.json")]
    public JsonResult Test()
    {
        return Default();
    }

    [HttpGet]
    [Route("LobbyNews_ios.json")]
    public JsonResult IPhone()
    {
        return Default();
    }

    [HttpGet]
    [Route("LobbyNews_androd.json")]
    public JsonResult Android()
    {
        return Default();
    }

    [HttpGet]
    [Route("LobbyNews_amazon.json")]
    public JsonResult Amazon()
    {
        return Default();
    }

    [HttpGet]
    [Route("LobbyNews_wp.json")]
    public JsonResult WindowsPhone()
    {
        return Default();
    }
}
