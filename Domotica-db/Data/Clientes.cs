using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domotica_db.Data.CustomIdentity;

namespace Domotica_db.Data
{
    public class Clientes
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        //lo he puesto en mayusculas el DNI por convención 
        public string NIF { get; set; }
        /* Esta sera la cadena donde esta almacenada la imagen dentro de nuestra propia aplicación, esto conlleva que si 
         * borro la imagen, tengo que dejar la cadena esta vacia
         */
        public string Imagen { get; set; }
        //campo explicando si es buen cliente o no para la empresa
        public string EsBueno { get; set; }
        public string Direccion { get; set; }
        //telefono fijo del cliente
        public string Telefono { get; set; }
        //telefono movil del cliente
        public string Movil { get; set; }
        public string SitioWeb { get; set; }
        public string Idioma { get; set; }
        
        //foreign key de la tabla aspnetusers
        
    }
}
