using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Caucedo.Base.Common;
using System.Xml.Linq;

namespace Caucedo.LoyaltyProgram.BLL
{
    public class UtilsApp
    {

        public class Reglas
        {
            public int id { get; set; }
            public string regla { get; set; }
            public List<Reglas> reglasList { get; set; }
        }
        public class Infraccion
        {
            public int id { get; set; }
            public string infraccion { get; set; }
        }


        public class Incidencias
        {
            public List<Reglas> reglasList { get; set; }
            public List<Infraccion> infraccionList { get; set; }
        }

        public class RecompensaC
        {
            public int ID { get; set; }
            public string Recompensa { get; set; }
            public int valorPuntos { get; set; }
            public int cantidad { get; set; }
        }

        public class Recompensas
        {
            public List<RecompensaC> RecompensasList { get; set; }
        }

        private static int PuntosReglasInfringidas = 0;
        private static int TotalPuntosDescontados = 0;

        public static Incidencias GenerarXML(List<string> reglasList, List<string> infraccionesList)
        {
            var NewIncidencia = new Incidencias();
            var re = new List<Reglas>();
            var infrac = new List<Infraccion>();

            var reglas = Bll.GetReglas();
            if (reglasList != null)
            {
                for (int i = 0; i < reglasList.Count; i++)
                {
                    var reglaFull = reglas.SingleOrDefault(rr => rr.ID_REGLA == Convert.ToInt32(reglasList[i]));
                    re.Add(new Reglas { id = reglaFull.ID_REGLA, regla = reglaFull.REGLA });
                }

            }

            var eventos = Bll.GetEventos();
            if (infraccionesList != null)
            {
                for (int i = 0; i < infraccionesList.Count; i++)
                {
                    var InfraccionFull = eventos.SingleOrDefault(inf => inf.ID_EVENTO == Convert.ToInt32(infraccionesList[i]));

                    infrac.Add(new Infraccion { id = InfraccionFull.ID_EVENTO, infraccion = InfraccionFull.EVENTO });

                }
            }

            NewIncidencia.reglasList = re;
            NewIncidencia.infraccionList = infrac;

            return NewIncidencia;
        }

        public static Recompensas GenerarXMLRecompensas(List<string> recompensasList)
        {
            var Recompensas = new Recompensas();

            var list = new List<RecompensaC>();

            var recompensas = Bll.GetRecompensas();

            foreach (var item in recompensasList.GroupBy(x => x))
            {
                var recompensaFull = recompensas.SingleOrDefault(rr => rr.ID_RECOMPENSA == Convert.ToInt32(item.Key));
                list.Add(new RecompensaC { ID = recompensaFull.ID_RECOMPENSA, Recompensa = recompensaFull.NOMBRE, valorPuntos = recompensaFull.VALOR_PUNTOS, cantidad = item.Count() });
            }
            Recompensas.RecompensasList = list;
            return Recompensas;
        }

        public static CambiosRegistroUpdate GenerarXMLCambiosRegistro(List<string> CambiosRegistro)
        {
            var Recompensas = new CambiosRegistroUpdate()
            {
                ROTULO = CambiosRegistro[0],
                FECHA_INCIDENTE = Convert.ToDateTime(CambiosRegistro[1]),
                DESCRIPCION = CambiosRegistro[2],
                USER = CambiosRegistro[3]
            };
            return Recompensas;
        }

        public static int GenerarPuntos(List<string> reglasList, List<string> infraccionesList)
        {
            PuntosReglasInfringidas = 0;
            TotalPuntosDescontados = 0;
            var eventos = Bll.GetEventos();
            var SI = eventos.SingleOrDefault(si => si.ID_EVENTO == 11);

            for (int i = 0; i < infraccionesList.Count; i++)
            {
                var InfraccionFull = eventos.SingleOrDefault(inf => inf.ID_EVENTO == Convert.ToInt32(infraccionesList[i]));

                if (InfraccionFull.ID_EVENTO == SI.ID_EVENTO)
                {
                    PuntosReglasInfringidas = reglasList.Count * InfraccionFull.PUNTAJE;
                }
                if (InfraccionFull.ID_EVENTO != 11)
                {
                    TotalPuntosDescontados += InfraccionFull.PUNTAJE;
                }
            }

            TotalPuntosDescontados += PuntosReglasInfringidas;
            return TotalPuntosDescontados;
        }

        //public static string GenerarEstadisticasReglas()
        //{
        //    var reglas = Bll.GetReglas();

        //    var xmlIncidentes = Bll.GetIncidentes();

        //    foreach (var incidente in xmlIncidentes)
        //    {
        //       var clase = Extensiones.DeserializeFromXml<Incidencias>(incidente);

        //            switch(clase.reglasList[0].id)
        //            {                  
        //                //case regla.ID_REGLA: break;
        //            }

        //    }


        //    return "";
        //}

    }

}
