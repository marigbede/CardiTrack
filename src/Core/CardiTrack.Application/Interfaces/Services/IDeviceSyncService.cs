using CardiTrack.Domain.Entities;

namespace CardiTrack.Application.Interfaces.Services;

public interface IDeviceSyncService
{
    Task SyncCardiMemberAsync(DeviceConnection connection);
}
