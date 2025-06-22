using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thenestle.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Thenestle.Persistence.Data.Configurations;
using System.ComponentModel;

namespace Thenestle.Persistence.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        // Таблицы
        public DbSet<Mood> Moods { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Couple> Couples { get; set; }
        public DbSet<Invite> Invites { get; set; }
        public DbSet<MoodEntry> MoodEntries { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<ReminderNotification> ReminderNotifications { get; set; }
        public DbSet<FoodOrder> FoodOrders { get; set; }
        public DbSet<CurrencyTransaction> CurrencyTransactions { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Применение конфигураций
            modelBuilder.ApplyConfiguration(new MoodConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CoupleConfiguration());
            modelBuilder.ApplyConfiguration(new InviteConfiguration());
            modelBuilder.ApplyConfiguration(new MoodEntryConfiguration());
            modelBuilder.ApplyConfiguration(new ReminderConfiguration());
            modelBuilder.ApplyConfiguration(new ReminderNotificationConfiguration());
            modelBuilder.ApplyConfiguration(new FoodOrderConfiguration());
            modelBuilder.ApplyConfiguration(new CurrencyTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new PlaceConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        }
    }
}
