using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caucedo.ActiveDirectoryServices.Entities
{
    public class Group
    {
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public bool? IsSecurityGroup { get; set; }
        public string Context { get; set; }
        public int? Scope { get; set; }
    }



    public enum FilterType
    { 
        LogonName = 1,
        DisplayName = 2

    }
}
