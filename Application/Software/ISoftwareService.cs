using Application.Software.DTOs;

namespace Application.Software;

public interface ISoftwareService
{
    Task AddSoftwareAsync(CreateSoftwareDto dto, CancellationToken cancellationToken);
}