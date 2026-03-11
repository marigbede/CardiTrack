using CardiTrack.Application.Interfaces.Repositories;
using CardiTrack.Domain.Entities;
using CardiTrack.Domain.Enums;
using CardiTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CardiTrack.Infrastructure.Repositories;

public class DeviceRepository : Repository<Device>, IDeviceRepository
{
    public DeviceRepository(CardiTrackDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Device>> GetActiveDevicesAsync()
        => await _dbSet.Where(d => d.IsActive).OrderBy(d => d.SortOrder).ToListAsync();

    public async Task<Device?> GetByDeviceTypeAsync(DeviceType deviceType)
        => await _dbSet.FirstOrDefaultAsync(d => d.DeviceType == deviceType && d.IsActive);
}
