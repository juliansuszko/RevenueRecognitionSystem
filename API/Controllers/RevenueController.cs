using Application.Revenue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RevenueController(IRevenueService revenueService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTotalRevenueAsync([FromQuery] string? currencyCode,
        CancellationToken cancellationToken = default)
    {
        var result = await revenueService.GetTotalRevenueAsync(currencyCode, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [Route("{softwareId:int}")]
    public async Task<IActionResult> GetTotalRevenueBySoftwareIdAsync(int softwareId, [FromQuery] string? currencyCode,
        CancellationToken cancellationToken)
    {
        var result = await revenueService.GetSoftwareRevenueAsync(softwareId, currencyCode, cancellationToken);
        return Ok(result);
    }
}