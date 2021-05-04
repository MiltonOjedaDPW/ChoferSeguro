using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caucedo.ActiveDirectoryServices.Entities
{
    public class Computer
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string DNSHostName { get; set; }
    }
}
