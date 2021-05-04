using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caucedo.LoyaltyProgram.BLL
{
    public class InsertChoferParams
    {
        public int ID_CHOFER { get; set; }
        public string CHOFER { get; set; }
        public string CEDULA { get; set; }
        public string RNTT { get; set; }
        public string EMPRESA { get; set; }
        public int PUNTOS_GANADOS { get; set; }
        public int PUNTOS_BALANCE { get; set; }
        public int PUNTOS_DESCONTADOS { get; set; }
    }
    public class Choferes
    {
        public int ID_CHOFER { get; set; }
        public string CHOFER { get; set; }
        public string CEDULA { get; set; }
        public string RNTT { get; set; }
        public string EMPRESA { get; set; }
        public string ESTATUS_AFILACION { get; set; }
        public int PUNTOS_VISITAS_RECORD { get; set; }
        public int PUNTOS_GANADOS { get; set; }
        public int PUNTOS_BALANCE { get; set; }
        public int PUNTOS_DESCONTADOS { get; set; }
        public int PUNTOS_GANADOS_LY { get; set; }
        public int PUNTOS_BALANCE_LY { get; set; }
        public int PUNTOS_DESCONTADOS_LY { get; set; }
        public string COD { get; set; }
        public int PUNTOS_ANO_ANTERIOSR { get; set; }
        public override string ToString()
        {
            return $"CHOFER: {CHOFER} - Cédula: {CEDULA} - RNTT: {RNTT} - ESTATUS {ESTATUS_AFILACION}";
        }
    }

    public class ChoferesN4
    {
        public string DRIVER_CARD_ID { get; set; }
        public DateTime ENTERED_YARD { get; set; }
    }

    public class ChoferesToSQL
    {
        public int RNTT { get; set; }
        public int VISITS { get; set; }
    }


}
