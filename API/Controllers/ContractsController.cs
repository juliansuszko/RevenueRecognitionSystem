using Application.Contracts;
using Application.Contracts.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ContractsController(IContractService contractService) : ControllerBase
{
    [HttpPost]
    [Route("/create-Contract")]
    public async Task<IActionResult> CreateContract([FromBody] CreateContractDto dto,
        CancellationToken cancellationToken)
    {
        await contractService.CreateContractAsync(dto, cancellationToken);
        return Created();
    }
}