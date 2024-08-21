using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Domain.Models;

public class LogContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<DmnLog> Log { get; set; }
}
