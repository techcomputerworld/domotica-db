using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistem_Ventas.Library
{
    //el paginador puede tener ciertos problemas para mover mucha cantidad de tuplas de la base de datos
    //<T> pasarle la clase que nos haga falta para paginarla 
    public class Paginador<T>
    {
        //cantidad de resultados por página de las tuplas que visualizaremos.
        private int pagi_cuantos = 1;
        //cantidad de enlaces que se mostraran como máximo en la barra de navegación
        /*
         * pagi_nav_num_enlaces es el numero maximo de enlaces numericos que vamos a visualizar este numero debe depender de la cantidad 
         * de tuplas a mostrar tengo que programarlo para que sea así en el paginador.
        */
        private int pagi_nav_num_enlaces = 3;
        private int pagi_actual;
        //definimos que ira en el enlace a la página anterior
        private String pagi_nav_anterior = "&laquo; Anterior";
        //definimos que ira en el enlace a la página siguiente
        private String pagi_nav_siguiente = "Siguiente &raquo;";
        //definimos lapagina 1ª, ultima 
        private String pagi_nav_primera = "&laquo; Primero";
        private String pagi_nav_ultima = "Último &raquo; ";
        private String pagi_navegacion = null;
        //T nos sirve para colocar cualquier tipo de clase que nos haga falta, dependiendo de la clase asi vamos a crear
        //pagina es para saber el numero de página actual que estamos navegando
        //el area de ese controlador 
        // el nombre del controlador 
        //metodo de accion de ese controlador 
        //Direccion de nuestra aplicación web
        public object[] paginador(List<T> table, int pagina, String area, String controller, String action, String host)
        {
            //esto significa que no estamos navegando a ninguna página
            if (pagina == 0)
            {
                pagi_actual = 1;
            }
            else
            {
                pagi_actual = pagina;
            }
            int pagi_totalReg = table.Count;
            //este valor puede darme un valor con decimales
            double valor1 = pagi_totalReg / pagi_cuantos;
            int pagi_totalPags = Convert.ToInt16(Math.Round(valor1));
            if (pagi_actual != 1)
            {
                //si no estamos en la página 1. Ponemos el enlace "Primera" 
                int pagi_url = 1; //será el número de página al que enlazamos 
                pagi_navegacion += "<a id='paginas1' href='" + host + "/" + controller + "/" + action + "?id=" + pagi_url +
                    "&area=" + area + "'>" + pagi_nav_primera + "</a>";

                //Si no estamos en la página 1. Ponemos el enlace "anterior"
                pagi_url = pagi_actual - 1;
                pagi_navegacion += "<a id='paginas1' href='" + host + "/" + controller + "/" + action + "?id=" + pagi_url +
                    "&area=" + area + "'>" + pagi_nav_anterior + "</a>";
                /*
                pagi_navegacion += "<a id='paginas1' href='" + host + "/" + controller + "/" + action + "?id=" + pagi_url +
                    "&area=" + area + "'>" + pagi_nav_siguiente + "</a>";
                */
            }
            // si se definio la variable pagi__nav_num_enlaces
            // Calculamos el intervalo para restar y sumar a partir de la página actual 
            double valor2 = (pagi_nav_num_enlaces / 2);
            int pagi_nav_intervalo = Convert.ToInt16(Math.Round(valor2));
            // Calculamos desde que número de página se mostrara
            int pagi_nav_desde = pagi_actual - pagi_nav_intervalo;
            int pagi_nav_hasta = pagi_actual + pagi_nav_intervalo;

            //si pagi_nav_desde es un numero negativo
            if (pagi_nav_desde < 1)
            {
                pagi_nav_hasta -= (pagi_nav_desde - 1);
                pagi_nav_desde = 1;
            }
            if (pagi_nav_hasta > pagi_totalPags)
            {
                // Le restamos la cantidad excedida al comienzo para mantener el número de enlaces que se quieren mostrar
                pagi_nav_desde -= (pagi_nav_hasta - pagi_totalPags);
                // Establecemos pagi_nav_hasta como el total de páginas.
                pagi_nav_hasta = pagi_totalPags;
                if (pagi_nav_desde < 1)
                {
                    pagi_nav_desde = 1;
                    //establecemos pagi_nav_hasta como el total de páginas
                    pagi_nav_hasta = pagi_totalPags;
                }
            }
            for (int pagi_i = pagi_nav_desde; pagi_i <= pagi_nav_hasta; pagi_i++)
            {

                //desde la pagina 1 hasta la ultima página (pagi_totalPags) 
                if (pagi_i == pagi_actual)
                {
                    //Si  el número de la página actual (pagi_actual). Se escribe el número, pero sin enlace y en negrita
                    pagi_navegacion += "<span id='paginas2'>" + pagi_i + "</span>";
                }
                else
                {
                    //he hecho un arreglo para que se vea algo mejor visualmente los numeros con su espacio
                    pagi_navegacion += "<a id='paginas1' href='" + host + "/" + controller + "/" + action + "?id=" + pagi_i +
                    "&area=" + area + "'>" + " " + pagi_i + " " + "</a>";
                }

            }
            if (pagi_actual < pagi_totalPags)
            {
                //Si no estamos en la última página. Ponemos el enlace "Siguiente"
                int pagi_url = pagi_actual + 1;
                pagi_navegacion += "<a id='paginas1' href='" + host + "/" + controller + "/" + action + "?id=" + pagi_url +
                    "&area=" + area + "'>" + pagi_nav_siguiente + "</a>";
                //Si no estamos en la última página. Ponemos el enlace "Última" 
                pagi_url = pagi_totalPags;
                pagi_navegacion += "<a href='" + host + "/" + controller + "/" + action + "?id=" + pagi_url +
                    "&area=" + area + "'>" + pagi_nav_ultima + "</a>";
            }
            /*
             * Obtención de los registros que se mostraran en la página actual
             *
             */
            //calculamos desde que registro se mostrará en esta página 
            int pagi_inicial = (pagi_actual - 1) * pagi_cuantos;
            //consulta SQL. Devuelve cantidad de registros empezando desde pagi_actual
            var query = table.Skip(pagi_inicial).Take(pagi_cuantos).ToList();

            /*
             * Generación de la información sobre los registros mostrados.
             *  ---------------------------------------------------------
             */
            int pagi_desde = pagi_inicial + 1;
            int pagi_hasta = pagi_inicial + pagi_cuantos;

            if (pagi_hasta > pagi_totalPags)
            {
                pagi_hasta = pagi_totalPags;
            }
            String pagi_info = " del <b>" + pagi_desde + "</b> al <b>" + pagi_hasta + "</b> de <b>" + pagi_totalPags + "</b>";
            object[] data = { pagi_info, pagi_navegacion, query };
            //finalmente devolvera un array de tipo object para saber en que página estamos imagino
            return data;
        }
    }
}
