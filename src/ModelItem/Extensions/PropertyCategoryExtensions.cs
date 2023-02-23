using System.Collections.Generic;
using System.Linq;
using Autodesk.Navisworks.Api;

namespace PedramElmi.Navisworks.Toolkit
{
    public static class PropertyCategoryExtensions
    {
        /// <summary>
        /// Returns the categories DisplayName
        /// </summary>
        /// <param name="categories">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<string> GetCategoriesDisplaName(this IEnumerable<PropertyCategory> categories)
        {
            return (from category in categories
                    select category.DisplayName).Distinct();
        }
        
        /// <summary>
        /// Returns the properties display name
        /// </summary>
        /// <param name="category">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<string> GetPropertiesDisplayName(this PropertyCategory category)
        {
            return from property in category.Properties select property.DisplayName;
        }

        public static IDictionary<string, IDictionary<string, object>> ToDictionary(this IEnumerable<PropertyCategory> categories)
        {
            var dictionary = new Dictionary<string, IDictionary<string, object>>();
            foreach (var category in categories)
            {
                dictionary.Add(category.DisplayName, category.Properties.ToDictionary());
            }
            return dictionary;
        }
    }
}