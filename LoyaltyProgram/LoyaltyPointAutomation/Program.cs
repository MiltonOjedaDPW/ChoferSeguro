using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caucedo.LoyaltyProgram.BLL;
using Caucedo.Base.Common;
using System.Globalization;

namespace LoyaltyPointAutomation
{
    class Program
    {
        readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            Console.Clear();
            //DateTime? date1 = new DateTime(2020, 01, 01);
            //DateTime? date2 = new DateTime(2020, 06, 30);
            DateTime? date1 = new DateTime(2020, 10, 20);//0801
            DateTime? date2 = new DateTime(2020, 10, 21);
            try//0707-0708//811-812 //903-904 //925-926  //1007-1008 // 1016-1017 //1019-1020
            {
                var sss = args;
                Console.WriteLine("..Working...\n");
                logger.Info($"..working...");
                if (args.Length != 0)
                {
                    date1 = DateTime.ParseExact(args[0], "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture);
                    logger.Info($"Start date {args[0]}");
                    date2 = DateTime.ParseExact(args[1], "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture);
                    logger.Info($"Start date {args[1]}");
                }
                //"09-oct-19 00:00:00" "09-oct-19 23:59:59"
                GenerarPuntosDiario(date1, date2);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }
        private static bool IsAllDigits(string s)
        {
            return s.All(char.IsDigit);
        }

        private static void GenerarPuntosDiario(DateTime? fromDate, DateTime? toDate)
        {
            var Sdate = (fromDate.HasValue ? fromDate.Value : DateTime.Today.AddDays(-1).DayStart()).ToString("dd-MMM-yy HH:mm:ss");
            var Edate = (fromDate.HasValue ? toDate.Value : DateTime.Today.AddDays(-1).EndOfDay()).ToString("dd-MMM-yy HH:mm:ss");

            Console.WriteLine($"Start of the day : {Sdate} \nEnd of the day: {Edate}");
            logger.Info($"Start of the day : {Sdate} | End of the day: {Edate}");
            var result = BllOracle.GetDriversVisits(Sdate, Edate);

            var list = result.Where(a => IsAllDigits(a.DRIVER_CARD_ID.Trim()) == true).ToList();
            Console.WriteLine($"Total Visits: {list.Count}");
            logger.Info($"Total Visits: {list.Count}");
            var choferesAgrupados = list.GroupBy(info => info.DRIVER_CARD_ID)
                    .Select(group => new ChoferesToSQL
                    {
                        RNTT = Convert.ToInt32(group.Key),
                        VISITS = group.Count()
                    })
                    .OrderBy(x => x.VISITS).ToList();

            var choferes = Bll.GetChoferes();

            var choferesExistentes = choferesAgrupados.Where(r2 =>
                choferes.Any(r1 => r1.RNTT == r2.RNTT.ToString())).ToList();


            var choferesNoExistentes = choferesAgrupados.Except(choferesExistentes).ToList();

            Console.WriteLine($"Total visiting drivers: {choferesAgrupados.Count}");
            logger.Info($"Total visiting drivers: {choferesAgrupados.Count}");
            if (choferesAgrupados.Count() != 0)
            {
                var rpt = 0;
                if (choferesNoExistentes.Count() >= 0)
                {
                    rpt = Bll.InsertPointAutomation(choferesAgrupados);
                }
                //if (choferesNoExistentes.Count() != 0)
                //{
                //    var result_ = BllOracle.GetDriversVisits("01-JAN-2020 0:0:01", "30-JUN-2020 23:59:59");
                //    var list_ = result_.Where(a => IsAllDigits(a.DRIVER_CARD_ID.Trim()) == true).ToList();
                //    var choferesAgrupados_ = list_.GroupBy(info => info.DRIVER_CARD_ID)
                //        .Select(group => new ChoferesToSQL
                //        {
                //            RNTT = Convert.ToInt32(group.Key),
                //            VISITS = group.Count()
                //        })
                //        .OrderBy(x => x.VISITS).ToList();

                //    var choferesExistentes_ = choferesAgrupados_.Where(r2 =>
                //    choferesNoExistentes.Any(r1 => r1.RNTT == r2.RNTT)).ToList();
                //    var rpts = Bll.InsertPointAutomation(choferesExistentes_);
                ////}

                Console.WriteLine($"\nUpdated Driver Points: {rpt - choferesNoExistentes.Count()} drivers");
                logger.Info($"Updated Driver Points: {rpt - choferesNoExistentes.Count()} drivers");

                Console.WriteLine($"\nNew Drivers: {choferesNoExistentes.Count()} drivers");
                logger.Info($"New Drivers: {choferesNoExistentes.Count()} drivers");

                Console.WriteLine($"\nNew Drivers with updated points:: {choferesNoExistentes.Count()} drivers");
                logger.Info($"New Drivers with updated points: {choferesNoExistentes.Count()} drivers");

            }
        }

    }
}
