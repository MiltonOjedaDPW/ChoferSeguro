using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caucedo.LoyaltyProgram.BLL;

namespace Caucedo.LoyaltyProgram.Models
{
    public class RecompensasVM
    {
        public List<Recompensas> Recompensa { get; set; }
        public List<string> Campanas { get; set; }
        public List<ListadoEntregaRecompensas> Recompensas { get; set; }
        public RecompensasVM()
        {
            Recompensa = Bll.GetRecompensas();
            Campanas = Bll.GetCampanasDeCanjes();
        }
    }
}