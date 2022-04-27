using PedramElmi.Navisworks.Toolkit.ModelItem;
using System.Collections.Generic;
using Api = Autodesk.Navisworks.Api;

namespace PedramElmi.Navisworks.Toolkit
{
    public static class PropertyCategoryCollectionExtensions
    {
        #region Public Methods

        /// <summary>
        /// Returns the properties display name
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static HashSet<string> GetPropertiesDisplayName(this Api.PropertyCategory category)
        {
            return CategoriesProperties.GetPropertiesDisplayName(category);
        }

        #endregion Public Methods
    }
}