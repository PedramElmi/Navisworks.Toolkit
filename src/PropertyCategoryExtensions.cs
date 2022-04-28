using PedramElmi.Navisworks.Toolkit.ModelItem;
using System.Collections.Generic;
using Api = Autodesk.Navisworks.Api;

namespace PedramElmi.Navisworks.Toolkit
{
    public static class PropertyCategoryExtensions
    {
        #region Public Methods

        /// <summary>
        /// Returns the categories display name of categories
        /// </summary>
        /// <param name="categories">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<string> GetCategoriesDisplaName(this Api.PropertyCategoryCollection categories)
        {
            return CategoriesProperties.GetCategoriesDisplaName(categories);
        }

        #endregion Public Methods
    }
}