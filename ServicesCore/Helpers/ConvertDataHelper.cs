using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HitServicesCore.Helpers
{
    public class ConvertDataHelper
    {

        /// <summary>
        /// contains info about the formating of data fields. Keys are the names of data columns 
        /// (ex: Key="DateFrom" - Value={"Format":"yyyyMMdd","Format":"el-GR"},   Key="Net" - Value={"Format":"yyyyMMdd","Format":"F2"}). 
        /// (see: ToString method below.)
        /// </summary>
        private Dictionary<string, Formater> formater;

        public ConvertDataHelper()
        {
            formater = new Dictionary<string, Formater>();
        }

        /// <summary>
        /// Constractor: Create formaters for columns
        /// </summary>
        /// <param name="formater"></param>
        public ConvertDataHelper(Dictionary<string, Formater> formater)
        {
            this.formater = formater;
            if (this.formater == null) this.formater = new Dictionary<string, Formater>();
        }

        /// <summary>
        /// Convert Data to xml
        /// </summary>
        /// <param name="data">list of data (Every data row must be as key-value pairs, see RunSQLScriptsDT.ReturnDictionaries)</param>
        /// <param name="RootElement">the name of the root element</param>
        /// <param name="element">the name of every line's element</param>
        /// <returns>xml as string</returns>
        public string ToXml(List<IDictionary<string, dynamic>> data, string RootElement, string element, IMapper mapper)
        {


            XElement rootEl = new XElement(RootElement);

            foreach (var item in data)
            {
                XElement el = new XElement(element, item.Select(kv => createXmlElement(kv.Key, kv.Value, mapper)));
                rootEl.Add(el);
            }

            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + rootEl.ToString();
        }

        /// <summary>
        /// create sub-elements into an xml file
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private XElement createXmlElement(string key, dynamic value, IMapper mapper)
        {
            IEnumerable<dynamic> list = value as IEnumerable<dynamic>;
            if (list == null)
            {
                return new XElement(key, ToString(key, value, true));
            }
            else
            {
                XElement el = new XElement(key + "List");
                foreach (var item in list)
                {
                    XElement elnested = new XElement(key);
                    IDictionary<string, dynamic> members = mapper.Map<IDictionary<string, dynamic>>(item);
                    foreach (var keymember in members.Keys)
                    {
                        XElement elnested2 = createXmlElement(keymember, members[keymember], mapper);
                        elnested.Add(elnested2);
                    }
                    el.Add(elnested);
                }
                return el;
            }
        }

        /// <summary>
        /// Convert Data to json
        /// </summary>
        /// <param name="data">list of data (Every data row must be as key-value pairs, see RunSQLScriptsDT.ReturnDictionaries)</param>
        /// <returns>json as string</returns>
        public string ToJson(List<IDictionary<string, dynamic>> data)
        {
            return System.Text.Json.JsonSerializer.Serialize(data);
        }

        /// <summary>
        /// Convert Data to csv
        /// </summary>
        /// <param name="data">list of data (Every data row must be as key-value pairs, see RunSQLScriptsDT.ReturnDictionaries)</param>
        /// <param name="header">true if file will contain header</param>
        /// <param name="delimeter">the delimeter (for tab use: \t)</param>
        /// <returns>csv as string</returns>
        public string ToCsv(List<IDictionary<string, dynamic>> data, bool header, string delimeter = ";")
        {
            StringBuilder file = new StringBuilder((data.Count() + 1) * 400);
            var items = data[0].Keys.Count(); // items per line count
            if (header)
            {
                var k = 0;
                // write header
                foreach (string key in data[0].Keys)
                {
                    file.Append(key.ToString());
                    if (++k != items) file.Append(delimeter);
                }
                file.AppendLine();
            }


            //  write data
            var i = 0;                        // lines index
            var lines = data.Count();         // lines count
            foreach (var item in data)
            {
                var j = 0;                    // items per line index
                foreach (string key in item.Keys)
                {
                    file.Append(ToString(key, item[key]));
                    if (++j != items) file.Append(delimeter); //this is not the last line
                }
                if (++i != lines) file.AppendLine();//this is not the last line
            }

            return file.ToString();
        }

        /// <summary>
        /// Convert Data to Fixed Lenght
        /// </summary>
        /// <param name="data">list of data (Every data row must be as key-value pairs, see RunSQLScriptsDT.ReturnDictionaries)</param>
        /// <param name="header">true if file will contain header</param>
        /// <param name="lengths">the list of lenghts that every column must have. ex: (10,20,5,10,...)</param>
        /// <param name="alignRight">true: values will be aligned to the right</param>
        /// <returns>csv as string</returns>
        public string ToFixedLenght(List<IDictionary<string, dynamic>> data, bool header, List<int?> lengths, bool alignRight)
        {
            StringBuilder file = new StringBuilder((data.Count() + 1) * 400);
            var items = data[0].Keys.Count(); // items per line count
            if (header)
            {
                var k = 0;
                // write header
                if (lengths != null)
                {
                    foreach (string key in data[0].Keys)
                    {
                        if (alignRight)
                            file.Append(key.PadLeft(lengths[k].Value));
                        else
                            file.Append(key.PadRight(lengths[k].Value));
                        k++;
                    }
                }
                file.AppendLine();
            }

            //  write data
            var i = 0;                        // lines index
            var lines = data.Count();         // lines count
            foreach (var item in data)
            {
                var j = 0;                    // items per line index
                foreach (string key in item.Keys)
                {
                    string value = ToString(key, item[key]);
                    if (alignRight)
                        file.Append(value.PadLeft(lengths[j].Value));
                    else
                        file.Append(value.PadRight(lengths[j].Value));
                    j++;
                }
                if (++i != lines) file.AppendLine();//this is not the last line
            }

            return file.ToString();
        }

        /// <summary>
        /// Convert Data to html 
        /// </summary>
        /// <param name="data">list of data (Every data row must be as key-value pairs, see RunSQLScriptsDT.ReturnDictionaries)</param>
        /// <param name="header">true if file will contain header</param>
        /// <param name="title">html's title</param>
        /// <param name="sortColumns">if true then add javascript for sorting columns</param>
        /// <param name="css">css classes for th, tr, td</param>
        /// <returns>csv as string</returns>
        public string ToHtml(List<IDictionary<string, dynamic>> data, bool header, string title, bool sortColumns, string css = "")
        {
            string startRow;
            if (header)
                startRow = "2";
            else
                startRow = "1";

            StringBuilder file = new StringBuilder((data.Count() + 1) * 500);
            file.Append(@"<!DOCTYPE html>
            <html>

               <head>
            <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
                            <meta charset = ""utf-8"" />
            <style type=""text/css"">
            " + css +
            @"
                </style>
         
               <title>" + title + @"</title>
               </head>
	
               <body>
                <div style='overflow-x:auto;'>
                   <table id='hittable'>");

            var items = data[0].Keys.Count(); // items per line count
            if (header)
            {
                var k = 0;
                file.Append("<tr> <th style='text-align: center;' colspan='" + data[0].Keys.Count.ToString() + "' > " + title + " </th></tr> ");
                file.Append("<tr>");

                // write header
                foreach (string key in data[0].Keys)
                {
                    file.Append("<th ");
                    if (sortColumns) file.Append(" onclick='sortTable(" + k.ToString() + ")' ");
                    file.Append("> " + key.ToString() + "</th>");
                    k++;
                }
                file.AppendLine("</tr>");
            }

            //  write data
            var i = 0;                        // lines index
            var lines = data.Count();         // lines count
            foreach (var item in data)
            {
                var j = 0;
                file.Append("<tr id='L" + i.ToString() + "'>");
                // items per line index
                foreach (string key in item.Keys)
                {
                    file.Append("<td id='" + i.ToString() + j.ToString() + "' class='td" + j.ToString() + "'>" + ToString(key, item[key]) + "</td>");
                    j++;
                }
                file.AppendLine("</tr>");
                if (++i != lines) file.AppendLine();//this is not the last line
            }

            file.Append(@"
                </table>
              </div>");
            if (sortColumns)
            {
                file.Append(@"
                <script>
                function sortTable(n) {
                  var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;
                  table = document.getElementById(""hittable"");
                  switching = true;
                                // Set the sorting direction to ascending:
                                dir = ""asc"";
                                /* Make a loop that will continue until
                                no switching has been done: */
                                while (switching)
                                {
                                    // Start by saying: no switching is done:
                                    switching = false;
                                    rows = table.getElementsByTagName(""TR"");
                                    /* Loop through all table rows (except the
                                    first, which contains table headers): */
                                    for (i = " + startRow + @"; i < (rows.length - 1); i++)
                                    {
                                       // Start by saying there should be no switching:
                                        shouldSwitch = false;
                                        /* Get the two elements you want to compare,
                                        one from current row and one from the next: */
                                        x = rows[i].getElementsByTagName(""TD"")[n];
                                        y = rows[i + 1].getElementsByTagName(""TD"")[n];
                                        if (isNaN(x.innerHTML))
                                        {
                                            /* Check if the two rows should switch place,
                                            based on the direction, asc or desc: */
                                            if (dir == ""asc"")
                                            {
                                                if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase())
                                                {
                                                    // If so, mark as a switch and break the loop:
                                                    shouldSwitch = true;
                                                    break;
                                                }
                                            }
                                            else if (dir == ""desc"")
                                            {
                                                if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase())
                                                {
                                                    // If so, mark as a switch and break the loop:
                                                    shouldSwitch = true;
                                                    break;
                                                }
                                            }
                                        }
                                        else   //x is number
                                        {
                                            /* Check if the two rows should switch place,
                                            based on the direction, asc or desc: */
                                            if (dir == ""asc"")
                                            {
                                                if (Number(x.innerHTML) > Number(y.innerHTML))
                                                {
                                                    // If so, mark as a switch and break the loop:
                                                    shouldSwitch = true;
                                                    break;
                                                }
                                            }
                                            else if (dir == ""desc"")
                                            {
                                                if (Number(x.innerHTML) < Number(y.innerHTML))
                                                {
                                                    // If so, mark as a switch and break the loop:
                                                    shouldSwitch = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (shouldSwitch)
                                    {
                                        /* If a switch has been marked, make the switch
                                        and mark that a switch has been done: */
                                        rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
                                        switching = true;
                                        // Each time a switch is done, increase this count by 1:
                                        switchcount++;
                                    }
                                    else
                                    {
                                        /* If no switching has been done AND the direction is ""asc"",
                                        set the direction to ""desc"" and run the while loop again. */
                                        if (switchcount == 0 && dir == ""asc"")
                                        {
                                            dir = ""desc"";
                                            switching = true;
                                        }
                                    }
                                }
                            }
                </script>
            ");
            }
            file.Append(@" </body>
       </html>");
            return file.ToString();
        }

        /// <summary>
        /// convert dynamic values to string
        /// </summary>
        /// <param name="key">dictionary's key</param>
        /// <param name="value">dictionary's value</param>
        /// <param name="returnNull">
        ///  true : in case of null value return null,  
        ///  false: in case of null value return "" </param>
        /// <returns>the (formated) string</returns>
        private string ToString(string key, dynamic value, bool returnNull = false)
        {
            if (value == null && !returnNull)
                return "";
            else if (value == null && returnNull)
                return null;
            else
            {
                if (!formater.ContainsKey(key))
                    return value.ToString();
                else
                    return value.ToString(formater[key].Format, formater[key].CultureInfo);
            }
        }
    }




    /// <summary>
    /// Class containing info about a field's formating
    /// </summary>
    public class Formater
    {
        /// <summary>
        /// format string. ex: "yyMMdd": 141103, "F2": 1230.12, "N2": 1,230.12
        /// </summary>
        public string Format { get; set; } = "";

        /// <summary>
        /// Culture Info. ex: "el-GR", "en-us", null. 
        /// If CultureInfoDetails==null then uses CultureInfo.InvariantCulture
        /// </summary>
        public string CultureInfoDescription { get; set; }

        /// <summary>
        /// Return the CultureInfo based the value from CultureInfoDetails 
        /// (If CultureInfoDetails==null then uses CultureInfo.InvariantCulture)
        /// </summary>
        public CultureInfo CultureInfo
        {
            get
            {
                if (string.IsNullOrEmpty(CultureInfoDescription))
                    return CultureInfo.InvariantCulture;
                else
                    return CultureInfo.CreateSpecificCulture(CultureInfoDescription);

            }
        }
    }
}

