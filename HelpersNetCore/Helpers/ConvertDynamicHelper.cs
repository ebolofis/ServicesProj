using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HitHelpersNetCore.Helpers
{
    public class ConvertDynamicHelper
    {
        /// <summary>
        /// Instance for mapper
        /// </summary>
        private readonly IMapper mapper;

        public ConvertDynamicHelper(IMapper _mapper)
        {
            mapper = _mapper;
        }

        /// <summary>
        /// Convert a IEnumerable(dynamic) to List(IDictionary(string, dynamic)) [Required if you want to save data to a file]
        /// </summary>
        /// <param name="list">a dynamic IEnumerable  (see RunSelect)</param>
        /// <returns></returns>
        public List<IDictionary<string, dynamic>> ToListDictionary(IEnumerable<dynamic> list)
        {
            List<IDictionary<string, dynamic>> results = new List<IDictionary<string, dynamic>>();
            foreach (var item in list.ToList())
            {
                results.Add(mapper.Map<IDictionary<string, dynamic>>(item));
            }
            return results;
        }

        /// <summary>
        /// Convert a dynamic object to IDictionary(string, dynamic) 
        /// </summary>
        /// <param name="obj">dynamic object</param>
        /// <param name="header">the list containing the names of the desired properties (keys). If null then keep the original names</param>
        /// <returns></returns>
        public IDictionary<string, dynamic> ToDictionary(dynamic obj, List<string> header = null)
        {

            IDictionary<string, dynamic> data = mapper.Map<IDictionary<string, dynamic>>(obj);

            if (header != null)
            {
                int i = 0;
                List<string> keys = data.Keys.ToList();
                IDictionary<string, dynamic> result = new Dictionary<string, dynamic>();
                foreach (string item in header)
                {
                    result.Add(item, data[keys[i]]);
                    i++;
                }
                return result;
            }
            else
                return data;
        }

        /// <summary>
        /// Convert a IEnumerable(dynamic) to List of a Model
        /// </summary>
        /// <param name="list">a dynamic IEnumerable  (see RunSelect)</param>
        /// <returns></returns>
        public List<T> ToModelList<T>(IEnumerable<dynamic> list)
        {
            return mapper.Map<List<T>>(list);
        }
    }
}
