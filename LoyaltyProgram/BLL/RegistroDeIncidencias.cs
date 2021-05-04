using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Caucedo.LoyaltyProgram.BLL
{

    public class RegIncidenciaParams
    {   
        public string ROTULO { get; set; }
        public string RNTT { get; set; }
        public string FULL_NAME { get; set; }
        public string CEDULA { get; set; }
        public string EMPRESA { get; set; }
        public DateTime FECHA { get; set; }
        public List<string> REGLAS { get; set; }
        public List<string> INFRACCIONES { get; set; }
        public string CREATED_BY { get; set; }
        public string DESCRIPCION { get; set; }
    }

    public class GetRegistrosParams
    {
        public DateTime DATE_START { get; set; }
        public DateTime DATE_END { get; set; }
        public string RNTT { get; set; }
        public string ROTULO { get; set; }
    }

    public class RegistroDeIncidencias
    {
        public int ID_REG_INCIDENCIA { get; set; }
        public string ROTULO { get; set; }
        public string RNTT { get; set; }
        public string FULL_NAME { get; set; }
        public string CEDULA { get; set; }
        public string EMPRESA { get; set; }
        public DateTime FECHA { get; set; }
        public string STR_FECHA => FECHA.ToString("dd-MMM-yyyy");
        public string STR_HORA => FECHA.ToString("hh:mm tt");
        public string INCIDENTES { get; set; }
        public string DESCRIPCION { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string STR_CREATED_DATE => CREATED_DATE.ToString("dd-MMM-yyyy HH:mm");
    }

    public class GetRegisDeIncidenciasChoferParams
    {
        public string ORIGEN { get; set; }
        public string MOTIVO { get; set; }
        public int PUNTOS { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string STR_FECHA => CREATED_DATE.ToString("dd-MM-yyyy");
        public string DETALLE { get; set; }
        public string strDETALLE { get; set; }
        public XDocument xml => XDocument.Parse(DETALLE);
        public List<string> listDetalle => (from i in xml.Descendants("RecompensaC").Elements()
                                           where i.Name == "Recompensa"
                                           select i.Value).ToList();
    }

    public class GetRegistroDeIncidenciasParams
    {
        public int ID_REGISTRO { get; set; }
        public string COD { get; set; }
        public string ROTULO { get; set; }
        public string RNTT { get; set; }
        public string FULL_NAME { get; set; }
        public string CEDULA { get; set; }
        public string EMPRESA { get; set; }
        public DateTime FECHA { get; set; }
        public string STR_FECHA => FECHA.ToString("dd-MMM-yyyy");
        public string STR_HORA => FECHA.ToString("hh:mm tt");
        public string REGLAS_LIST { get; set; }
        public string REGLAS { get; set; }
        public string DANOS { get; set; }
        public string LESIONES { get; set; }
        public string SEGURIDAD_FISICA { get; set; }
        public string DESCRIPCION { get; set; }

    }

    public class UpdateRegistroParams
    {
        public int ID_REGISTRO { get; set; }
        public string ROTULO { get; set; }
        public DateTime FECHA_INCIDENTE { get; set; }
        public string DESCRIPCION { get; set; }
        public string CAMBIOS { get; set; }
        public string USER { get; set; }
        public List<string> CambiosList { get; set; }

    }

    public class CambiosRegistroUpdate
    {
        public string ROTULO { get; set; }
        public DateTime FECHA_INCIDENTE { get; set; }
        public string DESCRIPCION { get; set; }
        public string USER { get; set; }
    }
}
