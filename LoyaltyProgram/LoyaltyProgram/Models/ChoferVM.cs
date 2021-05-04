using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caucedo.LoyaltyProgram.BLL;

namespace Caucedo.LoyaltyProgram.Models
{
    public class ChoferVM
    {
        public List<Choferes> Chofer { get; set; }
        public ChoferVM()
        {
            Chofer = Bll.GetChoferes();
        }
    }
}