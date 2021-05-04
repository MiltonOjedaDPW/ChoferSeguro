using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caucedo.LoyaltyProgram.BLL
{
    public class InsertEventoParams
    {
        public int ID_EVENTO { get; set; }
        public string EVENTO { get; set; }
        public int PUNTAJE { get; set; }
        public string CREATED_BY { get; set; }
        public bool ACTIVO { get; set; }
    }
    public class Eventos
    {
        public int ID_EVENTO { get; set; }
        public string EVENTO { get; set; }
        public int PUNTAJE { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public bool ACTIVO { get; set; }
        public List<Eventos> eventos { get; set; }
    }
}
