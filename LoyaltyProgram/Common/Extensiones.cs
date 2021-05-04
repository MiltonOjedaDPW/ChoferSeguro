using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;
using System.Data;
using System.ComponentModel;
using System.Reflection;
using System.Xml;
using System.Web.Script.Serialization;

namespace Caucedo.Base.Common
{
    public static class Extensiones
    {
        public static string[] SplitByCase(this string str, StringSplitOptions options = StringSplitOptions.None)
        {
            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
            var result = r.Replace(str, "~~").Split(new string[] { "~~" }, options);
            return result;
        }
        public static @type[] Join<@type>(this @type[] array1, @type[] array2)
        {
            @type[] newArray = new @type[array1.Length + array2.Length];
            Array.Copy(array1, newArray, array1.Length);
            Array.Copy(array2, 0, newArray, array1.Length, array2.Length);
            return newArray;
        }
        public static int ToInteger(this double num, bool absolute)
        {
            return absolute ? (int)Math.Abs(Math.Truncate(num)) : (int)Math.Truncate(num);
        }
        public static int ToInteger(this double num)
        {
            return (int)(Math.Truncate(num));
        }
        public static DateTime PrincipioQuincena(this DateTime dt)
        {
            var day = (dt.Day < 16) ? 1 : 16;
            return new DateTime(dt.Year, dt.Month, day, 0, 0, 0);
        }
        public static DateTime FinQuincena(this DateTime dt)
        {
            var day = (dt.Day < 16) ? 15 : dt.MonthEnd().Day;
            return new DateTime(dt.Year, dt.Month, day, 0, 0, 0);
        }
        public static bool InThePast(this DateTime dt, DateTime? currDt = null)
        {
            if (!currDt.HasValue)
            {
                currDt = DateTime.Now;
            }
            return currDt.Value.CompareTo(dt) > 0;
        }
        public static bool InTheFuture(this DateTime dt, DateTime? currDt = null)
        {
            if (!currDt.HasValue)
            {
                currDt = DateTime.Now;
            }
            return currDt.Value.CompareTo(dt) < 0;
        }
        public static string DateDiff(this DateTime date, string lang = "sp")
        {
            var currDt = DateTime.Now;
            var diff = currDt.Subtract(date);
            string result = date.ToShortDateString();

            var futureDt = (currDt.CompareTo(date) < 0);
            var spanish = (lang == "sp");

            var timePrefix = spanish ? (futureDt ? "En" : "Hace") : (futureDt ? "In" : "");
            var timeSuffix = spanish ? (futureDt ? "" : "") : (futureDt ? "" : "ago");

            var years = (diff.TotalDays.ToInteger(true) % 365);
            var totalDays = diff.TotalDays.ToInteger(true);
            var hours = diff.TotalHours.ToInteger(true);
            var minutes = diff.TotalMinutes.ToInteger(true);
            var seconds = diff.TotalSeconds.ToInteger(true);

            if (minutes <= 0)
            {
                result = spanish ? string.Format("{1} {0} segundos {2}", seconds, timePrefix, timeSuffix)
                                 : string.Format("{1} {0} seconds {2}", seconds, timePrefix, timeSuffix);
            }
            else
            {
                if (hours <= 0)
                {
                    var term = spanish ? (minutes > 1) ? "minutos" : "minuto"
                                       : (minutes > 1) ? "minutes" : "minute";
                    result = spanish ? string.Format("{1} {0} {3} {2}", minutes, timePrefix, timeSuffix, term)
                                     : string.Format("{1} {0} {3} {2}", minutes, timePrefix, timeSuffix, term);
                }
                else
                {
                    if (totalDays <= 0)
                    {

                        var term = spanish ? (hours > 1) ? "horas" : "hora"
                                           : (hours > 1) ? "hours" : "hour";
                        result = spanish ? string.Format("{1} {0} {3} {2}", hours, timePrefix, timeSuffix, term)
                                         : string.Format("{1} {0} {3} {2}", hours, timePrefix, timeSuffix, term);
                    }
                    else
                    {
                        if (totalDays == 1)
                        {
                            result = spanish ? futureDt ? "Mañana" : "Ayer"
                                             : futureDt ? "Tomorrow" : "Yesterday";
                        }
                        else
                        {
                            if (totalDays < 365)
                            {
                                if (totalDays < 30)
                                {
                                    result = spanish ? string.Format("{1} {0} dias {2}", totalDays, timePrefix, timeSuffix)
                                                     : string.Format("{1} {0} days {2}", totalDays, timePrefix, timeSuffix);
                                }
                                else
                                {
                                    var months = ((currDt.Year - date.Year) * 12) + currDt.Month - date.Month;
                                    if (months == 1)
                                    {
                                        result = spanish ? string.Format("{1} {0} mes {2}", months, timePrefix, timeSuffix)
                                                         : string.Format("{1} {0} month {2}", months, timePrefix, timeSuffix);
                                    }
                                    else
                                    {
                                        result = spanish ? string.Format("{1} {0} meses {2}", months, timePrefix, timeSuffix)
                                                         : string.Format("{1} {0} months {2}", months, timePrefix, timeSuffix);
                                    }
                                }
                            }
                            else
                            {
                                var term = spanish ? (years > 1) ? "años" : "año"
                                                   : (years > 1) ? "years" : "year";
                                result = spanish ? string.Format("{1} {0} {3} {2}", years, timePrefix, timeSuffix, term)
                                                 : string.Format("{1} {0} {3} {2}", years, timePrefix, timeSuffix, term);
                            }
                        }
                    }
                }
            }
            return result.Trim();
        }
        public static bool ContainsX(this string str, string value)
        {
            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(str, value, CompareOptions.IgnoreCase
                                                                            | CompareOptions.IgnoreNonSpace) > -1;
        }
        public static string ToCSV(this IDictionary<string, string> dicc)
        {
            var result = new StringBuilder();
            foreach (var kv in dicc)
            {
                result.Append($"{kv.Key}:{kv.Value}"); result.Append(",");
            }
            result.Remove(result.Length - 1, 1);
            return result.ToString();
        }
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }
        /// <summary>
        /// Get the array slice between the two indexes.
        /// ... Inclusive for start index, exclusive for end index.
        /// </summary>
        public static T[] Slice<T>(this T[] source, int start, int end)
        {
            // Handles negative ends.
            if (end < 0)
            {
                end = source.Length + end;
            }
            if (end > source.Length)
            {
                end = source.Length;
            }
            int len = end - start;

            // Return new array.
            T[] res = new T[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = source[i + start];
            }
            return res;
        }
        public static string ToCSV<@T>(this IEnumerable<@T> objs, Func<T, string> fnGetVal, string separator = ",")
        {
            var result = new StringBuilder();
            foreach (var o in objs)
            {
                if (o.IsNull()) { continue; }
                result.Append(fnGetVal(o)); result.Append(separator);
            }
            if (result.Length > 0)
            {
                result.Remove(result.Length - 1, 1);
            }
            return result.ToString();
        }
        public static string ToCSV(this IEnumerable<string> strs, string separator = ",")
        {
            var result = new StringBuilder();
            if (strs.Count() > 0)
            {
                foreach (var s in strs)
                {
                    result.Append(s); result.Append(separator);
                }
                result.Remove(result.Length - 1, 1);
            }
            return result.ToString();
        }
        public static bool ContainsAll(this string str, params string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (!str.ContainsX(values[i].Trim()))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool ContainsAny(this string str, params string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (str.ContainsX(values[i].Trim()))
                {
                    return true;
                }
            }
            return false;
        }
        public static object GetPropValue(this object obj, string propName)
        {
            var @type = obj.GetType();
            var p = @type.GetProperty(propName);
            return (p != null) ? p.GetValue(obj) : null;
        }
        public static void SetPropValue(this object obj, string propName, object value)
        {
            var @type = obj.GetType();
            var p = @type.GetProperty(propName);
            p.SetValue(obj, Convert.ChangeType(value, p.PropertyType));
        }
        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }
        public static void ProcessMatches(this string text, string pattern, Action<int, string> onMatch)
        {
            var matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                foreach (Group group in match.Groups)
                {
                    onMatch(group.Index, group.Value);
                }
            }
        }
        public static string ReplaceMatches(this string text, string pattern, string value)
        {
            return Regex.Replace(text, pattern, value);
        }
        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            T[] dest = new T[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, dest, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

            return dest;
        }
        public static int IndexOf<T>(this T[] source, T element)
        {
            return Array.IndexOf(source, element);
        }
        public static T[] Append<T>(this T[] source, T element)
        {
            Array.Resize(ref source, source.Length);
            source[source.Length - 1] = element;
            return source;
        }
        public static bool IsNull<T>(this T source)
        {
            return (source == null);
        }
        public static T DeserializeFromXml<T>(string xml)
        {
            T result;
            var ser = new XmlSerializer(typeof(T));
            using (var tr = new StringReader(xml))
            {
                result = (T)ser.Deserialize(tr);
            }
            return result;
        }
        static string RemoveInvalidXmlChars(string text)
        {
            var validXmlChars = text.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray();
            return new string(validXmlChars);
        }

        static bool IsValidXmlString(string text)
        {
            try
            {
                XmlConvert.VerifyXmlChars(text);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string CleanXML(this string xml)
        {
            return RemoveInvalidXmlChars(xml);
        }
        public static XElement SerializeToXml<T>(this T obj, params Type[] extraTypes)
        {
            string result;
            XmlSerializer Serializer = new XmlSerializer(typeof(T), extraTypes);
            using (StringWriter stringWriter = new StringWriter())
            {
                Serializer.Serialize(stringWriter, obj);
                result = stringWriter.ToString();
                stringWriter.Close();
            }
            return XElement.Parse(result);
        }
        public static string SerializeToJSON<T>(this T obj)
        {
            return new JavaScriptSerializer().Serialize(obj);
            /*
                //Create a stream to serialize the object to.  
                MemoryStream ms = new MemoryStream();
                // Serializer the User object to the stream.  
                var ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(ms, obj);
                byte[] json = ms.ToArray();
                ms.Close();
                return Encoding.UTF8.GetString(json, 0, json.Length);*/
        }
        public static T DeserializeFromJson<T>(string json)
        {
            T instance = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var ser = new DataContractJsonSerializer(instance.GetType());
            var result = (T)ser.ReadObject(ms);
            ms.Close();
            return result;
        }

        public static string ToHex(this Int64 intValue)
        {
            // Convert integer 182 as a hex in a string variable
            string hexValue = intValue.ToString("X");
            return hexValue;
        }

        public static ulong FromHexToInteger(this string hexValue)
        {
            // Convert the hex string back to the number
            var intAgain = ulong.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
            return intAgain;
        }

        public static string RemoveDomain(this string userName)
        {
            return userName.Substring(userName.LastIndexOf('\\') + 1);
        }
        public static DateTime LastWorkingDay(this DateTime dt)
        {
            dt = dt.AddDays(-1);
            while ((dt.DayOfWeek == DayOfWeek.Saturday) || (dt.DayOfWeek == DayOfWeek.Sunday))
            {
                dt = dt.AddDays(-1);
            }
            return dt;
        }
        public static DateTime SetHora(this DateTime dt, string hora)
        {
            dt = new DateTime(dt.Year, dt.Month, dt.Day, int.Parse(hora.Split(':')[0]), int.Parse(hora.Split(':')[1]), 00);
            return dt;
        }
        public static DateTime SetMinutos(this DateTime dt, string minutos)
        {
            dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, int.Parse(minutos), 00);
            return dt;
        }
        static public bool Between(this DateTime dt, DateTime dtFrom, DateTime dtTo)
        {
            return (dt >= dtFrom) && (dt <= dtTo);
        }
        static public DateTime DayStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day);
        }
        static public DateTime DayEnd(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day).AddDays(1).AddMinutes(-1);
        }
        static public DateTime ParseTime(string hora)
        {
            return new DateTime(1900, 01, 01, int.Parse(hora.Split(':')[0]), int.Parse(hora.Split(':')[1]), 00);
        }
        public static DateTime GetNext(this DateTime fromDt, DayOfWeek dow, bool forward = true)
        {
            while (fromDt.DayOfWeek != dow)
            {
                fromDt = fromDt.AddDays(forward ? 1 : -1);
            }
            return fromDt;
        }
        public static DateTime MonthEnd(this DateTime date)
        {
            DateTime result = new DateTime(date.Year, date.Month, 01);
            result = result.AddMonths(1).Subtract(new TimeSpan(0, 0, 1));
            return result;
        }
        public static DateTime MonthBegin(this DateTime date)
        {
            DateTime result = new DateTime(date.Year, date.Month, 01);
            return result;
        }
        public static DateTime NextMonday(this DateTime date)
        {
            DateTime result = date.AddDays(1);
            while (result.DayOfWeek != DayOfWeek.Monday)
            {
                result = result.AddDays(1);
            }
            return result;
        }
        public static DateTime LastMonday(this DateTime date)
        {
            DateTime result = date.AddDays(-1);
            while (result.DayOfWeek != DayOfWeek.Monday)
            {
                result = result.AddDays(-1);
            }
            return result;
        }
        public static DateTime BeginOfWeek(this DateTime date)
        {
            DateTime result = date;
            while (result.DayOfWeek != DayOfWeek.Sunday)
            {
                result = result.AddDays(-1);
            }
            return result;
        }
        public static DateTime EndOfWeek(this DateTime date)
        {
            DateTime result = date;
            while (result.DayOfWeek != DayOfWeek.Saturday)
            {
                result = result.AddDays(1);
            }
            return result;
        }
        static GregorianCalendar _gc = new GregorianCalendar();
        public static int GetWeekOfMonth(this DateTime time)
        {
            DateTime first = new DateTime(time.Year, time.Month, 1);
            return time.GetWeekOfYear() - first.GetWeekOfYear() + 1;
        }

        static int GetWeekOfYear(this DateTime time)
        {
            return _gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }
        /// <summary>
        /// Last minute and last second of the day
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime EndOfDay(this DateTime date)
        {
            return date.Date.AddDays(1).AddSeconds(-1);
        }
        /// <summary>
        /// Number of week on the year
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int WeekNumber(this DateTime date)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            return cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }
        public static string DiaSemana(this DateTime date)
        {
            return date.ToString("dddd");
        }
        public static DateTime[] SemanasEnMes(this DateTime date)
        {
            var dates = Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month)).Select(n => new DateTime(date.Year, date.Month, n));
            var weekends = from d in dates
                           group d by d.WeekNumber() into wn
                           select wn.First();
            return weekends.ToArray();
        }
        //static public void DataToCSV(this DataGridView gridView, string fileName, Dictionary<string, string> colFormulas = null)
        //{
        //	var sb = new StringBuilder();
        //	var headers = gridView.Columns.Cast<DataGridViewColumn>();
        //	if (colFormulas == null) colFormulas = new Dictionary<string, string>();

        //	sb.AppendLine(string.Join(",", headers.Select(column => "\"" + column.HeaderText + "\"").ToArray()));

        //	var n = 2;
        //	foreach (DataGridViewRow row in gridView.Rows)
        //	{
        //		var cells = row.Cells.Cast<DataGridViewCell>();
        //		sb.AppendLine(string.Join(",", cells.Select(cell => "\"" + (colFormulas.ContainsKey(cell.OwningColumn.DataPropertyName) ? "=" + colFormulas[cell.OwningColumn.DataPropertyName].Replace("##", n.ToString())
        //																																: cell.FormattedValue) + "\"").ToArray()));
        //		n++;
        //	}
        //	File.WriteAllText(fileName, sb.ToString(), Encoding.Default);
        //}
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static DataTable Pivot(this DataTable dt, DataColumn pivotColumn, DataColumn pivotValue)
        {
            // find primary key columns 
            //(i.e. everything but pivot column and pivot value)
            DataTable temp = dt.Copy();
            temp.Columns.Remove(pivotColumn.ColumnName);
            temp.Columns.Remove(pivotValue.ColumnName);
            string[] pkColumnNames = temp.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToArray();

            // prep results table
            DataTable result = temp.DefaultView.ToTable(true, pkColumnNames).Copy();
            result.PrimaryKey = result.Columns.Cast<DataColumn>().ToArray();
            dt.AsEnumerable()
                .Select(r => r[pivotColumn.ColumnName].ToString())
                .Distinct().ToList()
                .ForEach(c => result.Columns.Add(c, pivotColumn.DataType));

            // load it
            foreach (DataRow row in dt.Rows)
            {
                // find row to update
                DataRow aggRow = result.Rows.Find(
                    pkColumnNames
                        .Select(c => row[c])
                        .ToArray());
                // the aggregate used here is LATEST 
                // adjust the next line if you want (SUM, MAX, etc...)
                aggRow[row[pivotColumn.ColumnName].ToString()] = row[pivotValue.ColumnName];
            }

            return result;
        }
        /// <summary>
        /// Gets a Inverted DataTable
        /// </summary>        
        /// <param name="columnX">X Axis Column>/param>
        /// <param name="columnY">Y Axis Column>/param>
        /// <param name="columnZ">Z Axis Column (values)>/param>
        /// <param name="columnsToIgnore">Whether to ignore some column, it must be provided here>/param>
        /// <param name="nullValue">null Values to be filled>/param> 
        /// <returns>C# Pivot Table Method  - Felipe Sabino>/returns>
        public static DataTable GetInversedDataTable(this DataTable table, string columnX,
             string columnY, string columnZ, string nullValue, bool sumValues)
        {
            //Create a DataTable to Return
            DataTable returnTable = new DataTable();

            if (columnX == "")
                columnX = table.Columns[0].ColumnName;

            //Add a Column at the beginning of the table
            returnTable.Columns.Add(columnY);


            //Read all DISTINCT values from columnX Column in the provided DataTale
            List<string> columnXValues = new List<string>();

            foreach (DataRow dr in table.Rows)
            {

                string columnXTemp = dr[columnX].ToString();
                if (!columnXValues.Contains(columnXTemp))
                {
                    //Read each row value, if it's different from others provided, add to 
                    //the list of values and creates a new Column with its value.
                    columnXValues.Add(columnXTemp);
                    returnTable.Columns.Add(columnXTemp);
                }
            }

            //Verify if Y and Z Axis columns re provided
            if (columnY != "" && columnZ != "")
            {
                //Read DISTINCT Values for Y Axis Column
                var columnYValues = new List<string>();

                foreach (DataRow dr in table.Rows)
                {
                    if (!columnYValues.Contains(dr[columnY].ToString()))
                        columnYValues.Add(dr[columnY].ToString());
                }

                //Loop all Column Y Distinct Value
                foreach (string columnYValue in columnYValues)
                {
                    //Creates a new Row
                    DataRow drReturn = returnTable.NewRow();
                    drReturn[0] = columnYValue;
                    //foreach column Y value, The rows are selected distincted
                    DataRow[] rows = table.Select(columnY + "='" + columnYValue + "'");

                    //Read each row to fill the DataTable
                    foreach (DataRow dr in rows)
                    {
                        string rowColumnTitle = dr[columnX].ToString();

                        //Read each column to fill the DataTable
                        foreach (DataColumn dc in returnTable.Columns)
                        {
                            if (dc.ColumnName == rowColumnTitle)
                            {
                                //If Sum of Values is True it try to perform a Sum
                                //If sum is not possible due to value types, the value 
                                // displayed is the last one read
                                if (sumValues)
                                {
                                    try
                                    {
                                        drReturn[rowColumnTitle] =
                                             Convert.ToDecimal(drReturn[rowColumnTitle]) +
                                             Convert.ToDecimal(dr[columnZ]);
                                    }
                                    catch
                                    {
                                        drReturn[rowColumnTitle] = dr[columnZ];
                                    }
                                }
                                else
                                {
                                    drReturn[rowColumnTitle] = dr[columnZ];
                                }
                            }
                        }
                    }
                    returnTable.Rows.Add(drReturn);
                }
            }
            else
            {
                throw new Exception("The columns to perform inversion are not provided");
            }

            //if a nullValue is provided, fill the datable with it
            if (nullValue != "")
            {
                foreach (DataRow dr in returnTable.Rows)
                {
                    foreach (DataColumn dc in returnTable.Columns)
                    {
                        if (dr[dc.ColumnName].ToString() == "")
                            dr[dc.ColumnName] = nullValue;
                    }
                }
            }

            return returnTable;
        }
        public static void InsOrUpd<T>(this IList<T> list, T obj)
        {
            var idx = list.IndexOf(obj);
            if (idx < 0)
            {
                list.Add(obj);
            }
            else
            {
                list[idx] = obj;
            }
        }
        public static List<T> ToList<T>(this DataTable dt, Action<DataRow, Exception> OnError = null)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row, OnError);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr, Action<DataRow, Exception> OnError = null)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            try
            {
                foreach (DataColumn column in dr.Table.Columns)
                {
                    foreach (PropertyInfo pro in temp.GetProperties())
                    {
                        if (pro.Name == column.ColumnName)
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        else
                            continue;
                    }
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(dr, ex);
                }
                else
                {
                    throw;
                }
            }
            return obj;
        }
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
    }
}
