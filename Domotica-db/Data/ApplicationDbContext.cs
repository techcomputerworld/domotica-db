using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domotica_db.Data.CustomIdentity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Domotica_db.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserClaim,
        ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        public ApplicationDbContext()
        {
            //AddData();
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //AddData();
        }
        
        private async void AddData()
        {
            /* Esto se añade al inicio del programa pasara por aquí y se añaden lo que imagino que si no añado aquí datos,
             * no dara problemas por ser los mismos datos
             * Este código que estoy añadiendo es para añadir esos datos a esa tabla en cuestión de nuestra propia base
             * de datos y poder utilizar estos roles dentro de los roles de usuario. 
             */
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                //comprobación de que no existan los datos
                List<ApplicationRole> apRoles = await context.Roles.ToListAsync();
             
                ApplicationRole RoleSA = new ApplicationRole() { Name = "SuperAdmin", NormalizedName = "SuperAdmin" };
                context.Add(RoleSA);
                bool RoleSuperAdmin = apRoles.Contains(RoleSA);
                ApplicationRole RoleA = new ApplicationRole() { Name = "Admin", NormalizedName = "Admin" };
                context.Add(RoleA);
                bool RoleAdmin = apRoles.Contains(RoleA);
                ApplicationRole RoleE = new ApplicationRole() { Name = "Edit", NormalizedName = "Edit" };
                context.Add(RoleE);
                bool RoleEdit = apRoles.Contains(RoleE);
                ApplicationRole RoleU = new ApplicationRole() { Name = "User", NormalizedName = "User" };
                context.Add(RoleU);
                bool RoleUser = apRoles.Contains(RoleU);
                if (RoleSuperAdmin == false && RoleAdmin == false && RoleEdit == false && RoleUser == false)
                {
                    context.SaveChanges();
                }
                else
                {
                    context.Dispose();
                }
                
            }
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // código para solucionar el problema convirtiendo el valor a int16 en vez a bool. o poner un 0 o un 1, 
            // 0 false, 1 true.
            builder.Entity<ApplicationUser>()
                .Property(r => r.EmailConfirmed)
                .HasConversion(new BoolToZeroOneConverter<Int16>());
            builder.Entity<ApplicationUser>()
                .Property(r => r.PhoneNumberConfirmed)
                .HasConversion(new BoolToZeroOneConverter<Int16>());
            builder.Entity<ApplicationUser>()
                .Property(r => r.TwoFactorEnabled)
                .HasConversion(new BoolToZeroOneConverter<Int16>());
            builder.Entity<ApplicationUser>()
                .Property(r => r.LockoutEnabled)
                .HasConversion(new BoolToZeroOneConverter<Int16>());

            builder.Entity<ApplicationUser>(b =>
            {
                //b.HasKey(e => e.Id);
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
                
                    
            });

            builder.Entity<ApplicationRole>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });
            //añadir datos a ApplicationRole tabla ""
            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole() { Id = 1, Name = "SuperAdmin", NormalizedName = "SuperAdmin", ConcurrencyStamp = "" },
                new ApplicationRole() { Id = 2, Name = "Admin", NormalizedName = "Admin" , ConcurrencyStamp = "" },
                new ApplicationRole() { Id = 3, Name = "Edit", NormalizedName = "Edit", ConcurrencyStamp = "" },
                new ApplicationRole() { Id = 4, Name = "User", NormalizedName = "User", ConcurrencyStamp = "" }
            );
            //código añadido para ver si se soluciona el problema de registro de usuarios

        }

        /*  esto es para tener la clase usuarios como la tengo aquí, lógicamente de la forma que hemos modificado Identity tendre que poner una
            relación entre AspNetUsers y Usuarios simplemente eso pero eso ya me busco yo la vida para ver como se hace y aprender
        */
        public DbSet<Clientes> Clientes { get; set; }
        

        //public DbQuery<ApplicationUser> QueryUsers { get; set; }

    }
}
