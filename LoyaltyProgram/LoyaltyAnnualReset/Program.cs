using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caucedo.LoyaltyProgram.BLL;
using Caucedo.Base.Common;

namespace LoyaltyAnnualReset
{
    class Program
    {
        readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            Console.Clear();
            try
            {
                Console.WriteLine("..Working...\n");
                logger.Info($"..working...");
                var rpt = Bll.ResetPoint();
                logger.Info($"result = {rpt}");
                logger.Info($"..DONE..");   
            }
            catch (Exception ex)
            {
                logger.Error(ex); 
                Console.WriteLine($"Error: {ex.Message} ");
            }

        }
    }
}
