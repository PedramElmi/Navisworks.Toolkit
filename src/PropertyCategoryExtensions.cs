using PedramElmi.Navisworks.Toolkit.ModelItem;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Navisworks.Api;

namespace PedramElmi.Navisworks.Toolkit
{
    public static class PropertyCategoryExtensions
    {
        #region Public Methods

        /// <summary>
        /// Returns the categories display name
        /// </summary>
        /// <param name="categories">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<string> GetCategoriesDisplaName(this IEnumerable<PropertyCategory> categories)
        {
            return CategoriesProperties.GetCategoriesDisplaName(categories);
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
            return CategoriesProperties.GetPropertiesDisplayName(category);
        }

        #endregion Public Methods
    }
}