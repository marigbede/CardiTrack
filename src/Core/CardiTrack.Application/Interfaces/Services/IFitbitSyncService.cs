using CardiTrack.Domain.Entities;

namespace CardiTrack.Application.Interfaces.Services;

public interface IFitbitSyncService
{
    Task SyncCardiMemberAsync(DeviceConnection connection);
}
