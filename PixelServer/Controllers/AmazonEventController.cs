using Microsoft.AspNetCore.Mvc;

namespace PixelServer.Controllers;

[ApiController]
[Route($"{Settings.mainRoute}{Settings.pg3dConfig}/amazonEvent")]
public class AmazonEventController
{
    [Route("amazon-event.json")]
    public string Default()
    {
        return string.Empty;
    }

    [Route("amazon-event-test.json")]
    public string Test()
    {
        return string.Empty;
    }
}
