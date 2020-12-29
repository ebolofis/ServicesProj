using HitHelpersNetCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HitHelpersNetCore.Classes
{
    public class XMLHelper : IXMLHelper
    {
        /// <summary>
        /// convert xml string to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public T fromXML<T>(string xml) where T : class
        {
            using (var stringReader = new System.IO.StringReader(xml))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                return serializer.Deserialize(stringReader) as T;
            }
        }

        /// <summary>
        /// convert object to xml string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="replaceNulls">true: remove nill elements</param>
        /// <returns></returns>
        public string toXML<T>(T obj, bool replaceNulls = true)
        {
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                serializer.Serialize(stringwriter, obj);
                string xml = stringwriter.ToString();

                // Replace all nullable fields
                if (replaceNulls)
                    xml = Regex.Replace(xml, $"\\s+<\\w+ xsi:nil=\"true\" \\/>", string.Empty);
                return xml;
            }
        }
    }
}
