using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caucedo.LoyaltyProgram.BLL
{
    public class InsertReglasParams
    {
        public int ID_REGLA { get; set; }
        public string REGLA { get; set; }
        public string ICON { get; set; }
        public string DESCRIPCION { get; set; }
        public string CREATED_BY { get; set; }
        public bool ACTIVO { get; set; }
    }
    public class Reglas
    {
        public int ID_REGLA { get; set; }
        public string REGLA { get; set; }
        public string ICON { get; set; }
        public string DESCRIPCION { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public bool ACTIVO { get; set; }
        public List<Reglas> reglas { get; set; }
    }
}
