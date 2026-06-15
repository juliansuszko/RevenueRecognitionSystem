using Application.Clients.DTOs;

namespace Application.Clients;

public interface IClientService
{
    Task RegisterIndividualClientAsync(CreateIndividualClientDto dto, CancellationToken cancellationToken);
    Task RegisterCompanyClientAsync(CreateCompanyClientDto dto, CancellationToken cancellationToken);
    Task UpdateIndividualClientAsync(int id, UpdateIndividualClientDto dto, CancellationToken cancellationToken);
    Task UpdateCompanyClientAsync(int id, UpdateCompanyClientDto clientDto, CancellationToken cancellationToken);
    Task DeleteClientAsync(int id, CancellationToken cancellationToken);
}