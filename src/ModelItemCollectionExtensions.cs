using PedramElmi.Navisworks.Toolkit.ModelItem;
using System.Collections.Generic;
using System.Text;
using Api = Autodesk.Navisworks.Api;

namespace PedramElmi.Navisworks.Toolkit
{
    public static class ModelItemCollectionExtensions
    {
        #region Public Methods

        /// <summary>
        /// Add a custom Category Property (User-Defined). If this category exists, it will
        /// overwrites the new value of properties and remains the existing properties
        /// </summary>
        /// <param name="modelItems">
        /// </param>
        /// <param name="category">
        /// This category and its properties will be added to these ModelItems
        /// </param>
        public static void AddCustomPropertyCategory(this Api.ModelItemCollection modelItems, CustomPropertyCategory category)
        {
            category.AddToModelItems(modelItems);
        }

        /// <summary>
        /// Returns the intersected of category names
        /// </summary>
        /// <param name="modelItems">
        /// </param>
        /// <returns>
        /// </returns>
        public static HashSet<string> GetIntersectedCategoriesDisplayName(this IEnumerable<Api.ModelItem> modelItems)
        {
            return CategoriesProperties.GetIntersectedCategoriesDisplayName(modelItems);
        }

        /// <summary>
        /// Returns the intersected of property names
        /// </summary>
        /// <param name="modelItems">
        /// </param>
        /// <param name="categoryDisplayName">
        /// </param>
        /// <returns>
        /// </returns>
        public static HashSet<string> GetIntersectedPropertiesDisplayName(this IEnumerable<Api.ModelItem> modelItems, string categoryDisplayName)
        {
            return CategoriesProperties.GetIntersectedPropertiesDisplayName(modelItems, categoryDisplayName);
        }

        /// <summary>
        /// Serialize the ModelItems PropertyCategories in a JSON format
        /// </summary>
        /// <param name="modelItems">
        /// ModelItems
        /// </param>
        /// <param name="sortAlphabetically">
        /// True: Sorted, False: Unsorted
        /// </param>
        /// <param name="indentedFormat">
        /// True: Indented, False: Unindented
        /// </param>
        /// <param name="namingStrategy">
        /// Default, CamelCase, KebabCase, SnakeCase
        /// </param>
        /// <returns>
        /// JSON Formated of the ModelItems
        /// </returns>
        public static StringBuilder JsonSerialize(this Api.ModelItemCollection modelItems, bool sortAlphabetically = false, bool indentedFormat = false, NamingStrategy namingStrategy = NamingStrategy.Default)
        {
            return CategoriesProperties.SerializeModelItems(modelItems, sortAlphabetically, indentedFormat, namingStrategy);
        }

        /// <summary>
        /// Serialize the ModelItems PropertyCategories in a JSON format and save it in a .json text file
        /// </summary>
        /// <param name="modelItems">
        /// ModelItems
        /// </param>
        /// <param name="filePath">
        /// file path to save it on a .json text file
        /// </param>
        /// <param name="sortAlphabetically">
        /// True: Sorted, False: Unsorted
        /// </param>
        /// <param name="indentedFormat">
        /// True: Indented, False: Unindented
        /// </param>
        /// <param name="namingStrategy">
        /// Default, CamelCase, KebabCase, SnakeCase
        /// </param>
        public static void JsonSerialize(this Api.ModelItemCollection modelItems, string filePath, bool sortAlphabetically = false, bool indentedFormat = false, NamingStrategy namingStrategy = NamingStrategy.Default)
        {
            CategoriesProperties.SerializeModelItems(modelItems, filePath, sortAlphabetically, indentedFormat, namingStrategy);
        }

        #endregion Public Methods
    }
}