using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caucedo.LoyaltyProgram.BLL
{
    public class RegistroManual
    {
        public int ID_PUNTOS_MANUAL { get; set; }
        public string RNTT { get; set; }
        public string NOMBRE { get; set; }
        public string CEDULA { get; set; }
        public string DESCRIPCION { get; set; }
        public string TIPO_EVENTO { get; set; }
        public int PUNTOS { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
    }

    public class RegistroManualParams
    {
        public string RNTT { get; set; }
        public string NOMBRE { get; set; }
        public string CEDULA { get; set; }
        public string DESCRIPCION { get; set; }
        public string TIPO_EVENTO { get; set; }
        public int PUNTOS { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }

    }

}
