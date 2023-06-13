using Autodesk.Navisworks.Api;
using Community.Navisworks.Toolkit.Helper;
using System.Collections.Generic;
using System.Linq;

namespace Community.Navisworks.Toolkit
{
    public static class DataPropertyExtensions
    {
        /// <summary>
        /// Returns an IEnumerable of display names for the given DataProperty collection.
        /// </summary>
        /// <param name="properties">
        /// The collection of DataProperties to retrieve display names from.
        /// </param>
        /// <returns>
        /// An IEnumerable of display names.
        /// </returns>
        public static IEnumerable<string> GetPropertiesDisplayName(this IEnumerable<DataProperty> properties)
        {
            return from property in properties select property.DisplayName;
        }

        /// <summary>
        /// Converts a collection of DataProperties to a Dictionary with display names as keys and property values as values.
        /// </summary>
        /// <param name="properties">
        /// The collection of DataProperties to convert to a Dictionary.
        /// </param>
        /// <returns>
        /// A Dictionary with display names as keys and property values as values.
        /// </returns>
        public static IDictionary<string, object> ToDictionary(this IEnumerable<DataProperty> properties)
        {
            var dictionary = new Dictionary<string, object>();
            foreach(var property in properties)
            {
                dictionary.Insert(property.DisplayName, property.Value.GetValue());
            }
            return dictionary;
        }
    }
}