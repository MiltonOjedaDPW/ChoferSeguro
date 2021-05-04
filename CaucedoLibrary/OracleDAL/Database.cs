using Oracle.DataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Caucedo.Base.Oracle.DAL
{
    public class Database
    {
        private string connStr { get; set; }
        static private Database _DataServer = null;
        static public Database DataServer
        {
            get
            {
                if (_DataServer == null)
                {
                    string connStr = ConfigurationManager.ConnectionStrings["DataServer"].ConnectionString;
                    _DataServer = new Database(connStr);
                }
                return _DataServer;
            }
        }
        static public Database AdHoc(string server)
        {
            string connStr = ConfigurationManager.ConnectionStrings[server].ConnectionString;
            return new Database(connStr);
        }
        private Database(string connStr)
        {
            this.connStr = connStr;
        }
        public DataTable ExecQuery(string Query, string tableName, SearchRec search)
        {
            return ExecQuery(Query, tableName, search.ToSqlParams());
        }
        public DataTable ExecQuery(string Query, string tableName)
        {
            return ExecQuery(Query, tableName, new OracleParameter[] { });
        }
        public DataTable ExecQuery(string Query, string tableName, params OracleParameter[] Params)
        {
            OracleConnection conne = new OracleConnection(connStr);
            var da = new OracleDataAdapter(Query, conne);
            if (Params != null)
            {
                da.SelectCommand.Parameters.AddRange(Params);
            }
            conne.Open();
            DataSet ds = new DataSet();
            da.Fill(ds);
            da.InsertCommand.Connection = null;
            if (ds.Tables.Count > 0)
            {
                DataTable result = ds.Tables[0];
                result.TableName = tableName;
                return result;
            }
            return null;
        }
        public List<@type> ExecQuery<@type>(string Query, params OracleParameter[] Params) where @type : class
        {
            using (OracleConnection conne = new OracleConnection(connStr))
            {
                conne.Open();
                var comm = new OracleCommand(Query, conne);
                if (Params != null)
                {
                    comm.BindByName = true;
                    comm.Parameters.AddRange(Params);
                }
                comm.Prepare();
                var dr = comm.ExecuteReader(CommandBehavior.CloseConnection);
                var result = dr.ToList<@type>();
                return result;
            }
        }

        public OracleDataReader ExecQuery(string Query, params OracleParameter[] Params)
        {
            OracleConnection conne = new OracleConnection(connStr);
            conne.Open();
            var comm = new OracleCommand(Query, conne);
            if (Params != null)
            {
                comm.Parameters.AddRange(Params);
            }
            return comm.ExecuteReader(CommandBehavior.CloseConnection);
        }
        public int ExecNonQuery(string Query)
        {
            return ExecNonQuery(Query, new OracleParameter[] { });
        }
        public int ExecNonQuery(string Query, SearchRec search)
        {
            return ExecNonQuery(Query, search.ToSqlParams());
        }
        public int ExecSPNonQuery(string spName, params OracleParameter[] Params)
        {
            OracleConnection conne = new OracleConnection(connStr);
            OracleCommand comm = new OracleCommand(spName, conne);
            comm.Connection.Open();
            comm.Prepare();
            int r = 0;
            comm.CommandType = CommandType.StoredProcedure;
            if (Params != null)
            {
                comm.Parameters.AddRange(Params);
            }
            comm.Transaction = conne.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                r = comm.ExecuteNonQuery();
                comm.Transaction.Commit();
            }
            catch (Exception e)
            {

                comm.Transaction.Rollback();
                throw e;
            }
            finally
            {
                comm.Connection.Close();
                comm.Connection = null;
            }
            return r;
        }
        public int ExecNonQuery(string Query, params OracleParameter[] Params)
        {
            OracleConnection conne = new OracleConnection(connStr);
            OracleCommand comm = new OracleCommand(Query, conne);
            comm.Connection.Open();
            comm.Prepare();
            int r = 0;
            comm.CommandType = CommandType.Text;
            if (Params != null)
            {
                comm.Parameters.AddRange(Params);
            }
            comm.Transaction = conne.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                r = comm.ExecuteNonQuery();
                comm.Transaction.Commit();
            }
            catch (Exception e)
            {

                comm.Transaction.Rollback();
                throw e;
            }
            finally
            {
                comm.Connection.Close();
                comm.Connection = null;
            }
            return r;
        }
        public object ExecEscalar(string Query, SearchRec search)
        {
            return ExecEscalar(Query, search.ToSqlParams());
        }
        public object ExecSPEscalar(string SProc, params OracleParameter[] Params)
        {
            OracleConnection conne = new OracleConnection(connStr);
            OracleCommand comm = new OracleCommand(SProc, conne);
            comm.CommandType = CommandType.StoredProcedure;
            if (Params != null)
            {
                comm.Parameters.AddRange(Params);
                comm.Parameters.Add(new OracleParameter("resultCursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output });
            }
            var da = new OracleDataAdapter(comm);
            DataSet ds = new DataSet();
            da.Fill(ds);
            comm.Connection = null;
            return ds.Tables[0].Rows[0][0];
        }
        public object ExecEscalar(string Query, params OracleParameter[] Params)
        {
            OracleConnection conne = new OracleConnection(connStr);
            OracleCommand comm = new OracleCommand(Query, conne);
            comm.Connection.Open();
            comm.Prepare();
            object r = 0;
            comm.CommandType = CommandType.Text;
            comm.Parameters.AddRange(Params);
            comm.Transaction = conne.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                r = comm.ExecuteScalar();
                comm.Transaction.Commit();
            }
            catch (Exception e)
            {

                comm.Transaction.Rollback();
                throw e;
            }
            finally
            {
                comm.Connection.Close();
                comm.Connection = null;
            }
            return r;
        }
        public DataSet ExecSelSPDS(string SProc, SearchRec search)
        {
            return ExecSelSPDS(SProc, search.ToSqlParams());
        }
        public DataSet ExecSelSPDS(string SProc, params OracleParameter[] Params)
        {
            OracleConnection conne = new OracleConnection(connStr);
            OracleCommand comm = new OracleCommand(SProc, conne);
            comm.CommandType = CommandType.StoredProcedure;
            if (Params != null)
            {
                comm.Parameters.AddRange(Params);
            }
            var da = new OracleDataAdapter(comm);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        delegate void ExecSPAsyncDel(string Text);
        public void ExecuteSelSPAsync(string sp, IAsyncResult result, params OracleParameter[] Params)
        {
            OracleConnection conne = new OracleConnection(connStr);
            OracleCommand comm = new OracleCommand(sp, conne);
            comm.CommandType = CommandType.StoredProcedure;
            if (Params != null)
            {
                comm.Parameters.AddRange(Params);
            }
        }

        public DataTable ExecSelSP(string SProc, params OracleParameter[] Params)
        {
            DataSet ds = ExecSelSPDS(SProc, Params);
            return (ds.Tables.Count > 0) ? ds.Tables[0] : null;
        }
        public DataTable ExecSelSP<@type>(string SProc, ref @type paramObj)
        {
            OracleParameter[] Params = SearchRec.ToSqlParams(paramObj);
            DataSet ds = ExecSelSPDS(SProc, Params);
            foreach (var para in Params)
            {
                if (para.Direction == ParameterDirection.Output)
                {
                    paramObj.SetFieldValue(para.ParameterName.Replace("@", ""), para.Value);
                }
            }
            return (ds.Tables.Count > 0) ? ds.Tables[0] : null;
        }
        public OracleDataReader ExecReaderSelSP(string SProc, params OracleParameter[] Params)
        {
            OracleConnection conne = new OracleConnection(connStr);
            conne.Open();
            OracleCommand comm = new OracleCommand(SProc, conne);
            comm.CommandType = CommandType.StoredProcedure;
            if (Params != null)
            {
                comm.Parameters.AddRange(Params);
                comm.Parameters.Add(new OracleParameter("resultCursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output });
            }
            return comm.ExecuteReader(CommandBehavior.CloseConnection);
        }
        public List<@type> ExecReaderSelSP<@type>(string SProc, Dictionary<string, Func<@type, object>> convertMap = null, params OracleParameter[] Params) where @type : class
        {
            using (OracleConnection conne = new OracleConnection(connStr))
            {
                conne.Open();
                OracleCommand comm = new OracleCommand(SProc, conne);
                comm.CommandType = CommandType.StoredProcedure;
                if (Params != null)
                {
                    comm.BindByName = true;
                    comm.Parameters.AddRange(Params);
                    comm.Parameters.Add(new OracleParameter("resultCursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output });
                }
                comm.Prepare();
                var rdr = comm.ExecuteReader(CommandBehavior.CloseConnection);
                var result = rdr.ToList<@type>(convertMap);
                return result;
            }
        }
    }

    public class SearchRec
    {
        private Dictionary<string, object> parameters;
        public SearchRec()
        {
            this.parameters = new Dictionary<string, object>();
        }
        public void AddParam(string name, object value)
        {
            this.parameters.Add(name, value);
        }
        public void AddParam(string name, object value, bool ignoreIfNull)
        {
            if (ignoreIfNull)
            {
                if (value == null)
                {
                    return;
                }
            }
            this.parameters.Add(name, value);
        }
        public override string ToString()
        {
            string result = string.Empty;
            List<string> fields = this.parameters.Keys.ToList<string>();
            foreach (string f in fields)
            {
                string value = Convert.ToString(this.parameters[f]).Trim();
                if (value != string.Empty)
                {
                    result = string.Concat(result, "@", f, " ='", value, "', ");
                }
            }
            result = result.Remove(result.LastIndexOf(","));
            return result;
        }
        public OracleParameter[] ToSqlParams()
        {
            List<OracleParameter> result = new List<OracleParameter>();
            foreach (var item in this.parameters)
            {
                if (item.Value == null)
                {
                    continue;
                }
                string campo = item.Key;
                object Value = item.Value;
                if (Value is System.Collections.ICollection)// tipo.GetProperty(info.Name).GetType() is IEnumerable)
                {
                    Value = getIEnumerableValues(Value as IEnumerable);
                }
                if (item.Value.GetType().Equals(typeof(DateTime)))
                {
                    bool badSqlDt = (Convert.ToDateTime(Value) < DateTime.MinValue)
                                    ||
                                 (Convert.ToDateTime(Value) > DateTime.MaxValue);
                    if (badSqlDt)
                    {
                        continue;
                    }
                }
                var p = new OracleParameter(campo, Value);
                result.Add(p);
            }
            return result.ToArray();
        }
        static public OracleParameter[] ToSqlParams(object obj, string[] PropiedadesAIgnorar = null, OracleParameter[] ParametrosAgregar = null)
        {
            List<OracleParameter> result = new List<OracleParameter>();

            Type tipo = obj.GetType();
            PropertyInfo[] infoCampos = tipo.GetProperties();
            foreach (var info in infoCampos)
            {
                bool ignorar = false; bool salida = false; int paramSize = 255; bool convertirXML = false;
                if (PropiedadesAIgnorar != null)
                {
                    if (PropiedadesAIgnorar.Contains(info.Name, StringComparer.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }
                }
                Attribute[] atributos = Attribute.GetCustomAttributes(tipo.GetProperty(info.Name));
                foreach (var att in atributos)
                {
                    if (att is IgnorarEnParam)
                    {
                        if ((att as IgnorarEnParam).Ignorar)
                        {
                            ignorar = true;
                        }
                    }
                    if (att is ParametroSalida)
                    {
                        if ((att as ParametroSalida).EsParametroSalida)
                        {
                            salida = true;
                            paramSize = (att as ParametroSalida).Tamaño;
                        }
                    }
                    if (att is GuardarXML)
                    {
                        convertirXML = true;
                    }
                }
                if (ignorar)
                {
                    continue;
                }
                string campo = info.Name;
                object valor = tipo.GetProperty(info.Name).GetValue(obj, null);
                if (valor == null)
                {
                    //continue;
                    valor = DBNull.Value;
                }
                else
                {

                    if (info.PropertyType is System.Collections.ICollection)// tipo.GetProperty(info.Name).GetType() is IEnumerable)
                    {
                        valor = getIEnumerableValues(valor as IEnumerable);
                    }
                    if (info.PropertyType.Equals(typeof(DateTime)))
                    {
                        bool badSqlDt = (Convert.ToDateTime(valor) < DateTime.MinValue)
                                        ||
                                     (Convert.ToDateTime(valor) > DateTime.MaxValue);
                        if (badSqlDt)
                        {
                            continue;
                        }
                    }
                    //valor = Convert.ChangeType(valor, info.PropertyType);
                    if (info.PropertyType.Equals(typeof(Boolean)))
                    {
                        valor = Convert.ToBoolean(valor) ? 1 : 0;
                    }
                }
                var newParam = new OracleParameter(campo, valor);
                if (salida)
                {
                    newParam.Direction = ParameterDirection.Output;
                    if (valor is string)
                    {
                        newParam.Size = paramSize;
                    }
                }
                result.Add(newParam);
            }
            if (ParametrosAgregar != null)
            {
                result.AddRange(ParametrosAgregar);
            }
            return result.ToArray();
        }

        private static string getIEnumerableValues(IEnumerable valor)
        {
            StringBuilder result = new StringBuilder();
            foreach (var item in valor)
            {
                result.Append(item.ToString());
                result.Append('█');
            }
            if (result.Length > 0) { result.Length--; }
            return result.ToString();
        }

        public void AddParam(OracleParameter[] paramss)
        {
            foreach (var item in paramss)
            {
                string nm = item.ParameterName;
                nm = nm.StartsWith("@") ? nm.Substring(1) : nm;
                this.parameters.Add(nm, item.Value);
            }
        }
    }

    public static class DalExtensions
    {
        public static Dictionary<string, int> GetAllNames(this IDataRecord record)
        {
            var result = new Dictionary<string, int>();
            for (int i = 0; i < record.FieldCount; i++)
            {
                result.Add(record.GetName(i).ToLower(), i);
            }
            return result;
        }
        public static bool IsNull<T>(this T source)
        {
            return (source == null) || (Convert.IsDBNull(source));
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
        public static XElement SerializeToXml<T>(T obj)
        {
            string result;
            XmlSerializer Serializer = new XmlSerializer(typeof(T));
            using (StringWriter stringWriter = new StringWriter())
            {
                Serializer.Serialize(stringWriter, obj);
                result = stringWriter.ToString();
                stringWriter.Close();
            }
            return XElement.Parse(result);
        }
        public static object GetFieldValue<T>(this T obj, string FieldName)
        {
            Type t = obj.GetType();
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                if (pi.Name.Equals(FieldName))
                {
                    return pi.GetValue(obj, null);
                }
            }
            return null;
        }
        public static void SetFieldValue<T>(this T obj, string FieldName, object value)
        {
            Type t = obj.GetType();
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                if (pi.Name.Equals(FieldName))
                {
                    if (pi.CanWrite)
                    {
                        try
                        {
                            pi.SetValue(obj, value, null);
                        }
                        catch /*(Exception e)*/
                        {
                            pi.SetValue(obj, null, null);
                        }
                    }
                }

            }
        }
        public static DataTable ToDataTable<@Type>(this IEnumerable<@Type> list, Dictionary<string, Func<object, object>> convertMap = null, List<string> skipList = null)
        {
            DataTable result = new DataTable();
            PropertyInfo[] properties = typeof(@Type).GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                if (skipList != null)
                {
                    if (skipList.Contains(pi.Name))
                    {
                        continue;
                    }
                }
                var newCol = new DataColumn(pi.Name);
                result.Columns.Add(newCol);
            }
            foreach (var item in list)
            {
                var newRow = result.NewRow();
                foreach (PropertyInfo pi in properties)
                {
                    if (skipList != null)
                    {
                        if (skipList.Contains(pi.Name))
                        {
                            continue;
                        }
                    }
                    if (convertMap != null)
                    {
                        if (convertMap.ContainsKey(pi.Name))
                        {
                            newRow[pi.Name] = convertMap[pi.Name](pi.GetValue(item, null));
                            continue;
                        }
                    }
                    newRow[pi.Name] = pi.GetValue(item, null);
                }
                result.Rows.Add(newRow);
            }
            return result;
        }
        public static List<@Type> ToList<@Type>(this IDataReader dr, Dictionary<string, Func<@Type, object>> convertMap = null) where @Type : class
        {
            var result = new List<@Type>();
            PropertyInfo[] properties = typeof(@Type).GetProperties();
            var hasDefaultConstructor = (typeof(@Type).GetConstructor(System.Type.EmptyTypes) != null);
            while (dr.Read())
            {
                var obj = (!hasDefaultConstructor) ? default(@Type) : (@Type)Activator.CreateInstance(typeof(@Type));
                if (!hasDefaultConstructor)
                {
                    obj = ((dr[0] == null) || (dr[0] == DBNull.Value)) ? default(@Type) : (@Type)dr[0];
                }
                else
                {
                    var dataFields = dr.GetAllNames();
                    foreach (PropertyInfo pi in properties)
                    {
                        if (pi.CanWrite)
                        {
                            string propName = pi.Name;
                            if (!dataFields.ContainsKey(propName.ToLower())) { continue; }
                            var propValue = dr[propName];
                            try
                            {
                                if (!convertMap.IsNull())
                                {
                                    if (convertMap.ContainsKey(propName.ToLower()))
                                    {
                                        continue;
                                    }
                                }
                                if (!propValue.IsNull())
                                {
                                    pi.SetValue(obj, Convert.ChangeType(propValue, pi.PropertyType), null);
                                }
                                else
                                {
                                    bool canBeNull = !pi.PropertyType.IsValueType || (Nullable.GetUnderlyingType(pi.PropertyType) != null);
                                    if (canBeNull)
                                    {
                                        pi.SetValue(obj, null, null);
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                pi.SetValue(obj, null, null);
                            }
                        }
                    }
                }
                if (convertMap != null)
                {
                    foreach (var entry in convertMap)
                    {
                        var campo = entry.Key;
                        var objValue = convertMap[campo](obj);
                        obj.SetFieldValue(campo, objValue);
                    }
                }
                result.Add(obj);
            }
            return result;
        }
        public static void GetFromDataRow<@Type>(this @Type obj, DataRow dr)
        {
            PropertyInfo[] properties = typeof(@Type).GetProperties();
            List<string> cols = new List<string>();
            foreach (DataColumn col in dr.Table.Columns)
            {
                cols.Add(col.ColumnName.ToUpper());
            }
            foreach (PropertyInfo pi in properties)
            {
                if (cols.Contains(pi.Name.ToUpper()))
                {
                    if (pi.CanWrite)
                    {
                        try
                        {
                            pi.SetValue(obj, dr[pi.Name], null);
                        }
                        catch
                        {
                            pi.SetValue(obj, null, null);
                        }
                    }
                }

            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class GuardarXML : Attribute
    {
        public GuardarXML()
        {

        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class GuardarEncriptado : Attribute
    {
        public bool Encryptar { get; set; }
        public GuardarEncriptado()
        {
            this.Encryptar = true;
        }
        public GuardarEncriptado(bool encriptar)
        {
            this.Encryptar = encriptar;
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnorarEnParam : Attribute
    {
        public bool Ignorar { get; set; }
        public IgnorarEnParam()
        {
            this.Ignorar = true;
        }
        public IgnorarEnParam(bool Ignorar)
        {
            this.Ignorar = Ignorar;
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class ParametroSalida : Attribute
    {
        public bool EsParametroSalida { get; set; }
        public int Tamaño { get; set; }
        public ParametroSalida()
        {
            this.EsParametroSalida = false;
        }
        public ParametroSalida(bool DeSalida, int Tamaño)
        {
            this.EsParametroSalida = DeSalida;
            this.Tamaño = Tamaño;
        }
    }
}
