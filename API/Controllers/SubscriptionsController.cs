using Application.Subscription;
using Application.Subscription.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class SubscriptionsController(ISubscriptionService service) : ControllerBase
{
    [HttpPost]
    [Route("create-subscription")]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionDto dto,
        CancellationToken cancellationToken)
    {
        await service.CreateSubscriptionAsync(dto, cancellationToken);
        return Created();
    }
}