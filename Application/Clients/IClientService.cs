using Application.Clients.DTOs;

namespace Application.Clients;

public interface IClientService
{
    Task RegisterIndividualClientAsync(CreateIndividualClientDto dto, CancellationToken cancellationToken);
    Task RegisterCompanyClientAsync(CreateCompanyClientDto dto, CancellationToken cancellationToken);
}