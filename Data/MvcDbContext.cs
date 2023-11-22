using Microsoft.EntityFrameworkCore;
using MVCCRUD.Models.Domain;

namespace MVCCRUD.Data
{
    public class MvcDbContext : DbContext
    {
        public MvcDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
    }
}
