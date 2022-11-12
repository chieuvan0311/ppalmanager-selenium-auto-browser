using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace PAYPAL.DataConnection
{
    public partial class PaypalDbContext : DbContext
    {
        public PaypalDbContext()
            : base("name=PaypalDbContext")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Del_Account> Del_Account { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.AccPassword)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.EmailPassword)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Proxy)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.ProfileId)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Email_2FA)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.BankCard)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.AccOtherType)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.EmailType)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.AccCategory)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.SecondEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.AccPassword_Old)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.EmailPassword_Old)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Acc_2FA_Old)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Email_2FA_Old)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.RecoveryEmail_Old)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Questions_Old)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Random_AccPassword)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Random_EmailPassword)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Random_Questions)
                .IsUnicode(false);

            modelBuilder.Entity<Admin>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.AccPassword)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.EmailPassword)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.Proxy)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.ProfileId)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.Email_2FA)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.BankCard)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.AccOtherType)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.EmailType)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.AccCategory)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.SecondEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.AccPassword_Old)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.EmailPassword_Old)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.Acc_2FA_Old)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.Email_2FA_Old)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.RecoveryEmail_Old)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.Questions_Old)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.Random_AccPassword)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.Random_EmailPassword)
                .IsUnicode(false);

            modelBuilder.Entity<Del_Account>()
                .Property(e => e.Random_Questions)
                .IsUnicode(false);
        }
    }
}
