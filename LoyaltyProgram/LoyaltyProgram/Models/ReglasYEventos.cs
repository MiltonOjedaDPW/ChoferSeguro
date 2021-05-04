using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caucedo.LoyaltyProgram.BLL;
using Caucedo.Base.Common;

namespace Caucedo.LoyaltyProgram.Models
{
    public class ReglasYEventos
    {
        public List<Eventos> Eventos { get; set; }
        public List<Reglas> Reglas { get; set; }
        public List<GetRegistroDeIncidenciasParams> Incidencias { get; set; }
        
        
        
        public ReglasYEventos()
        {
            DateTime date = DateTime.Today;
            
            GetRegistrosParams pa = new GetRegistrosParams();
            pa.DATE_START = date.AddDays(-10);
            pa.DATE_END = date.DayEnd();
            Eventos = Bll.GetEventos();
            Reglas = Bll.GetReglas();
            Incidencias = Bll.GetRegistrosDeIncidencias(pa);
        }
    }
}