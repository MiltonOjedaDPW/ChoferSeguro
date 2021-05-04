using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caucedo.LoyaltyProgram.BLL;

namespace Caucedo.LoyaltyMovil.Models
{
    public class dataVM
    {
        public List<Eventos> Eventos { get; set; }
        public List<Reglas> Reglas { get; set; }
        public List<Recompensas> Recompensa { get; set; }
        public List<Choferes> Chofer { get; set; }
        public List<Campanias> Campania { get; set; }
        public Choferes chofer { get; set; }
        public List<GetRegisDeIncidenciasChoferParams> ListaDeIncidencias { get; set; }
        public List<GetRegisDeIncidenciasChoferParams> ListaDeCanjes { get; set; }
        public List<GetRegisDeIncidenciasChoferParams> ListaDeIncidencias_LY { get; set; }
        public List<GetRegisDeIncidenciasChoferParams> ListaDeCanjes_LY { get; set; }
        public int PuntosCanjeados { get; set; }
        public int PuntosCanjeados_LY { get; set; }
        public dataVM()
        {
            Eventos = Bll.GetEventos();
            Reglas = Bll.GetReglas();
            Recompensa = Bll.GetRecompensas();
            Campania = Bll.GetCampanias();
            Chofer = Bll.GetChoferes();
        }
    }
}