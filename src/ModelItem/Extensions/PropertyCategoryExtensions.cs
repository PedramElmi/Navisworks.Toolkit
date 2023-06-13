using System.Collections.Generic;
using System.Linq;
using Autodesk.Navisworks.Api;
using Community.Navisworks.Toolkit.Helper;

namespace Community.Navisworks.Toolkit
{
    public static class PropertyCategoryExtensions
    {
        /// <summary>
        /// Extension method that returns the distinct display names of the given PropertyCategory objects.
        /// </summary>
        /// <param name="categories">The collection of PropertyCategory objects.</param>
        /// <returns>The distinct display names of the PropertyCategory objects.</returns>
        public static IEnumerable<string> GetCategoriesDisplayName(this IEnumerable<PropertyCategory> categories)
        {
            return categories.Select(category => category.DisplayName).Distinct();
        }

        /// <summary>
        /// Extension method that returns the display names of the properties in the given PropertyCategory object.
        /// </summary>
        /// <param name="category">The PropertyCategory object.</param>
        /// <returns>The display names of the properties in the PropertyCategory object.</returns>
        public static IEnumerable<string> GetPropertiesDisplayName(this PropertyCategory category)
        {
            return category.Properties.Select(property => property.DisplayName);
        }

        /// <summary>
        /// Extension method that converts a collection of PropertyCategory objects to a dictionary.
        /// </summary>
        /// <param name="categories">The collection of PropertyCategory objects to be converted.</param>
        /// <returns>A dictionary containing the display names and properties of each PropertyCategory object.</returns>
        public static IDictionary<string, object> ToDictionary(this IEnumerable<PropertyCategory> categories)
        {
            var dictionary = new Dictionary<string, object>();
            // Iterate through each PropertyCategory object in the collection
            foreach(var category in categories)
            {
                // Insert the display name and properties of the current PropertyCategory object into the dictionary
                dictionary.Insert(category.DisplayName, category.Properties.ToDictionary());
            }
            // Return the resulting dictionary
            return dictionary;
        }

    }
}