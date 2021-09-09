
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class PingController : ControllerBase
{
    private readonly PingRecorder _pingRecorder;

    public PingController(PingRecorder pingRecorder)
    {
        _pingRecorder = pingRecorder;
    }

    [HttpPost]
    public async Task<IActionResult> Ping()
    {
        // curl -X POST -I  -H 'Content-Type: application/json' -H 'Content-Length: 0' http://localhost:5000/ping
        await _pingRecorder.Ping();
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetLastPing()
    {
        return Ok(await _pingRecorder.GetLastPingDateTime());
    }
}