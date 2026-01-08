using CardiTrack.Infrastructure.Persistence;
using CardiTrack.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace CardiTrack.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CardiTrackDbContext _context;
    private IDbContextTransaction? _transaction;

    public IOrganizationRepository Organizations { get; }
    public IUserRepository Users { get; }
    public ICardiMemberRepository CardiMembers { get; }
    public ISubscriptionRepository Subscriptions { get; }
    public IUserCardiMemberRepository UserCardiMembers { get; }

    public UnitOfWork(
        CardiTrackDbContext context,
        IOrganizationRepository organizations,
        IUserRepository users,
        ICardiMemberRepository cardiMembers,
        ISubscriptionRepository subscriptions,
        IUserCardiMemberRepository userCardiMembers)
    {
        _context = context;
        Organizations = organizations;
        Users = users;
        CardiMembers = cardiMembers;
        Subscriptions = subscriptions;
        UserCardiMembers = userCardiMembers;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
