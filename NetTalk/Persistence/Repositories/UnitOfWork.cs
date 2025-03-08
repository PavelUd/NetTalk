using System.Data;
using Application.Interfaces.Repositories;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly NetTalkDbContext _context;
    private readonly IMediator _mediator;
    private readonly ILogger<UnitOfWork> _logger;
    private ChatRepository? _chatRepository;
    private UserRepository? _userRepository;
    private readonly IEventStoreRepository _eventStoreRepository;

    public UnitOfWork(NetTalkDbContext context, IMediator mediator,IEventStoreRepository eventStoreRepository, ILogger<UnitOfWork> logger)
    {
        _context = context;
        _mediator = mediator;
        _eventStoreRepository = eventStoreRepository;
        _logger = logger;
    }

    public async Task SaveChangesAsync()
    {
        var strategy = _context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            _logger.LogInformation("----- Begin transaction: '{TransactionId}'", transaction.TransactionId);
            try
            {
                var (domainEvents, eventStores) = BeforeSaveChanges();
                var rowsAffected = await _context.SaveChangesAsync();

                _logger.LogInformation("----- Commit transaction: '{TransactionId}'", transaction.TransactionId);
                
                await AfterSaveChangesAsync(domainEvents, eventStores);
                await transaction.CommitAsync();

                _logger.LogInformation(
                   "----- Transaction successfully confirmed: '{TransactionId}', Rows Affected: {RowsAffected}",
                  transaction.TransactionId,
                   rowsAffected);
            }
            catch (Exception ex)
            {
              _logger.LogError(
                    ex,
                    "An unexpected exception occurred while committing the transaction: '{TransactionId}', message: {Message}",
                    transaction.TransactionId,
                    ex.Message);

                await transaction.RollbackAsync();

                throw;
            }
        });
    }
    
    public void Commit()
    {
        var result = _context.SaveChanges();
    }

    public void Rollback()
    {
        _context.RollbackTransaction();
    }


    private (IReadOnlyList<BaseEvent> domainEvents, IReadOnlyList<EventStore> eventStores) BeforeSaveChanges()
    {
        var entities = _context.ChangeTracker.Entries<BaseEntity>()
            .Where(entry => entry.Entity.DomainEvents.Any())
            .ToList();;
        
        var domainEvents = entities.SelectMany(x => x.Entity.DomainEvents)
            .ToList();
        var eventStores = domainEvents
            .ConvertAll(@event => new EventStore(@event.AggregateId, @event.GetType().ToString(), JsonConvert.SerializeObject(@event.AggregateId)));
        
        entities.ForEach(entry => entry.Entity.ClearDomainEvents());
        return (domainEvents, eventStores) ;
        
    }
    
    private async Task AfterSaveChangesAsync(
        IReadOnlyList<BaseEvent> domainEvents, IReadOnlyList<EventStore> eventStores)
    {
        if (!domainEvents.Any())
            return;
        if (eventStores.Count > 0)
            await _eventStoreRepository.StoreAsync(eventStores);
        
        await Task.WhenAll(domainEvents.Select(@event => _mediator.Publish(@event)));
    }
    
    
    private bool _disposed;
    
    ~UnitOfWork() => Dispose(false);
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    private void Dispose(bool disposing)
    {
        if (_disposed)
            return;
        
        if (disposing)
        {
            _context.Dispose();
        }

        _disposed = true;
    }
}