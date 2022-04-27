using PedramElmi.Navisworks.Toolkit.ModelItem;
using System.Collections.Generic;
using Api = Autodesk.Navisworks.Api;

namespace PedramElmi.Navisworks.Toolkit
{
    public static class ModelItemExtensions
    {
        #region Public Methods

        /// <summary>
        /// Add a custom Category Property (User-Defined). If this category exists, it will
        /// overwrites the new value of properties and remains the existing properties
        /// </summary>
        /// <param name="modelItem"></param>
        /// <param name="category">This category and its properties will be added to this ModelItem</param>
        public static void AddCustomPropertyCategory(this Api.ModelItem modelItem, CustomPropertyCategory category)
        {
            var modelItems = new Api.ModelItemCollection()
            {
                modelItem
            };
            modelItems.AddCustomPropertyCategory(category);
        }

        /// <summary>
        /// Gets the icon type that it is on the visual tree.
        /// </summary>
        /// <param name="modelItem"></param>
        /// <returns></returns>
        public static IconType GetIconType(this Api.ModelItem modelItem)
        {
            return CategoriesProperties.GetIconType(modelItem);
        }

        /// <summary>
        /// Returns the properties display name
        /// </summary>
        /// <param name="modelItem"></param>
        /// <param name="categoryDisplayName"></param>
        /// <returns></returns>
        public static HashSet<string> GetPropertiesDisplayName(this Api.ModelItem modelItem, string categoryDisplayName)
        {
            return CategoriesProperties.GetPropertiesDisplayName(modelItem, categoryDisplayName);
        }

        #endregion Public Methods
    }
}