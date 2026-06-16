using Application.Clients;
using Application.Clients.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientsController(IClientService clientService) : ControllerBase
{
    [HttpPost]
    [Route("individual")]
    public async Task<IActionResult> RegisterIndividualClient([FromBody] CreateIndividualClientDto dto, CancellationToken cancellationToken)
    {
        await clientService.RegisterIndividualClientAsync(dto, cancellationToken);
        return Created();
    }

    [HttpPost]
    [Route("company")]
    public async Task<IActionResult> RegisterCompanyClient([FromBody] CreateCompanyClientDto dto,
        CancellationToken cancellationToken)
    {
        await clientService.RegisterCompanyClientAsync(dto, cancellationToken);
        return Created();
    }

    [HttpPut]
    [Route("individual/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateIndividualClient(int id, [FromBody] UpdateIndividualClientDto dto,
        CancellationToken cancellationToken)
    {
        await clientService.UpdateIndividualClientAsync(id, dto, cancellationToken);
        return NoContent();
    }
    
    [HttpPut]
    [Route("company/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateIndividualClient(int id, [FromBody] UpdateCompanyClientDto dto,
        CancellationToken cancellationToken)
    {
        await clientService.UpdateCompanyClientAsync(id, dto, cancellationToken);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteClient(int id, CancellationToken cancellationToken)
    {
        await clientService.DeleteClientAsync(id, cancellationToken);
        return Ok();
    }

}