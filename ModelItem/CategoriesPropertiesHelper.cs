using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Navisworks.Api;

namespace NavisworksDevHelper.ModelItem
{
    public static class CategoriesPropertiesHelper
    {
        /// <summary>
        ///  Returns the Value based on its type.
        /// </summary>
        /// <param name="variantData">VariantData (Value)</param>
        /// <param name="toDisplayValueAsString">Is it for display as string?</param>
        /// <returns>with displayAsString True: returns best string for displaying, false: returns object</returns>
        public static object GetCleanedVariantData(VariantData variantData, bool toDisplayValueAsString = false)
        {
            switch (variantData.DataType)
            {
                // Empty. No data stored.
                case VariantDataType.None:
                    goto default;

                // Unit-less double value
                case VariantDataType.Double:
                    return toDisplayValueAsString ? Math.Round(variantData.ToDouble(),2) : variantData.ToDouble();

                // Unit-less 32 bit integer value
                case VariantDataType.Int32:
                    return variantData.ToInt32();

                // Boolean (true/false) value
                case VariantDataType.Boolean:
                    return variantData.ToBoolean();

                // String intended for display to the end user (normally localized)
                case VariantDataType.DisplayString:
                    return variantData.ToDisplayString();

                // A specific date and time (usually UTC)
                case VariantDataType.DateTime:
                    return variantData.ToDateTime();

                // A double that represents a length (specific units depend on context)
                case VariantDataType.DoubleLength:
                    return variantData.ToDoubleLength();

                // A double that represents an angle in radians
                case VariantDataType.DoubleAngle:
                    return variantData.ToDoubleAngle();

                // A named constant
                case VariantDataType.NamedConstant:
                    if (toDisplayValueAsString)
                    {
                        return variantData.ToNamedConstant().DisplayName;
                    }
                    else
                    {
                        return variantData.ToNamedConstant();
                    }

                // String intended to be used as a programmatic identifier. 7-bit ASCII characters only.
                case VariantDataType.IdentifierString:
                    return variantData.ToIdentifierString();

                // A double that species an area (specific units depend on context)
                case VariantDataType.DoubleArea:
                    return variantData.ToDoubleArea();

                // A double that species a volume (specific units depend on context)
                case VariantDataType.DoubleVolume:
                    return variantData.ToDoubleVolume();

                // A 3D point value
                case VariantDataType.Point3D:
                    return variantData.ToPoint3D();

                // A 2D point value
                case VariantDataType.Point2D:
                    return variantData.ToPoint2D();

                // the default
                default:
                    return variantData.ToString();
            }
        }

        /// <summary>
        /// Gets the icon type that it is on visual tree.
        /// </summary>
        /// <param name="modelItem"></param>
        /// <returns></returns>
        public static IconType GetIconType(Autodesk.Navisworks.Api.ModelItem modelItem)
        {
            var iconProperty = modelItem.PropertyCategories.FindPropertyByName("LcOaNode", "LcOaNodeIcon");

            var iconValue = GetCleanedVariantData(iconProperty.Value, true) as string;

            switch (iconValue)
            {
                case "File":
                    return IconType.File;

                case "Layer":
                    return IconType.Layer;

                case "Collection":
                    return IconType.Collection;

                case "Composite Object":
                    return IconType.CompositeObject;

                case "Insert Group":
                    return IconType.InsertGroup;

                case "Geometry":
                    return IconType.Geometry;

                default:
                    return IconType.Unidentified;
            }
        }

    }
}
