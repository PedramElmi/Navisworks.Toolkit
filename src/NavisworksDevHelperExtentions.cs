using Autodesk.Navisworks.Api;
using NavisworksDevHelper.ModelItemHelpers;
using System.Text;

namespace NavisworksDevHelper
{
    public static class NavisworksDevHelperExtentions
    {
        #region Public Methods

        /// <summary>
        /// Add a custom Category Property (User-Defined). If this category exists, it will
        /// overwrites the new value of properties and remains the existing properties
        /// </summary>
        /// <param name="modelItem"></param>
        /// <param name="category">This category and its properties will be added to this ModelItem</param>
        public static void AddCustomPropertyCategory(this ModelItem modelItem, CustomPropertyCategory category)
        {
            var modelItems = new ModelItemCollection()
            {
                modelItem
            };
            modelItems.AddCustomPropertyCategory(category);
        }

        /// <summary>
        /// Add a custom Category Property (User-Defined). If this category exists, it will
        /// overwrites the new value of properties and remains the existing properties
        /// </summary>
        /// <param name="modelItems"></param>
        /// <param name="category">This category and its properties will be added to these ModelItems</param>
        public static void AddCustomPropertyCategory(this ModelItemCollection modelItems, CustomPropertyCategory category)
        {
            category.AddToModelItems(modelItems);
        }

        /// <summary>
        /// Casts the variant data to its basic data type (Use Object as a basic type)
        /// </summary>
        /// <typeparam name="T">NamedConstant, double, int, DateTime, string, object and etc</typeparam>
        /// <param name="variantData"></param>
        /// <returns></returns>
        public static T Cast<T>(this VariantData variantData)
        {
            var output = CategoriesPropertiesHelper.GetVariantData(variantData);
            return (T)output;
        }

        /// <summary>
        /// Returns the suitable string for displaying data of the VariantData class and Cleaned
        /// version of .ToString()
        /// </summary>
        /// <param name="variantData"></param>
        /// <returns></returns>
        public static string ToCleanedString(this VariantData variantData)
        {
            return CategoriesPropertiesHelper.GetCleanedString(variantData);
        }

        /// <summary>
        /// Gets the dynamic value based on its type
        /// </summary>
        /// <param name="variantData"></param>
        /// <returns>
        /// dynamic type that can be used as its type (i.e. NamedConstant, double, string etc...)
        /// available in Autodesk.Navisworks.Api.VariantDataType
        /// </returns>
        public static dynamic ToDynamic(this VariantData variantData)
        {
            return CategoriesPropertiesHelper.GetVariantData(variantData);
        }

        /// <summary>
        /// Gets the icon type that it is on the visual tree.
        /// </summary>
        /// <param name="modelItem"></param>
        /// <returns></returns>
        public static IconType GetIconType(this ModelItem modelItem)
        {
            return CategoriesPropertiesHelper.GetIconType(modelItem);
        }

        /// <summary>
        /// Serialize the ModelItems PropertyCategories in a JSON format
        /// </summary>
        /// <param name="modelItems">ModelItems</param>
        /// <param name="sortAlphabetically">True: Sorted, False: Unsorted</param>
        /// <param name="indentedFormat">True: Indented, False: Unindented</param>
        /// <param name="namingStrategy">Default, CamelCase, KebabCase, SnakeCase</param>
        /// <returns>JSON Formated of the ModelItems</returns>
        public static StringBuilder JsonSerialize(this ModelItemCollection modelItems, bool sortAlphabetically = false, bool indentedFormat = false, NamingStrategy namingStrategy = NamingStrategy.Default)
        {
            return CategoriesPropertiesHelper.SerializeModelItems(modelItems, sortAlphabetically, indentedFormat, namingStrategy);
        }

        /// <summary>
        ///  Serialize the ModelItems PropertyCategories in a JSON format and save it in a .json text file
        /// </summary>
        /// <param name="modelItems">ModelItems</param>
        /// <param name="filePath">file path to save it on a .json text file</param>
        /// <param name="sortAlphabetically">True: Sorted, False: Unsorted</param>
        /// <param name="indentedFormat">True: Indented, False: Unindented</param>
        /// <param name="namingStrategy">Default, CamelCase, KebabCase, SnakeCase</param>
        public static void JsonSerialize(this ModelItemCollection modelItems, string filePath ,bool sortAlphabetically = false, bool indentedFormat = false, NamingStrategy namingStrategy = NamingStrategy.Default)
        {
            CategoriesPropertiesHelper.SerializeModelItems(modelItems, filePath, sortAlphabetically, indentedFormat, namingStrategy);
        }

        #endregion Public Methods
    }
}