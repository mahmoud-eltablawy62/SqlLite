using Microsoft.EntityFrameworkCore;
using syncData.Models;

namespace syncData
{
    public class UserContextSqlLite : DbContext
    {

        public UserContextSqlLite(DbContextOptions<UserContextSqlLite> options) : base(options) { }
        public DbSet<User> LocalUser { get; set; }

        

    }
}
