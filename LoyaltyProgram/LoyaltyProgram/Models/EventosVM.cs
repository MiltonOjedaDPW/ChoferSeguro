using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caucedo.LoyaltyProgram.BLL;

namespace Caucedo.LoyaltyProgram.Models
{
    public class EventosVM
    {
        public List<Eventos> Eventos { get; set; }

        public EventosVM()
        {
            Eventos = Bll.GetEventos();
        }
    }
}