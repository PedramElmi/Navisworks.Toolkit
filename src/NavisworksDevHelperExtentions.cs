using Autodesk.Navisworks.Api;

namespace NavisworksDevHelper.ModelItemHelpers
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
            return GetIconType(modelItem);
        }

        #endregion Public Methods
    }
}