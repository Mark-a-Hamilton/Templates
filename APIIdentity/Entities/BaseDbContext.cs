namespace APIIdentity.Entities;

public abstract class BaseDbContext : IdentityDbContext<User>
{
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BaseDbContext(DbContextOptions<DataContext> options, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        var currentUser = _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedOn = DateTime.UtcNow;
                entry.Entity.CreatedBy = currentUser?.UserName ?? "System"; // Replace with actual user
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedOn = DateTime.UtcNow;
                entry.Entity.ModifiedBy = currentUser?.UserName ?? "System"; // Replace with actual user
            }
        }

        return base.SaveChanges();
    }
}
