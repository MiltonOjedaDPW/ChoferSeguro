using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caucedo.Base.Common
{
    public enum ISOContainerType
    {
        General, OpenTop, Tank,
        Bulk,
        Flat,
        Reefer,
        Insulated,
        Named,
        Ventilated
    }
    public enum ContainerSize
    {
        Unknown = 0,
        Dry_20 = 20, Dry_40 = 40, Dry_45 = 45,
        Reefer_20 = -20, Reefer_40 = -40, Reefer_45 = -45
    }
}
