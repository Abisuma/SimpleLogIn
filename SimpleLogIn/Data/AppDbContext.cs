using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleLogIn.Models;

namespace SimpleLogIn.Data
{
    public class AppDbContext:IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<EmailModel> EmailModels { get; set; }
        //public DbSet<UserActivation> UserActivations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //seeding data into my db
            modelBuilder.Entity<EmailModel>().HasData
            (
               new EmailModel { Id = 1, FullName="Kadri olawale", UserEmail = "kadwales@gmail.com", Password = "123", ImageUrl= ""},
                new EmailModel { Id = 2, FullName = "John Jules", UserEmail = "wales@gmail.com", Password = "12365657", ImageUrl="" }
            );
        }
    }
}

