using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using syncData.Models;

namespace syncData
{
    

        public class userContext : DbContext
        {
  

        public userContext(DbContextOptions<userContext> options) : base(options) { }
            public DbSet<User> User { get; set; }

        }

    }

