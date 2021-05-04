using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caucedo.Base.Oracle.DAL;

namespace Caucedo.LoyaltyProgram.BLL
{
    public static class ConnStrs
    {
        public static string N4 { get { return "N4"; } }
    }
    public static class BllOracle
    {
        public static List<ChoferesN4> GetDriversVisits(string SDate, string EDate)
        {
            var para = SearchRec.ToSqlParams(
                                    new
                                    {
                                        startDate = SDate,
                                        endDate = EDate,
                                    });
            var affected = Database.AdHoc(ConnStrs.N4)
                                      .ExecReaderSelSP("PKG_COMMON.GET_DRIVERS_VISITS", para).ToList<ChoferesN4>();
            return affected;
        }

    }
}
