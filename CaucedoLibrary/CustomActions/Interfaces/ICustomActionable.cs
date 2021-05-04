using Caucedo.Base.SqlSvr.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caucedo.Base.Common.CustomActions.Interfaces
{
    public interface ICustomActionable
    {
        void SetParameters(dynamic param1, dynamic param2, dynamic param3, dynamic param4, dynamic param5, dynamic param6, dynamic param7);
        //void SetParameters();

        string OnExecute();
       
       
    }
}


/*
 * 
 * 
 * 
 *  StringBuilder OnExecute(string StringParam);
        StringBuilder OnExecute(IEnumerable<string> ListStringParams);
        StringBuilder OnExecute(IEnumerable<string> ListStringParams, Database database);
        StringBuilder OnExecute(IEnumerable<string> ListStringParams,Database database1, Database database2);
        StringBuilder OnExecute(IEnumerable<string> ListStringParams, Database database1, Database database2,Database database3);
        StringBuilder OnExecute(List<Common.Models.Employee> employees, Database database1, Database database2);


       void SetParameters(List<Common.Models.Employee> employees);
        void SetParameters(List<string> paramlistString);
        void SetParameters(string paramString);

 * 
 * 
 * 
 * 
 */
