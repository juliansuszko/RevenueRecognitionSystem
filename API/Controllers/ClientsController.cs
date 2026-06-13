using Application.Clients;
using Application.Clients.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController(IClientService clientService) : ControllerBase
{
    [HttpPost]
    [Route("/individual")]
    public async Task<IActionResult> RegisterIndividualClient([FromBody] CreateIndividualClientDto dto, CancellationToken cancellationToken)
    {
        await clientService.RegisterIndividualClientAsync(dto, cancellationToken);
        return Created();
    }

    [HttpPost]
    [Route("/company")]
    public async Task<IActionResult> RegisterCompanyClient([FromBody] CreateCompanyClientDto dto,
        CancellationToken cancellationToken)
    {
        await clientService.RegisterCompanyClientAsync(dto, cancellationToken);
        return Created();
    }

}