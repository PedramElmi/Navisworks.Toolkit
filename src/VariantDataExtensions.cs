using PedramElmi.Navisworks.Toolkit.ModelItem;
using System;
using System.Text.RegularExpressions;
using Api = Autodesk.Navisworks.Api;

namespace PedramElmi.Navisworks.Toolkit
{
    public static class VariantDataExtensions
    {
        #region Public Methods

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
                        return double.NaN;
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