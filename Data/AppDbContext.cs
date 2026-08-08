using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Data
{
    //IdentityDbContext<ApplicationUser> → gives you the tables: AspNetUsers, AspNetRoles, AspNetUserClaims, AspNetUserRoles, etc.It's pure schema, inherited automatically so you don't have to define those tables yourself.
    public class ApplicationUser : IdentityUser { }

    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
    }

    public class Transaction
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}