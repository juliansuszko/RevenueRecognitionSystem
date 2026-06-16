using Application.Contracts.DTOs;

namespace Application.Contracts;

public interface IContractService
{
    Task CreateContractAsync(CreateContractDto dto, CancellationToken cancellationToken);
}