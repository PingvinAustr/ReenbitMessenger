using Microsoft.EntityFrameworkCore;

namespace ReenbitMessenger.DataAccess
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext>
    options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersChats>().HasKey(sc => new { sc.UserId, sc.ChatId });
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Chats> Chats { get; set; }
        public DbSet<UsersChats> UsersChats { get; set; }
        public DbSet<Messages> Messages { get; set; }   

       
    }
}
