using Application.Payments;
using Application.Payments.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController(IPaymentService service) : ControllerBase
{
    [HttpPost]
    [Route("/contract")]
    public async Task<IActionResult> CreateContractPayment([FromBody] CreateContractPaymentDto dto,
        CancellationToken cancellationToken)
    {
        await service.CreateContractPaymentAsync(dto, cancellationToken);
        return Created();
    }

    [HttpPost]
    [Route("/subcription")]
    public async Task<IActionResult> CreateSubscriptionPayment([FromBody] CreateSubscriptionPaymentDto dto, CancellationToken cancellationToken)
    {
        await service.CreateSubscriptionPaymentAsync(dto, cancellationToken);
        return Created();
    }
}