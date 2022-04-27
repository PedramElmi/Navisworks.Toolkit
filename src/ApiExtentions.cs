using PedramElmi.Navisworks.Toolkit.ModelItem;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Api = Autodesk.Navisworks.Api;

namespace PedramElmi.Navisworks.Toolkit
{
    public static class ApiExtentions
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
        /// Add a custom Category Property (User-Defined). If this category exists, it will
        /// overwrites the new value of properties and remains the existing properties
        /// </summary>
        /// <param name="modelItems"></param>
        /// <param name="category">This category and its properties will be added to these ModelItems</param>
        public static void AddCustomPropertyCategory(this Api.ModelItemCollection modelItems, CustomPropertyCategory category)
        {
            category.AddToModelItems(modelItems);
        }

        /// <summary>
        /// Returns the categories display name of categories
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        public static HashSet<string> GetCategoriesDisplaName(this Api.PropertyCategoryCollection categories)
        {
            return CategoriesProperties.GetCategoriesDisplaName(categories);
        }

        /// <summary>
        /// Gets the icon type that it is on the visual tree.
        /// </summary>
        /// <param name="modelItem"></param>
        /// <returns></returns>
        public static IconType GetIconType(this global::Autodesk.Navisworks.Api.ModelItem modelItem)
        {
            return CategoriesProperties.GetIconType(modelItem);
        }

        /// <summary>
        /// Returns the intersected of category names
        /// </summary>
        /// <param name="modelItems"></param>
        /// <returns></returns>
        public static HashSet<string> GetIntersectedCategoriesDisplayName(this Api.ModelItemCollection modelItems)
        {
            return CategoriesProperties.GetIntersectedCategoriesDisplayName(modelItems);
        }

        /// <summary>
        /// Returns the intersected of property names
        /// </summary>
        /// <param name="modelItems"></param>
        /// <param name="categoryDisplayName"></param>
        /// <returns></returns>
        public static HashSet<string> GetIntersectedPropertiesDisplayName(this Api.ModelItemCollection modelItems, string categoryDisplayName)
        {
            return CategoriesProperties.GetIntersectedPropertiesDisplayName(modelItems, categoryDisplayName);
        }

        /// <summary>
        /// Returns the properties display name
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static HashSet<string> GetPropertiesDisplayName(this Api.PropertyCategory category)
        {
            return CategoriesProperties.GetPropertiesDisplayName(category);
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

        /// <summary>
        /// Serialize the ModelItems PropertyCategories in a JSON format
        /// </summary>
        /// <param name="modelItems">ModelItems</param>
        /// <param name="sortAlphabetically">True: Sorted, False: Unsorted</param>
        /// <param name="indentedFormat">True: Indented, False: Unindented</param>
        /// <param name="namingStrategy">Default, CamelCase, KebabCase, SnakeCase</param>
        /// <returns>JSON Formated of the ModelItems</returns>
        public static StringBuilder JsonSerialize(this Api.ModelItemCollection modelItems, bool sortAlphabetically = false, bool indentedFormat = false, NamingStrategy namingStrategy = NamingStrategy.Default)
        {
            return CategoriesProperties.SerializeModelItems(modelItems, sortAlphabetically, indentedFormat, namingStrategy);
        }

        /// <summary>
        /// Serialize the ModelItems PropertyCategories in a JSON format and save it in a .json text file
        /// </summary>
        /// <param name="modelItems">ModelItems</param>
        /// <param name="filePath">file path to save it on a .json text file</param>
        /// <param name="sortAlphabetically">True: Sorted, False: Unsorted</param>
        /// <param name="indentedFormat">True: Indented, False: Unindented</param>
        /// <param name="namingStrategy">Default, CamelCase, KebabCase, SnakeCase</param>
        public static void JsonSerialize(this Api.ModelItemCollection modelItems, string filePath, bool sortAlphabetically = false, bool indentedFormat = false, NamingStrategy namingStrategy = NamingStrategy.Default)
        {
            CategoriesProperties.SerializeModelItems(modelItems, filePath, sortAlphabetically, indentedFormat, namingStrategy);
        }

        /// <summary>
        /// Parses the variant data to the double type
        /// </summary>
        /// <param name="variantData"></param>
        /// <returns>data as double type</returns>
        public static double ParseDouble(this Api.VariantData variantData)
        {
            dynamic data = variantData.ToDynamic();
            if (data is double)
            {
                return data;
            }
            else
            {
                try
                {
                    return Convert.ToDouble(data);
                }
                catch (Exception)
                {
                    try
                    {
                        const string pattern = @"\b(-?)(0|([1-9][0-9]*))(\.[0-9]+)?\b";
                        Match match = Regex.Match(data.ToString(), pattern);
                        if (match.Success)
                        {
                            return Convert.ToDouble(match.Value);
                        }
                        else
                        {
                            return double.NaN;
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the suitable string for displaying data of the VariantData class and Cleaned
        /// version of .ToString()
        /// </summary>
        /// <param name="variantData"></param>
        /// <returns></returns>
        public static string ToCleanedString(this Api.VariantData variantData)
        {
            return CategoriesProperties.GetCleanedString(variantData);
        }

        /// <summary>
        /// Gets the dynamic value based on its type
        /// </summary>
        /// <param name="variantData"></param>
        /// <returns>
        /// dynamic type that can be used as its type (i.e. NamedConstant, double, string etc...)
        /// available in Autodesk.Navisworks.Api.VariantDataType
        /// </returns>
        public static dynamic ToDynamic(this Api.VariantData variantData)
        {
            return CategoriesProperties.GetVariantData(variantData);
        }

        #endregion Public Methods
    }
}