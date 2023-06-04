using Autodesk.Navisworks.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PedramElmi.Navisworks.Toolkit.Helper;

namespace PedramElmi.Navisworks.Toolkit
{
    public static class DataPropertyExtensions
    {
        #region DataProperty



        #endregion

        #region IEnumerable<DataProperty>

        public static IDictionary<string, object> ToDictionary(this IEnumerable<DataProperty> properties)
        {
            var dictionary = new Dictionary<string, object>();
            foreach(var property in properties)
            {
                dictionary.Insert(property.DisplayName, property.Value.GetValue());
            }
            return dictionary;
        }

        public static IEnumerable<string> GetPropertiesDisplayName(this IEnumerable<DataProperty> properties)
        {
            return from property in properties select property.DisplayName;
        }

        #endregion
    }
}
