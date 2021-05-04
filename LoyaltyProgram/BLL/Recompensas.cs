using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Caucedo.LoyaltyProgram.BLL
{
    public class InsertRecompensaParams
    {
        public int ID_RECOMPENSA { get; set; }
        public string NOMBRE { get; set; }
        public string IMAGEN { get; set; }
        public string DESCRIPCION { get; set; }
        public int VALOR_PUNTOS { get; set; }
        public int STOCK { get; set; }
        public string CREATED_BY { get; set; }
        public bool ACTIVO { get; set; }
    }

    public class Recompensas
    {
        DateTimeFormatInfo MxFormat = new CultureInfo("es-MX", false).DateTimeFormat;
        public int ID_RECOMPENSA { get; set; }
        public string NOMBRE { get; set; }
        public string IMAGEN { get; set; }
        public string DESCRIPCION { get; set; }
        public int VALOR_PUNTOS { get; set; }
        public int STOCK { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public bool ACTIVO { get; set; }
        public string STR_CREATED_DATE => CREATED_DATE.ToString(MxFormat.LongDatePattern);
        public List<Recompensas> recompensas { get; set; }
    }

    public class EntregarRecompensaParams
    {
        public string RNTT { get; set; }
        public List<string> RECOMPENSAS { get; set; }
        public int PUNTOS_TOTALES { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
    }

    public class ListadoEntregaRecompensas
    {
        public string RNTT { get; set; }
        public string CHOFER { get; set; }
        public string CEDULA { get; set; }
        public string RECOMPENSAS { get; set; }
        public int PUNTOS_RECOMEPENSAS_T { get; set; }
        public string CAMPANIA { get; set; }
        public string RECOMPENSAS_LIST { get; set; }
        //public List<string> listRecompensas => (from i in XDocument.Parse(RECOMPENSAS).Descendants("RecompensaC").Elements()
        //                                    where i.Name == "Recompensa"
        //                                    select i.Value).ToList();
    }
}
