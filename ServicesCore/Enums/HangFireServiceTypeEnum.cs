using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HitServicesCore.Enums
{
    public enum HangFireServiceTypeEnum
    {
        Plugin = 0,
        ExportData = 1,
        SqlScripts = 2,
        SaveToTable = 3,
        ReadFromCsv = 4
    }
}
