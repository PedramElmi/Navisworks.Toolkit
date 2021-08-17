using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Navisworks.Api;

namespace NavisworksDevHelper
{
    class ModelItemPropertiesHelper
    {
        public static class NavisworksModelItemPropertiesHelper
        {
            /// <summary>
            /// Returns the Value based on its type.
            /// </summary>
            /// <param name="variantData">Value</param>
            /// <returns>Cleaned Value</returns>
            public static object GetCleanedVariantData(VariantData variantData)
            {
                switch (variantData.DataType)
                {
                    // Empty. No data stored.
                    case VariantDataType.None:
                        return string.Empty;

                    // Unit-less double value
                    case VariantDataType.Double:
                        return variantData.ToDouble();

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
                        return variantData.ToNamedConstant();

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
        }
    }
}
