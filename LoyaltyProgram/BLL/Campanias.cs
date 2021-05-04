using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caucedo.LoyaltyProgram.BLL
{
    public class Campanias
    {
        public int ID_CAMPANIA { get; set; }
        public string CAMPANIA { get; set; }
        public DateTime DATE_START { get; set; }
        public string STR_DATE_START => DATE_START.ToString("dd-MMM-yyyy");
        public DateTime DATE_END { get; set; }
        public string STR_DATE_END => DATE_END.ToString("dd-MMM-yyyy");
        public DateTime FECHA_CANJE { get; set; }
        public string STR_FECHA_CANJE => FECHA_CANJE.ToString("dd-MMM-yyyy");
        public string CREATED_BY { get; set; }
        public bool ACTIVO { get; set; }
    }

    public class CrearCampaniaParams
    {
        public int ID_CAMPANIA { get; set; }
        public string CAMPANIA { get; set; }
        public DateTime DATE_START { get; set; }
        public DateTime DATE_END { get; set; }
        public DateTime FECHA_CANJE { get; set; }
        public string CREATED_BY { get; set; }
        public bool ACTIVO { get; set; }
    }

    public class UpdateCampaniaParams
    {
        public int ID_CAMPANIA { get; set; }
        public string CAMPANIA { get; set; }
        public DateTime FECHA_CANJE { get; set; }
        public bool ACTIVO { get; set; }
    }
}
