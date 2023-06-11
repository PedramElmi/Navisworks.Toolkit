using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Api = Autodesk.Navisworks.Api;

namespace PedramElmi.Navisworks.Toolkit
{
    public static class VariantDataExtensions
    {
        /// <summary>
        /// Retrieves the value from the given VariantData object based on its data type.
        /// </summary>
        /// <param name="variantData">
        /// The VariantData object to extract the value from.
        /// </param>
        /// <returns>
        /// An object representing the value stored in the given VariantData object.
        /// </returns>
        public static object GetValue(this Api.VariantData variantData)
        {
            switch(variantData.DataType)
            {
                case Api.VariantDataType.None:
                    goto default;
                case Api.VariantDataType.Double:
                    return variantData.ToDouble();
                case Api.VariantDataType.Int32:
                    return variantData.ToInt32();
                case Api.VariantDataType.Boolean:
                    return variantData.ToBoolean();
                case Api.VariantDataType.DisplayString:
                    return variantData.ToDisplayString();
                case Api.VariantDataType.DateTime:
                    return variantData.ToDateTime();
                case Api.VariantDataType.DoubleLength:
                    return variantData.ToDoubleLength();
                case Api.VariantDataType.DoubleAngle:
                    return variantData.ToDoubleAngle();
                case Api.VariantDataType.NamedConstant:
                    return variantData.ToNamedConstant().DisplayName;
                case Api.VariantDataType.IdentifierString:
                    return variantData.ToIdentifierString();
                case Api.VariantDataType.DoubleArea:
                    return variantData.ToDoubleArea();
                case Api.VariantDataType.DoubleVolume:
                    return variantData.ToDoubleVolume();
                case Api.VariantDataType.Point3D:
                    return variantData.ToPoint3D();
                case Api.VariantDataType.Point2D:
                    return variantData.ToPoint2D();
                default:
                    return variantData.ToString();
            }
        }

        /// <summary>
        /// Converts the given VariantData object to a double value.
        /// </summary>
        /// <param name="variantData">
        /// The VariantData object to be converted.
        /// </param>
        /// <returns>
        /// A double value representing the data in the given VariantData object.
        /// </returns>
        public static double ParseDouble(this Api.VariantData variantData)
        {
            object data = variantData.GetValue();
            if(data is double value)
            {
                return value;
            }
            else
            {
                try
                {
                    return Convert.ToDouble(data);
                }
                catch(Exception)
                {
                    try
                    {
                        const string pattern = @"\b(-?)(0|([1-9][0-9]*))(\.[0-9]+)?\b";
                        Match match = Regex.Match(data.ToString(), pattern);
                        if(match.Success)
                        {
                            return Convert.ToDouble(match.Value);
                        }
                        else
                        {
                            return double.NaN;
                        }
                    }
                    catch(Exception)
                    {
                        return double.NaN;
                    }
                }
            }
        }

        /// <summary>
        /// Converts the given VariantData object to a suitable display string based on its data type.
        /// </summary>
        /// <param name="variantData">
        /// The VariantData object to be converted.
        /// </param>
        /// <returns>
        /// A string representation of the data in the given VariantData object, suitable for display.
        /// </returns>
        public static string ToDisplayStringAlternative(this Api.VariantData variantData)
        {
            if(variantData.IsDisposed)
            {
                return "Disposed";
            }
            switch(variantData.DataType)
            {
                case Api.VariantDataType.None:
                    return "None";
                case Api.VariantDataType.Double:
                {
                    double num = variantData.ToDouble();
                    CultureInfo currentCulture = CultureInfo.CurrentCulture;
                    return num.ToString(currentCulture);
                }
                case Api.VariantDataType.Int32:
                {
                    int num2 = variantData.ToInt32();
                    CultureInfo currentCulture2 = CultureInfo.CurrentCulture;
                    return num2.ToString(currentCulture2);
                }
                case Api.VariantDataType.Boolean:
                {
                    bool flag = variantData.ToBoolean();
                    CultureInfo currentCulture5 = CultureInfo.CurrentCulture;
                    return flag.ToString(currentCulture5);
                }
                case Api.VariantDataType.DisplayString:
                {
                    return variantData.ToDisplayString();
                }
                case Api.VariantDataType.DateTime:
                {
                    return variantData.ToDateTime().ToString(CultureInfo.CurrentCulture);
                }
                case Api.VariantDataType.DoubleLength:
                {
                    double num4 = variantData.ToDoubleLength();
                    CultureInfo currentCulture4 = CultureInfo.CurrentCulture;
                    return num4.ToString(currentCulture4);
                }
                case Api.VariantDataType.DoubleAngle:
                {
                    double num3 = variantData.ToDoubleAngle();
                    CultureInfo currentCulture3 = CultureInfo.CurrentCulture;
                    return num3.ToString(currentCulture3);
                }
                case Api.VariantDataType.NamedConstant:
                {
                    Api.NamedConstant namedConstant = variantData.ToNamedConstant();
                    return namedConstant != null ? namedConstant.ToString() : "<null>";
                }
                case Api.VariantDataType.IdentifierString:
                {
                    return variantData.ToIdentifierString();
                }
                case Api.VariantDataType.Point3D:
                {
                    return variantData.ToPoint3D().ToString();
                }
                case Api.VariantDataType.Point2D:
                {
                    return variantData.ToPoint2D().ToString();
                }
                default:
                    return "Unknown";
            }
        }
    }
}