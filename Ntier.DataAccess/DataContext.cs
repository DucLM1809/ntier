using Microsoft.EntityFrameworkCore;
using Ntier.Shared.Models;

namespace Ntier.DataAccess;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
}
