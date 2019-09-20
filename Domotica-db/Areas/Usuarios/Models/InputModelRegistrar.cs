using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domotica_db.Areas.Usuarios.Models
{
    public class InputModelRegistrar
    {
        [Required(ErrorMessage = "<font color='red'> El campo nombre es obligatorio </font>")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "<font color='red'> El campo apellido es obligatorio </font>")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "<font color='red'> El campo DNI es obligatorio </font>")]
        public string NIF { get; set; }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        //este campo es para cogerlo de la tabla usuarios y ponerlo aqui con la propiedad set
        public string Imagen { get; set; }
        public string Email { get; set; }
        //Role es la lista de selección de roles
        public string Role { get; set; }
        public string RoleUser { get; set; }
        public int ApplicationUserId { get; set; }
        /*
        [Required(ErrorMessage = "<font color='red'> El campo de contraseña es obligatorio. </font>")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{6})$", ErrorMessage = "<font color='red'> El " +
            "formato telefono ingresado no es válido. </font>")]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }
        */
        /*
        [Required(ErrorMessage = "<font color='red'> El campo correo electrónico es obligatorio. </font>")]
        [EmailAddress(ErrorMessage = "<font color='red'> El campo de correo electrónico no es una dirección de correo " +
            "electrónico válida.</font> ")]
        //[DataType(DataType.PhoneNumber)]
        public string Email { get; set; }
        */
        /*
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "<font color='red'> El campo contraseña es obligatorio. </font>")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "<font color='red'> El numero de caracteres de {0} debe ser " +
            "al menos {2} ", MinimumLength = 6)]
        public string Password { get; set; }
        */
    }
}
