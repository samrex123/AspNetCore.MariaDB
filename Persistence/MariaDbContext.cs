using AspNetCore.MariaDB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.MariaDB.Persistence
{
    public partial class MariaDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public MariaDbContext(DbContextOptions<MariaDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Comment> Comments{ get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public DbSet<AspNetCore.MariaDB.Models.Discussion> Discussion { get; set; }
    }
}
