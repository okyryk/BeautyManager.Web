using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using BeautyManager.Web.Data;

namespace BeautyManager.Web.Api
{
    public class BeautyManagerDbContext : DbContext
    {
        public BeautyManagerDbContext(DbContextOptions<BeautyManagerDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>().OwnsOne(c => c.Address);
            //modelBuilder.Entity<User>().HasMany<AuctionInfo>(c => c.Auctions);
        }
    }
}
