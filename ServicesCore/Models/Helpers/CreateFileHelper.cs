﻿using HitServicesCore.Models.IS_Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HitServicesCore.Models.Helpers
{
    public class CreateFileHelper
    {
        private SystemInfo sysinfo;
        public CreateFileHelper ( SystemInfo _sysinfo)
        {
            sysinfo = _sysinfo;
        }

        public void CreateSqlScriptFile(ISRunSqlScriptsModel data, string fileName)
        {
            string isServicePath = Path.Combine(new string[] { sysinfo.rootPath, "IS_Services", "SqlScripts" });
            string jsonString = JsonSerializer.Serialize(data);
            File.WriteAllText(isServicePath + "\\"+ fileName + ".json", jsonString, Encoding.Default);
        }
        public void CreateSaveToTableFile(ISSaveToTableModel data, string fileName)
        {
            string isServicePath = Path.Combine(new string[] { sysinfo.rootPath, "IS_Services", "SaveToTable" });
            string jsonString = JsonSerializer.Serialize(data);
            File.WriteAllText(isServicePath + "\\" + fileName + ".json", jsonString, Encoding.Default);
        }

        public void CreateReadCsvFile(ISReadFromCsvModel data, string fileName)
        {
            string isServicePath = Path.Combine(new string[] { sysinfo.rootPath, "IS_Services", "ReadCsv" });
            string jsonString = JsonSerializer.Serialize(data);
            File.WriteAllText(isServicePath + "\\" + fileName + ".json", jsonString, Encoding.Default);
        }
        public void CreateExportDataFile(ISExportDataModel data, string fileName)
        {
            string isServicePath = Path.Combine(new string[] { sysinfo.rootPath, "IS_Services", "ExportData" });
            string jsonString = JsonSerializer.Serialize(data);
            File.WriteAllText(isServicePath + "\\" + fileName + ".json", jsonString, Encoding.Default);
        }
    }
}
