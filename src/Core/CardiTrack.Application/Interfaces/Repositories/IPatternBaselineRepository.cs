using CardiTrack.Domain.Entities;

namespace CardiTrack.Application.Interfaces.Repositories;

public interface IPatternBaselineRepository : IRepository<PatternBaseline>
{
    Task<PatternBaseline?> GetLatestByCardiMemberAsync(Guid cardiMemberId, int periodDays);
}
