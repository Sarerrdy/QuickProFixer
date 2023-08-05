using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuickProFixer.Models.Entities;
using QuickProFixer.Models.UtilityModels;

namespace QuickProFixer.Models.Context
{
    public class QuickProFixerContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public QuickProFixerContext(DbContextOptions<QuickProFixerContext> options) : base(options)
        {
           
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<LGA> LGAs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FixerProfile> FixerProfiles { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ServiceType> Category { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<SentNotification> sentNotifications { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<PriceRateType>  priceRateTypes { get; set; }
        public DbSet<NotificationResponseStatus> NotificationResponseStatuses { get; set; }
        public DbSet<Message> Messages { get; set; }
      //  public DbSet<NotificationReciever> NotificationRecievers { get; set; }
    }
}
