using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Azure.DatabaseContext
{
    public class AzureDatabaseContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public static IList<Employee> GetEmployees()
        {
            using (AzureDatabaseContext dbContext = new AzureDatabaseContext())
            {
                return dbContext.Employees.ToList();
            }
        }
    }
}