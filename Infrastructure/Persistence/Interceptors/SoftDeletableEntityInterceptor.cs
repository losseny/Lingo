using Application.Common.Interfaces;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence.Interceptors;
/// <summary>
/// This is the same as <see cref="AuditableEntityInterceptor"/>
/// but this one is for info related to deletions of entities in the database
/// </summary>
public class SoftDeletableEntityInterceptor : SaveChangesInterceptor
{
    private readonly IDateTime _dateTime;

    public SoftDeletableEntityInterceptor(IDateTime dateTime)
    {
        _dateTime = dateTime;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context is null)
            return;

        foreach (var entry in context.ChangeTracker.Entries<BaseSoftDeletableEntity>())
        {
            if (entry.State is EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.CurrentValues[nameof(BaseSoftDeletableEntity.DeletedAt)] = _dateTime.Now;
            }
        }
    }
}