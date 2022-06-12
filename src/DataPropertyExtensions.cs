using Autodesk.Navisworks.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedramElmi.Navisworks.Toolkit
{
    public static class DataPropertyExtensions
    {
        public static IEnumerable<string> GetPropertiesDisplayName(this IEnumerable<DataProperty> properties)
        {
            return from property in properties select property.DisplayName;
        }
    }
}
