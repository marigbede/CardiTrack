using CardiTrack.Domain.Entities;
using CardiTrack.Domain.Enums;

namespace CardiTrack.Application.Interfaces.Repositories;

public interface IDeviceRepository : IRepository<Device>
{
    Task<IEnumerable<Device>> GetActiveDevicesAsync();
    Task<Device?> GetByDeviceTypeAsync(DeviceType deviceType);
}
