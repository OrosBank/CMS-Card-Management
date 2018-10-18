using Cards.Models.ViewModels;
using Cards.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Cards.DatabaseLink
{
    public class ApplicationUserLogin : IdentityUserLogin<int> { }
    public class ApplicationUserClaim : IdentityUserClaim<int> { }
    public class ApplicationUserRole : IdentityUserRole<int> { }
    public class ApplicationUser : IdentityUser<int>
    {
        public string StaffId { get; set; }
        
        public string Name { get; set; }
        public int BranchId { get; set; }
    }
    public class ApplicationRole : IdentityRole<int>
    {
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string IPAddress { get; set; }

        public ApplicationRole() : base() { }
        public ApplicationRole(string name)
            : this()
        {
            this.Name = name;
        }

        public ApplicationRole(string name, string description)
            : this(name)
        {
            this.Description = description;
        }
    }


    public class ApplicationRoleClaim : IdentityRoleClaim<int> { }
    public class ApplicationUserToken : IdentityUserToken<int> { }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserClaim, ApplicationUserRole,
    ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        public ApplicationDbContext(string v)
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CardIssuance> CardIssuances { get; set; }
        public DbSet<CardMode> CardModes { get; set; }
        public DbSet<CardProduct> CardProducts { get; set; }
        public DbSet<CardType> CardTypes { get; set; }
        public DbSet<CardStatus> CardStatuses { get; set; }
        public DbSet<FileType> FileType { get; set; }
        public DbSet<Pin> Pin { get; set; }
        public DbSet<PinStatus> PinStatus { get; set; }
        public DbSet<TranCharges> TranCharges { get; set; }
        public DbSet<AccountDetail> AccountDetails { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<PanDetails> PanDetails { get; set; }
        public DbSet<PanUpload> PanUploads { get; set; }
        public DbSet<PanUploadDetails> PanUploadDetails { get; set; }
        public DbSet<ErrorLog> errorLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<ApplicationRole>().ToTable("Roles");
            builder.Entity<ApplicationUserRole>().ToTable("UserRoles");
            builder.Entity<ApplicationUserClaim>().ToTable("UserClaims");
            builder.Entity<ApplicationUserLogin>().ToTable("UserLogins");
            builder.Entity<ApplicationRoleClaim>().ToTable("RoleClaims");
            builder.Entity<ApplicationUserToken>().ToTable("UserToken");

            builder.Entity<ApplicationUser>(b =>
            {
                b.Property(u => u.Email).HasColumnName("EmailAddress");
            });
            
        }
    }
}
