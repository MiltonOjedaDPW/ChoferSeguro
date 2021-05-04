using Caucedo.Base.Common.CustomActions.Interfaces;
using Caucedo.Base.SqlSvr.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Caucedo.Base.Common.CustomActions
{
    public class CustomEngine
    {
        private string[] _IgnoreDLL;
        private string _folderEntry;
        private object _param1, _param2, _param3, _param4, _param5, _param6, _param7;
        private static int _actionCount;

        public CustomEngine(string folderEntry, string[] IgnoreDLL = null, dynamic param1 = null, dynamic param2 = null, dynamic param3 = null, dynamic param4 = null, dynamic param5 = null,
            dynamic param6 = null, dynamic param7 = null)
        {
            this._IgnoreDLL = IgnoreDLL;
            this._folderEntry = folderEntry;
            this._param1 = param1;
            this._param2 = param2;
            this._param3 = param3;
            this._param4 = param4;
            this._param5 = param5;
            this._param6 = param6;
            this._param7 = param7;
        }

        /// <summary>
        /// load custom actions DLL from directory
        /// </summary>
        /// <param name="folder">main directory path </param>
        /// <returns></returns>
        private List<ICustomActionable> loadCustomActions(string folder, string filename = "")
        {

            var result = new List<ICustomActionable>();
            var dlls = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories)
                                .Select(f => Path.GetFullPath(f));

            if (!Directory.Exists(folder))
            {
                return null;
            }

            if (filename.Length > 0)
            {
                if (File.Exists(filename))
                {
                    dlls = dlls.Where(x => x == filename).ToList();
                }
            }

            foreach (var dll in dlls)
            {

                FileInfo fileInfo = new FileInfo(dll);

                if (_IgnoreDLL != null)
                {
                    if (_IgnoreDLL.FirstOrDefault(x => x.ToLower() == fileInfo.Name.ToLower()) != null)
                    {
                        continue;
                    }
                }

                var task = loadTask(dll);

                if (task != null)
                {
                    result.Add(task);
                }
                else
                {
                    continue;
                }
            }
            return result;
        }

        /// <summary>
        /// load and instantiate an object of type ICustomActionable
        /// </summary>
        /// <param name="dll">path of the custom DLL</param>
        /// <returns></returns>
        private ICustomActionable loadTask(string dll)
        {
            Assembly a = Assembly.LoadFile(dll);
            Type[] types = a.GetTypes();

            foreach (Type type in types)
            {

                // Does this class support the transport interface?
                Type typeTest = type.GetInterface("ICustomActionable");

                if (typeTest == null)
                {
                    // Not supported.
                    continue;
                }

                // This class supports the interface. Instantiate it.
                var obj = a.CreateInstance(type.FullName) as ICustomActionable;


                return obj;
            }

            return null;
        }

        /// <summary>
        /// Execute Custom Action from custom DLL
        /// </summary>
        /// <returns></returns>
        public StringBuilder Run(string dll = "")
        {
            StringBuilder log = new StringBuilder();
            List<ICustomActionable> DLLS = null;

            if (dll.Length == 0) { DLLS = loadCustomActions(_folderEntry); } else { DLLS = loadCustomActions(_folderEntry, dll); }

            if (DLLS == null)
            {
                return null;
            }

            foreach (var DLL in DLLS)
            {
                DLL.SetParameters(this._param1, this._param2, this._param3, this._param4, this._param5, this._param6, this._param7);
                log.Append(DLL.OnExecute() + Environment.NewLine);
                _actionCount += 1;
            }


            return log;
        }


        public static int GetActionCount()
        {
            return _actionCount;
        }


        #region obsolete
        /// <summary>
        ///  Execute Custom Action from custom DLL
        /// </summary>
        /// <param name="Stringparam">String parameter for custom action</param>
        /// <returns></returns>
        //public StringBuilder Run()
        //{
        //    StringBuilder log = new StringBuilder();
        //    var DLLS = loadCustomActions(_folderEntry);

        //    if (DLLS == null)
        //    {
        //        return null;
        //    }

        //    foreach (var DLL in DLLS)
        //    {
        //        DLL.SetParameters();
        //        log.Append(DLL.OnExecute() + Environment.NewLine);
        //        _actionCount += 1;
        //    }


        //    return log;
        //}

        /// <summary>
        /// Execute Custom Action from custom DLL
        /// </summary>
        /// <param name="ListStringParams">List String parameter for custom action</param>
        /// <returns></returns>
        //public StringBuilder Run(List<string> ListStringParams)
        //{
        //    StringBuilder log = new StringBuilder();
        //    var DLLS = loadCustomActions(_folderEntry);

        //    if (DLLS == null)
        //    {
        //        return null;
        //    }

        //    foreach (var DLL in DLLS)
        //    {
        //        log.Append(DLL.OnExecute(ListStringParams) + Environment.NewLine);
        //        _actionCount += 1;
        //    }


        //    return log;
        //}

        /// <summary>
        /// Execute Custom Action from custom DLL
        /// </summary>
        /// <param name="ListStringParams">List String parameter for custom action</param>
        /// <param name="database"> MS SQL Server Database connection object</param>
        /// <returns></returns>
        //public StringBuilder Run(List<string> ListStringParams, Database database)
        //{
        //    StringBuilder log = new StringBuilder();
        //    var DLLS = loadCustomActions(_folderEntry);

        //    if (DLLS == null)
        //    {
        //        return null;
        //    }

        //    foreach (var DLL in DLLS)
        //    {
        //        log.Append(DLL.OnExecute(ListStringParams, database) + Environment.NewLine);
        //        _actionCount += 1;
        //    }


        //    return log;
        //}

        //public StringBuilder Run(List<string> ListStringParams, Database database1, Database database2)
        //{
        //    StringBuilder log = new StringBuilder();
        //    var DLLS = loadCustomActions(_folderEntry);

        //    if (DLLS == null)
        //    {
        //        return null;
        //    }

        //    foreach (var DLL in DLLS)
        //    {
        //        log.Append(DLL.OnExecute(ListStringParams, database1, database2) + Environment.NewLine);
        //        _actionCount += 1;
        //    }


        //    return log;
        //}

        //public StringBuilder Run(List<string> ListStringParams, Database database1, Database database2, Database database3)
        //{
        //    StringBuilder log = new StringBuilder();
        //    var DLLS = loadCustomActions(_folderEntry);

        //    if (DLLS == null)
        //    {
        //        return null;
        //    }

        //    foreach (var DLL in DLLS)
        //    {
        //        log.Append(DLL.OnExecute(ListStringParams, database1, database2, database3) + Environment.NewLine);
        //        _actionCount += 1;
        //    }


        //    return log;
        //}

        //public StringBuilder Run(List<Common.Models.Employee> employees, Database database1, Database database2)
        //{
        //    StringBuilder log = new StringBuilder();
        //    var DLLS = loadCustomActions(_folderEntry);

        //    if (DLLS == null)
        //    {
        //        return null;
        //    }

        //    foreach (var DLL in DLLS)
        //    {
        //        log.Append(DLL.OnExecute(employees, database1, database2) + Environment.NewLine);
        //        _actionCount += 1;
        //    }


        //    return log;
        //}

        #endregion

    }
}
