using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caucedo.Base.Common.CustomActions
{
    public enum LoggerContext
    {
        Info = 1,
        Error = 2,
        Fatal = 3,
        Warn = 4,
    }

    public sealed class CustomLogger
    {
        private List<string> _Logging;
        private string _ProcessName;
        private string _ProcessDescription;
        private string _ProcessType;

        private static int logCounter = 1;

        public CustomLogger(string processName, string processDescription,string Processtype)
        {
            _ProcessName = processName;
            _ProcessDescription = processDescription;
            _ProcessType = Processtype;

            _Logging = new List<string>();
        }

        public void Add(LoggerContext logType, string LogMessage, string LogauxField = "", string LogauxField2 = "")
        {
            if (LogMessage.Length == 0) { return; }

            string errortypeDescription = string.Empty;
            switch (logType)
            {
                case LoggerContext.Info:
                    errortypeDescription = "Info";
                    break;
                case LoggerContext.Error:
                    errortypeDescription = "Error";
                    break;
                case LoggerContext.Fatal:
                    errortypeDescription = "Fatal";
                    break;
                case LoggerContext.Warn:
                    errortypeDescription = "Warning";
                    break;
                default:
                    errortypeDescription = "Info";
                    break;
            }


            string log = string.Format("\"Type\":\"{0}\",\"Message\":\"{1}\",\"AuxField1\":\"{2}\",\"AuxField2\":\"{3}\",\"LogID\":{4} ",
                errortypeDescription, LogMessage, LogauxField, LogauxField2, logCounter);

            _Logging.Add(log);

            Console.WriteLine(log);
            logCounter += 1;
        }
        public void Remove(int Id)
        {
            if (_Logging.Count > 0)
            {
                var item = _Logging.FirstOrDefault(x => x.Contains(Id.ToString()));

                if (item != null)
                {
                    _Logging.Remove(item);
                }
            }
        }
        public string GetLogger()
        {
            string logger = string.Empty;

            if (_Logging.Count == 0)
            {
                return "No logger items added";
            }

            logger = logger = "{ \"Logger\": { " + Environment.NewLine;
            logger = logger + string.Format("   \"ProcessName\":\"{0}\",{1}", _ProcessName, Environment.NewLine);
            logger = logger + string.Format("   \"ProcessDescription\":\"{0}\",{1}", _ProcessDescription, Environment.NewLine);
            logger = logger + string.Format("   \"ProcessType\":\"{0}\",{1}", _ProcessType, Environment.NewLine);
            logger = logger + string.Format("   \"ProcessLogs\":[{0}", Environment.NewLine);

            foreach (var item in _Logging)
            {
                string aux_item = $"{{{item}}}";
                logger = logger + aux_item + "," + Environment.NewLine;
            }
            logger = logger.Substring(0, logger.LastIndexOf(',')) + Environment.NewLine;

            logger = logger + string.Format("]{0}", Environment.NewLine);
            logger = logger + "}" + Environment.NewLine;
            logger = logger + "}";
            return logger;
        }

    }

}
