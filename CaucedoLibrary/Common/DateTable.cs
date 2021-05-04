using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Caucedo.Base.Common
{
    public enum RangoFechas
    {
        Ninguno,
        Cualquier_Fecha,
        Semana_Anterior,
        Ayer,
        Hoy,
        Mañana_De_Hoy,
        Tarde_De_Hoy,
        Noche_De_Hoy,
        Primer_Turno,
        Segundo_Turno,
        Tercer_Turno,
        Mañana,
        Esta_Semana,
        Este_Mes,
        Próxima_Semana,
        Entre_Las_Fechas,
        En_Fecha,
        En_Hora
    }

    public delegate DateTime CalculoFecha(DateTime desdeFecha, DateTime hastaFecha);

    public class DateCalcTableEntry
    {
        public CalculoFecha CalcDesdeFecha { get; set; }
        public CalculoFecha CalcHastaFecha { get; set; }
    }
    public class DateCalcTable : IDictionary<RangoFechas, DateCalcTableEntry>
    {
        Dictionary<RangoFechas, DateCalcTableEntry> dicc = new Dictionary<RangoFechas, DateCalcTableEntry>();
        public void Add(RangoFechas rango, CalculoFecha calcDesdeFecha, CalculoFecha calcHastaFecha)
        {
            this.dicc.Add(rango, new DateCalcTableEntry() { CalcDesdeFecha = calcDesdeFecha, CalcHastaFecha = calcHastaFecha });
        }
        public void Add(RangoFechas rango, DateCalcTableEntry calculos)
        {
            this.dicc.Add(rango, calculos);
        }

        public bool ContainsKey(RangoFechas rango)
        {
            return this.dicc.ContainsKey(rango);
        }

        public ICollection<RangoFechas> Keys
        {
            get { return this.dicc.Keys; }
        }

        public bool Remove(RangoFechas rango)
        {
            return this.dicc.Remove(rango);
        }

        public bool TryGetValue(RangoFechas rango, out DateCalcTableEntry calculos)
        {
            return this.dicc.TryGetValue(rango, out calculos);
        }

        public ICollection<DateCalcTableEntry> Values
        {
            get { return this.dicc.Values; }
        }

        public DateCalcTableEntry this[RangoFechas rango]
        {
            get
            {
                return this.dicc[rango];
            }
            set
            {
                this.dicc[rango] = value;
            }
        }

        public void Add(KeyValuePair<RangoFechas, DateCalcTableEntry> item)
        {
            this.dicc.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this.dicc.Clear();
        }

        public bool Contains(KeyValuePair<RangoFechas, DateCalcTableEntry> item)
        {
            return this.dicc.Contains(item);
        }

        public void CopyTo(KeyValuePair<RangoFechas, DateCalcTableEntry>[] array, int arrayIndex)
        {

        }

        public int Count
        {
            get { return this.dicc.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<RangoFechas, DateCalcTableEntry> item)
        {
            return this.dicc.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<RangoFechas, DateCalcTableEntry>> GetEnumerator()
        {
            return this.dicc.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this.dicc as IEnumerable).GetEnumerator();
        }
    }

    public class FiltroFecha : IComparable<FiltroFecha>, IEquatable<FiltroFecha>
    {
        private DateTime _desdeFecha = DateTime.Today;
        public Func<RangoFechas, DateTime, string> CalcLabel { get; set; }
        public string Label { get { return (CalcLabel != null) ? CalcLabel(this.Rango, this.DesdeFecha) : this.Rango.ToString().Replace('_', ' '); } }
        public DateTime DesdeFecha
        {
            get { return _desdeFecha; }
        }
        private DateTime _hastaFecha = DateTime.Today;
        public DateTime HastaFecha
        {
            get { return _hastaFecha; }
        }

        private RangoFechas _rango = RangoFechas.Ninguno;
        public RangoFechas Rango
        {
            get { return _rango; }
            set
            {
                try
                {
                    calcValues(value, _desdeFecha, _hastaFecha);
                }
                catch //(Exception e)
                {
                    setToday();
                }
            }
        }

        private void setToday()
        {
            this._rango = RangoFechas.Hoy;
            this._desdeFecha = DateTime.Today;
            this._hastaFecha = DateTime.Today;
        }

        private void calcValues(RangoFechas rango, DateTime desdeFecha, DateTime hastaFecha)
        {
            try
            {
                this._rango = rango;
                this._desdeFecha = ObjectNames.DefaultDateCalcTable[rango].CalcDesdeFecha(desdeFecha, hastaFecha);
                this._hastaFecha = ObjectNames.DefaultDateCalcTable[rango].CalcHastaFecha(desdeFecha, hastaFecha);
            }
            catch //(Exception e)
            {
                setToday();
            }
        }
        private FiltroFecha()
        {
            this.CalcLabel = (rango, fecha) => { return this.Rango.ToString().Replace('_', ' '); };
        }
        public FiltroFecha(XElement xml) : base()
        {
            try
            {
                this._desdeFecha = (xml.Attribute("desdeFecha") != null) ? DateTime.Parse(xml.Attribute("desdeFecha").Value) : DateTime.Today;
                this._hastaFecha = (xml.Attribute("hastaFecha") != null) ? DateTime.Parse(xml.Attribute("hastaFecha").Value) : DateTime.Today.EndOfDay();
                RangoFechas rango = (xml.Attribute("rango") != null) ? (RangoFechas)Enum.Parse(typeof(RangoFechas), xml.Attribute("rango").Value) : RangoFechas.Ninguno;
                Rango = rango;
            }
            catch //(Exception e)
            {
                setToday();
            }
        }
        public FiltroFecha(RangoFechas rango, DateTime desdeFecha, DateTime hastaFecha) : base()
        {
            try
            {
                this._desdeFecha = desdeFecha;
                this._hastaFecha = hastaFecha;
                this.Rango = rango;
            }
            catch //(Exception e)
            {
                setToday();
            }
        }
        public override string ToString()
        {
            return genDescription();
        }

        private string genDescription()
        {
            var result = "";
            switch (this.Rango)
            {
                case RangoFechas.Ninguno:
                    break;
                case RangoFechas.Cualquier_Fecha:
                    break;
                case RangoFechas.Semana_Anterior:
                    {
                        result = string.Format("La Semana Anterior ({0} hasta {1})", this._desdeFecha.ToShortDateString(), this._hastaFecha.ToShortDateString());
                    }
                    break;
                case RangoFechas.Ayer:
                    {
                        result = string.Format("Ayer ({0})", this._desdeFecha.ToShortDateString());
                    }
                    break;
                case RangoFechas.Hoy:
                    {
                        result = string.Format("Hoy ({0})", this._desdeFecha.ToShortDateString());
                    }
                    break;
                case RangoFechas.Mañana_De_Hoy:
                    {
                        result = string.Format("Mañana de Hoy ({0} hasta {1})", this._desdeFecha.ToShortTimeString(), this._hastaFecha.ToShortTimeString());
                    }
                    break;
                case RangoFechas.Tarde_De_Hoy:
                    {
                        result = string.Format("Tarde de Hoy ({0} hasta {1})", this._desdeFecha.ToShortTimeString(), this._hastaFecha.ToShortTimeString());
                    }
                    break;
                case RangoFechas.Noche_De_Hoy:
                    {
                        result = string.Format("Noche de Hoy ({0} hasta {1})", this._desdeFecha.ToShortTimeString(), this._hastaFecha.ToShortTimeString());
                    }
                    break;
                case RangoFechas.Primer_Turno:
                    {
                        result = string.Format("Primer Turno ({0} hasta {1})", this._desdeFecha.ToShortTimeString(), this._hastaFecha.ToShortTimeString());
                    }
                    break;
                case RangoFechas.Segundo_Turno:
                    {
                        result = string.Format("Segundo Turno ({0} hasta {1})", this._desdeFecha.ToShortTimeString(), this._hastaFecha.ToShortTimeString());
                    }
                    break;
                case RangoFechas.Tercer_Turno:
                    {
                        result = string.Format("Tercer Turno ({0} hasta {1})", this._desdeFecha.ToShortTimeString(), this._hastaFecha.ToShortTimeString());
                    }
                    break;
                case RangoFechas.Mañana:
                    {
                        result = string.Format("Mañana ({0})", this._desdeFecha.ToShortDateString());
                    }
                    break;
                case RangoFechas.Esta_Semana:
                    {
                        result = string.Format("Esta Semana ({0} hasta {1})", this._desdeFecha.ToShortDateString(), this._hastaFecha.ToShortDateString());
                    }
                    break;
                case RangoFechas.Este_Mes:
                    {
                        result = string.Format("Este Mes ({0} hasta {1})", this._desdeFecha.ToShortDateString(), this._hastaFecha.ToShortDateString());
                    }
                    break;
                case RangoFechas.Próxima_Semana:
                    {
                        result = string.Format("Próxima Semana ({0} hasta {1})", this._desdeFecha.ToShortDateString(), this._hastaFecha.ToShortDateString());
                    }
                    break;
                case RangoFechas.Entre_Las_Fechas:
                    {
                        result = string.Format("Desde {0} hasta {1}", this._desdeFecha.ToShortDateString(), this._hastaFecha.ToShortDateString());
                    }
                    break;
                case RangoFechas.En_Fecha:
                    {
                        result = string.Format("En {0}", this._desdeFecha.ToShortDateString());
                    }
                    break;
                case RangoFechas.En_Hora:
                    {
                        result = string.Format("En {0}", this._desdeFecha.ToShortTimeString());
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
        public XElement ToXml()
        {
            var result = new XElement("fecha");
            result.SetAttributeValue("rango", this.Rango);
            result.SetAttributeValue("label", this.Label);
            result.SetAttributeValue("desdeFecha", this.DesdeFecha.ToString("yyyy-MM-ddTHH:mm:ss"));
            result.SetAttributeValue("hastaFecha", this.HastaFecha.ToString("yyyy-MM-ddTHH:mm:ss"));
            return result;
        }

        public int CompareTo(FiltroFecha other)
        {
            return this.Rango.CompareTo(other.Rango);
        }

        public bool Equals(FiltroFecha other)
        {
            return (this.Rango.Equals(other.Rango)
                  &&
                  this.DesdeFecha.Equals(other.DesdeFecha)
                  &&
                  this.HastaFecha.Equals(other.HastaFecha));
        }
    }

    public class FiltroFechaList : IList<FiltroFecha>
    {
        List<FiltroFecha> list = new List<FiltroFecha>();
        public void Add(FiltroFecha filtro)
        {
            this.list.Add(filtro);
        }
        public void Add(RangoFechas rango, DateTime desdeFecha, DateTime hastaFecha)
        {
            this.list.Add(new FiltroFecha(rango, desdeFecha, hastaFecha));
        }
        public void Clear()
        {
            this.list.Clear();
        }
        public int IndexOf(FiltroFecha filtro)
        {
            return this.list.IndexOf(filtro);
        }
        public void Remove(FiltroFecha filtro)
        {
            this.list.Remove(filtro);
        }
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var item in this.list)
            {
                result.AppendLine(item.ToString());
            }
            return result.ToString();
        }
        public FiltroFechaList()
        {
            this.list = new List<FiltroFecha>();
        }
        public FiltroFechaList(XElement xml)
        {
            this.list = new List<FiltroFecha>();
            foreach (var item in xml.Descendants("fecha"))
            {
                this.list.Add(new FiltroFecha(item));
            }
        }
        public XElement ToXml()
        {
            var result = new XElement("filtros");
            foreach (var item in this.list)
            {
                result.Add(item.ToXml());
            }
            return result;
        }


        public void Insert(int index, FiltroFecha item)
        {
            this.list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this.list.RemoveAt(index);
        }

        public FiltroFecha this[int index]
        {
            get
            {
                return this.list[index];
            }
            set
            {
                this.list[index] = value;
            }
        }


        public bool Contains(FiltroFecha item)
        {
            return this.list.Contains(item);
        }

        public void CopyTo(FiltroFecha[] array, int arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<FiltroFecha>.Remove(FiltroFecha item)
        {
            return this.list.Remove(item);
        }

        public IEnumerator<FiltroFecha> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this.list as IEnumerable).GetEnumerator();
        }
    }
    static class ObjectNames
    {
        public static int TicketWidth { get { return 320; } }
        public static int TicketHeight { get { return 200; } }
        public static string DefaultDateFormat { get { return _getConfigValue("DefaultDateFormat", "dd/MM/yyyy hh:mm"); } }
        public static string HoraPrincipioDia { get { return "00:00";/* _getConfigValue("HoraPrincipioDia", "00:00");*/ } }

        public static string HoraInicio1erTurno { get { return _getConfigValue("HoraInicio1erTurno", "07:00"); } }
        public static string HoraFin1erTurno { get { return _getConfigValue("HoraFin1erTurno", "14:59"); } }

        public static string HoraInicio2doTurno { get { return _getConfigValue("HoraInicio2doTurno", "15:00"); } }
        public static string HoraFin2doTurno { get { return _getConfigValue("HoraFin2doTurno", "22:59"); } }

        public static string HoraInicio3erTurno { get { return _getConfigValue("HoraInicio3erTurno", "23:00"); } }
        public static string HoraFin3erTurno { get { return _getConfigValue("HoraFin3erTurno", "06:59"); } }

        public static string DefaultDateFilters { get { return _getConfigValue("DefaultDateFilters", "<filtros><fecha rango='Hoy'/> <fecha rango='Ayer'/><fecha rango='Esta_Semana'/><fecha rango='Este_Mes'/></filtros>"); } }
        static string _getConfigValue(string name, string defaultValue)
        {
            var result = defaultValue;
            if (ConfigurationManager.AppSettings[name] != null)
            {
                result = ConfigurationManager.AppSettings[name];
            }
            return result;
        }
        public static DateCalcTable DefaultDateCalcTable
        {
            get
            {
                return new DateCalcTable(){
             {  RangoFechas.Ninguno,
                new DateCalcTableEntry()
                                        {
                                            CalcDesdeFecha=(fromDt, toDt)=>{ return new DateTime(1900, 01, 01);},
                                            CalcHastaFecha=(fromDt, toDt)=>{ return new DateTime(1900, 01, 01);}
                                        }
            },
            {  RangoFechas.Cualquier_Fecha,
                new DateCalcTableEntry()
                                        {
                                            CalcDesdeFecha=(fromDt, toDt)=>{ return new DateTime(1900, 01, 01);},
                                            CalcHastaFecha=(fromDt, toDt)=>{ return new DateTime(2100, 01, 01);}
                                        }
            },
            {  RangoFechas.Semana_Anterior,
                new DateCalcTableEntry()
                                        {
                                            CalcDesdeFecha=(fromDt, toDt) =>
                                            {
                                                return (DateTime.Today.DayOfWeek == DayOfWeek.Monday) ? DateTime.Today.AddDays(-7).SetHora(HoraPrincipioDia)
                                                                                                      : DateTime.Today.LastMonday().AddDays(-7).SetHora(HoraPrincipioDia);
                                            },
                                            CalcHastaFecha=(fromDt, toDt) =>
                                            {
                                                return (DateTime.Today.DayOfWeek == DayOfWeek.Monday) ? DateTime.Today.AddDays(-7).AddDays(6).EndOfDay()
                                                                                                      : DateTime.Today.LastMonday().AddDays(-7).AddDays(6).EndOfDay();
                                            }
                                        }
            },
            {  RangoFechas.Esta_Semana,
                new DateCalcTableEntry()
                                        {
                                            CalcDesdeFecha=(fromDt, toDt) =>
                                            {
                                                return (DateTime.Today.DayOfWeek == DayOfWeek.Sunday) ? DateTime.Today.SetHora(HoraPrincipioDia)
                                                                                                      : DateTime.Today.GetNext(DayOfWeek.Sunday, false).SetHora(HoraPrincipioDia);
                                            },
                                            CalcHastaFecha=(fromDt, toDt) =>
                                            {
                                                return (DateTime.Today.DayOfWeek == DayOfWeek.Sunday) ? DateTime.Today.AddDays(7).EndOfDay()
                                                                                                      : DateTime.Today.GetNext(DayOfWeek.Sunday, false).AddDays(7).EndOfDay();
                                            }
                                        }
            },
             {  RangoFechas.Ayer,
                new DateCalcTableEntry()
                                        {
                                            CalcDesdeFecha=(fromDt, toDt) => { return DateTime.Today.AddDays(-1).SetHora(HoraPrincipioDia);},
                                            CalcHastaFecha=(fromDt, toDt) => { return DateTime.Today.AddDays(-1).EndOfDay();}
                                        }
            },
            {  RangoFechas.Hoy,
                new DateCalcTableEntry()
                                        {
                                            CalcDesdeFecha=(fromDt, toDt) => {return DateTime.Today.SetHora(HoraPrincipioDia);},
                                            CalcHastaFecha=(fromDt, toDt) => {return DateTime.Today.EndOfDay();}
                                        }
            },
            {  RangoFechas.Mañana_De_Hoy,
                new DateCalcTableEntry()
                                        {
                                            CalcDesdeFecha=(fromDt, toDt) => {return DateTime.Today.SetHora(HoraPrincipioDia);},
                                            CalcHastaFecha=(fromDt, toDt) => {return DateTime.Today.SetHora("11:59");}
                                        }
            },
            {  RangoFechas.Tarde_De_Hoy,
                new DateCalcTableEntry()
                                        {
                                            CalcDesdeFecha=(fromDt, toDt) => {return DateTime.Today.SetHora("12:00");},
                                            CalcHastaFecha=(fromDt, toDt) => {return DateTime.Today.SetHora("17:59");}
                                        }
            },
            {  RangoFechas.Noche_De_Hoy,
                new DateCalcTableEntry()
                                        {
                                            CalcDesdeFecha=(fromDt, toDt) => {return DateTime.Today.SetHora("18:00");},
                                            CalcHastaFecha=(fromDt, toDt) => {return DateTime.Today.EndOfDay();}
                                        }
            },
             {  RangoFechas.Mañana,
                new DateCalcTableEntry()
                                        {
                                            CalcDesdeFecha=(fromDt, toDt) => {return DateTime.Today.AddDays(1).SetHora(HoraPrincipioDia);},
                                            CalcHastaFecha=(fromDt, toDt) => {return DateTime.Today.AddDays(1).EndOfDay();}
                                        }
            },
            {  RangoFechas.Primer_Turno,
                new DateCalcTableEntry()
                                        {
                                            CalcDesdeFecha=(fromDt, toDt) => {return DateTime.Today.SetHora(HoraInicio1erTurno);},
                                            CalcHastaFecha=(fromDt, toDt) => {return DateTime.Today.SetHora(HoraFin1erTurno);}
                                        }
            },
            {  RangoFechas.Segundo_Turno,
                new DateCalcTableEntry()
                                        {
                                            CalcDesdeFecha=(fromDt, toDt) => {return DateTime.Today.SetHora(HoraInicio2doTurno);},
                                            CalcHastaFecha=(fromDt, toDt) => {return DateTime.Today.SetHora(HoraFin2doTurno);}
                                        }
            },
            {  RangoFechas.Tercer_Turno,
                new DateCalcTableEntry()
                                        {
                                            CalcDesdeFecha=(fromDt, toDt) => {return DateTime.Today.SetHora(HoraInicio3erTurno);},
                                            CalcHastaFecha=(fromDt, toDt) => {return DateTime.Today.AddDays(1).SetHora(HoraFin3erTurno);}
                                        }
            },
             {  RangoFechas.Próxima_Semana,
                new DateCalcTableEntry()
                                        {
                                            CalcDesdeFecha=(fromDt, toDt) => {return DateTime.Today.NextMonday().SetHora(HoraPrincipioDia);},
                                            CalcHastaFecha=(fromDt, toDt) => {return DateTime.Today.NextMonday().AddDays(6).EndOfDay();}
                                        }
            },
            {  RangoFechas.Este_Mes,
                new DateCalcTableEntry()
                                        {
                                            CalcDesdeFecha=(fromDt, toDt) => {return DateTime.Today.MonthBegin();},
                                            CalcHastaFecha=(fromDt, toDt) => {return DateTime.Today.MonthEnd();}
                                        }
            },
            {  RangoFechas.Entre_Las_Fechas,
                new DateCalcTableEntry()
                                        {
                                            CalcDesdeFecha=(fromDt, toDt) => {return fromDt.SetHora(HoraPrincipioDia);},
                                            CalcHastaFecha=(fromDt, toDt) => {return toDt.EndOfDay();}
                                        }
            },
            {  RangoFechas.En_Fecha,
                new DateCalcTableEntry()
                                        {
                                            CalcDesdeFecha=(fromDt, toDt) => {return fromDt.SetHora(HoraPrincipioDia);},
                                            CalcHastaFecha=(fromDt, toDt) => {return fromDt.EndOfDay();}
                                        }
            },
            {  RangoFechas.En_Hora,
                new DateCalcTableEntry()
                                        {
                                            CalcDesdeFecha=(fromDt, toDt) => {return fromDt.SetMinutos("00");},
                                            CalcHastaFecha=(fromDt, toDt) => {return fromDt.SetMinutos("59");}
                                        }
            }
        };
            }
        }


    }
}
