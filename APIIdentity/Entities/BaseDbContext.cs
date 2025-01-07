namespace APIIdentity.Entities
{
    public abstract class BaseDbContext : DbContext
    {
        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    entry.Entity.CreatedBy = "System"; // Replace with actual user
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedOn = DateTime.UtcNow;
                    entry.Entity.ModifiedBy = "System"; // Replace with actual user
                }
            }

            return base.SaveChanges();
        }
    }
}