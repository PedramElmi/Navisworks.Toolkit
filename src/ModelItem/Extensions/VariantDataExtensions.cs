using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Api = Autodesk.Navisworks.Api;

namespace PedramElmi.Navisworks.Toolkit
{
    public static class VariantDataExtensions
    {
        /// <summary>
        /// Parses the variant data to the double type
        /// </summary>
        /// <param name="variantData"></param>
        /// <returns>data as double type</returns>
        public static double ParseDouble(this Api.VariantData variantData)
        {
            object data = variantData.GetValue();
            if (data is double value)
            {
                return value;
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
        /// Works with all data types (<see cref="Api.VariantDataType"/>) and returns the suitable string for displaying data
        /// </summary>
        /// <param name="variantData"></param>
        /// <returns></returns>
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
                    string str5 = num.ToString(currentCulture);
                    return str5;
                }
                case Api.VariantDataType.Int32:
                {
                    int num2 = variantData.ToInt32();
                    CultureInfo currentCulture2 = CultureInfo.CurrentCulture;
                    string str6 = num2.ToString(currentCulture2);
                    return str6;
                }
                case Api.VariantDataType.Boolean:
                {
                    bool flag = variantData.ToBoolean();
                    CultureInfo currentCulture5 = CultureInfo.CurrentCulture;
                    string str11 = flag.ToString(currentCulture5);
                    return str11;
                }
                case Api.VariantDataType.DisplayString:
                {
                    string str10 = variantData.ToDisplayString();
                    return str10;
                }
                case Api.VariantDataType.DateTime:
                {
                    string str9 = variantData.ToDateTime().ToString(CultureInfo.CurrentCulture);
                    return str9;
                }
                case Api.VariantDataType.DoubleLength:
                {
                    double num4 = variantData.ToDoubleLength();
                    CultureInfo currentCulture4 = CultureInfo.CurrentCulture;
                    string str8 = num4.ToString(currentCulture4);
                    return str8;
                }
                case Api.VariantDataType.DoubleAngle:
                {
                    double num3 = variantData.ToDoubleAngle();
                    CultureInfo currentCulture3 = CultureInfo.CurrentCulture;
                    string str7 = num3.ToString(currentCulture3);
                    return str7;
                }
                case Api.VariantDataType.NamedConstant:
                {
                    Api.NamedConstant namedConstant = variantData.ToNamedConstant();
                    string str4 = (!(namedConstant == null)) ? namedConstant.ToString() : "<null>";
                    return str4;
                }
                case Api.VariantDataType.IdentifierString:
                {
                    string str3 = variantData.ToIdentifierString();
                    return str3;
                }
                case Api.VariantDataType.Point3D:
                {
                    string str2 = variantData.ToPoint3D().ToString();
                    return str2;
                }
                case Api.VariantDataType.Point2D:
                {
                    string str = variantData.ToPoint2D().ToString();
                    return str;
                }
                default:
                    return "Unknown";
            }
        }

        /// <summary>
        /// Gets the value object based on its type. The types are defined in <see cref="Api.VariantDataType"/>
        /// </summary>
        /// <param name="variantData"></param>
        /// <returns></returns>
        public static object GetValue(this Api.VariantData variantData)
        {
            switch(variantData.DataType)
            {
                // Empty. No data stored.
                case Api.VariantDataType.None:
                    goto default;

                // Unit-less double value
                case Api.VariantDataType.Double:
                    return variantData.ToDouble();

                // Unit-less 32 bit integer value
                case Api.VariantDataType.Int32:
                    return variantData.ToInt32();

                // Boolean (true/false) value
                case Api.VariantDataType.Boolean:
                    return variantData.ToBoolean();

                // String intended for display to the end user (normally localized)
                case Api.VariantDataType.DisplayString:
                    return variantData.ToDisplayString();

                // A specific date and time (usually UTC)
                case Api.VariantDataType.DateTime:
                    return variantData.ToDateTime();

                // A double that represents a length (specific units depend on context)
                case Api.VariantDataType.DoubleLength:
                    return variantData.ToDoubleLength();

                // A double that represents an angle in radians
                case Api.VariantDataType.DoubleAngle:
                    return variantData.ToDoubleAngle();

                // A named constant
                case Api.VariantDataType.NamedConstant:
                    return variantData.ToNamedConstant().DisplayName;

                // String intended to be used as a programmatic identifier. 7-bit ASCII characters only.
                case Api.VariantDataType.IdentifierString:
                    return variantData.ToIdentifierString();

                // A double that species an area (specific units depend on context)
                case Api.VariantDataType.DoubleArea:
                    return variantData.ToDoubleArea();

                // A double that species a volume (specific units depend on context)
                case Api.VariantDataType.DoubleVolume:
                    return variantData.ToDoubleVolume();

                // A 3D point value
                case Api.VariantDataType.Point3D:
                    return variantData.ToPoint3D();

                // A 2D point value
                case Api.VariantDataType.Point2D:
                    return variantData.ToPoint2D();

                // the default
                default:
                    return variantData.ToString();
            }
        }
    }
}