using BusinessCardManagerAPI.Data.Models.Contract;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace BusinessCardManagerAPI.Data.Interceptors
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null)
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                //if (entry is null || entry.State != EntityState.Deleted ||! (entry.Entity is ISoftDeleteable))
                if (entry is not { State: EntityState.Deleted, Entity: ISoftDeletable entity })
                    continue;
                entry.State = EntityState.Modified;

                entity.Delete();
            }
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            if (eventData.Context is null)
                return base.SavedChanges(eventData, result);

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                //if (entry is null || entry.State != EntityState.Deleted ||! (entry.Entity is ISoftDeleteable))
                if (entry is not { State: EntityState.Deleted, Entity: ISoftDeletable entity })
                    continue;
                entry.State = EntityState.Modified;

                entity.Delete();
            }
            return base.SavedChanges(eventData, result);
        }
    }
}
