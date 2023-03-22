using Application.Common.Interfaces;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence.Interceptors;

/// <summary>
/// This intercept the response from ef(db framework of c#)
/// so you can view, modify and suppress the execution of SaveChanges and then change the result before ef returns it
/// </summary>
public class AuditableEntityInterceptor : SaveChangesInterceptor
{
 	private readonly ICurrentUserService _currentUserService;
 	private readonly IDateTime _dateTime;
 
 	public AuditableEntityInterceptor(
 		ICurrentUserService currentUserService,
 		IDateTime dateTime)
 	{
 		_currentUserService = currentUserService;
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
 
    /// <summary>
    /// When the execution get intercepted the result get changed here
    /// So in this context we just add who created it and when it got created when we update or add something to the db
    /// </summary>
    /// <param name="context"></param>
 	private void UpdateEntities(DbContext? context)
 	{
 		if (context is null)
 			return;
 
 		foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
 		{
 			if (entry.State is EntityState.Added)
 			{
 				entry.Entity.CreatedBy = _currentUserService.UserId.ToString();
 				entry.Entity.Created = _dateTime.Now;
 			}
 
 			if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
 			{
 				entry.Entity.LastModifiedBy = _currentUserService.UserId.ToString();
 				entry.Entity.LastModified = _dateTime.Now;
 			}
 		}
 	}
 }
 
 public static class Extensions
 {
 	public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
 		entry.References.Any(referenceEntry =>
 			referenceEntry.TargetEntry != null &&
 			referenceEntry.TargetEntry.Metadata.IsOwned() &&
 			referenceEntry.TargetEntry.State is EntityState.Added or EntityState.Modified);
 }
