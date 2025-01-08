namespace APIIdentity.Entities;

public class DataContext : BaseDbContext
{
    //public DbSet<YourEntity> TableName { get; set; }
    #region Leaf / Data Tables - No References
    #endregion

    #region Branch / Transaction Tables - With References
    #endregion
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
}
