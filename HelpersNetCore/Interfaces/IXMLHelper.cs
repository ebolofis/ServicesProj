using System;
using System.Collections.Generic;
using System.Text;

namespace HitHelpersNetCore.Interfaces
{
    public interface IXMLHelper
    {
        /// <summary>
        /// convert object to xml string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="replaceNulls">true: remove nill elements</param>
        /// <returns></returns>
        string toXML<T>(T obj, bool replaceNulls = true);


        /// <summary>
        /// convert xml string to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        T fromXML<T>(string xml) where T : class;
    }
}
