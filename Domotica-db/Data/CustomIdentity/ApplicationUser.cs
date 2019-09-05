using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



namespace Domotica_db.Data.CustomIdentity
{
    //tabla AspNetRoles
    public class ApplicationRole : IdentityRole<int>
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
    }
    //tabla AspNetUserRoles
    public class ApplicationRoleClaim : IdentityRoleClaim<int>
    {
        public virtual ApplicationRole Role { get; set; }
    }
    //esta tabla esta mal
    public class ApplicationUser : IdentityUser<int>
    {
        //public override int Id { get; set; }
        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        //campo que se verá en la tabla Usuarios como UsuariosID o algo asi
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        //lo he puesto en mayusculas el DNI por convención 
        public string NIF { get; set; }
        /* Esta sera la cadena donde esta almacenada la imagen dentro de nuestra propia aplicación, esto conlleva que si 
         * borro la imagen, tengo que dejar la cadena esta vacia
         */
        public string Imagen { get; set; }
        //public virtual ICollection<Usuarios> Usuarios { get; set; }
    }
    
    //tabla AspNetUsersRole
    public class ApplicationUserRole : IdentityUserRole<int>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
    //AspNetUserClaims para decirme que el id estan 
    public class ApplicationUserClaim : IdentityUserClaim<int>
    {
        public virtual ApplicationUser User { get; set; }
    }

    public class ApplicationUserLogin : IdentityUserLogin<int>
    {
        public virtual ApplicationUser User { get; set; }
    }

    public class ApplicationUserToken : IdentityUserToken<int>
    {
        public virtual ApplicationUser User { get; set; }
    }

} //final del namepace Domotica_db.Data.CustomIdentity
