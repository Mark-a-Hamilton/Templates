namespace API.Database;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    // public DbSet<ModelName> TableName { get; set; }
    #region Leaf / Data Tables - No References
    #endregion

    #region Branch / Transaction Tables - With References
    #endregion
}
