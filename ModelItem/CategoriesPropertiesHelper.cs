using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Autodesk.Navisworks.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace NavisworksDevHelper.ModelItem
{
    public static partial class CategoriesPropertiesHelper
    {
        #region Methods

        /// <summary>
        ///  gets the Value based on its type.
        /// </summary>
        /// <param name="variantData">VariantData (Value)</param>
        /// <returns>Object that can be casted to its class (i.e. NamedConstant, double etc...) available in Autodesk.Navisworks.Api.VariantDataType</returns>
        public static object GetCleanedVariantData(VariantData variantData)
        {
            switch (variantData.DataType)
            {
                // Empty. No data stored.
                case VariantDataType.None:
                    goto default;

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

        /// <summary>
        /// Returns the suitable string for displaying data of the VariantData class
        /// </summary>
        /// <param name="variantData"></param>
        /// <returns></returns>
        public static string GetDisplayableVariantData(VariantData variantData)
        {
            switch (variantData.DataType)
            {
                // Empty. No data stored.
                case VariantDataType.None:
                    goto default;

                // Unit-less double value
                case VariantDataType.Double:
                    goto default;

                // Unit-less 32 bit integer value
                case VariantDataType.Int32:
                    goto default;

                // Boolean (true/false) value
                case VariantDataType.Boolean:
                    goto default;

                // String intended for display to the end user (normally localized)
                case VariantDataType.DisplayString:
                    return variantData.ToDisplayString();

                // A specific date and time (usually UTC)
                case VariantDataType.DateTime:
                    goto default;

                // A double that represents a length (specific units depend on context)
                case VariantDataType.DoubleLength:
                    goto default;

                // A double that represents an angle in radians
                case VariantDataType.DoubleAngle:
                    goto default;

                // A named constant
                case VariantDataType.NamedConstant:
                    return variantData.ToNamedConstant().DisplayName;

                // String intended to be used as a programmatic identifier. 7-bit ASCII characters only.
                case VariantDataType.IdentifierString:
                    return variantData.ToIdentifierString();

                // A double that species an area (specific units depend on context)
                case VariantDataType.DoubleArea:
                    goto default;

                // A double that species a volume (specific units depend on context)
                case VariantDataType.DoubleVolume:
                    goto default;

                // A 3D point value
                case VariantDataType.Point3D:
                    goto default;

                // A 2D point value
                case VariantDataType.Point2D:
                    goto default;

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

            var iconValue = GetDisplayableVariantData(iconProperty.Value);

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

        /// <summary>
        /// Serialize ModelItem Properties to JSON
        /// </summary>
        /// <param name="modelItems"></param>
        /// <returns>JSON as StringBuilder</returns>
        public static StringBuilder SerializeModelItems(Autodesk.Navisworks.Api.ModelItemCollection modelItems, bool sortAlphabetically = false, bool indentedFormat = false)
        {

            // setting the data in the serializable classes
            var preparedModelItems = new List<ModelItemSerializable>();
            foreach (var modelItem in modelItems)
            {
                preparedModelItems.Add(new ModelItemSerializable(modelItem));
            }

            var output = new StringBuilder();
            using (TextWriter textWriter = new StringWriter(output))
            {
                using (JsonWriter jsonWriter = new JsonTextWriter(textWriter))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    if (sortAlphabetically)
                    {
                        var jsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings
                        {
                            ContractResolver = new OrderedContractResolver(),
                        };
                        serializer = Newtonsoft.Json.JsonSerializer.Create(jsonSerializerSettings);
                    }

                    if (indentedFormat)
                    {
                        serializer.Formatting = Formatting.Indented; 
                    }

                    serializer.Serialize(jsonWriter, preparedModelItems);
                }
            }

            return output;
        }
        
        /// <summary>
        /// Serialize ModelItems to JSON file and save it in the file in filePath
        /// </summary>
        /// <param name="modelItems"></param>
        /// <param name="filePath">file path of the JSON file. Example: "D:\\Test\\test.json"</param>
        public static void SerializeModelItems(Autodesk.Navisworks.Api.ModelItemCollection modelItems, string filePath, bool sortAlphabetically = false, bool indentedFormat = false)
        {
            // setting the data in the serializable classes
            var preparedModelItems = new List<ModelItemSerializable>();
            foreach (var modelItem in modelItems)
            {
                preparedModelItems.Add(new ModelItemSerializable(modelItem));
            }

            using (TextWriter textWriter = new StreamWriter(filePath))
            {
                using (JsonWriter jsonWriter = new JsonTextWriter(textWriter))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    if (sortAlphabetically)
                    {
                        var jsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings
                        {
                            ContractResolver = new OrderedContractResolver(),
                        };
                        serializer = Newtonsoft.Json.JsonSerializer.Create(jsonSerializerSettings);
                    }

                    if (indentedFormat)
                    {
                        serializer.Formatting = Formatting.Indented;
                    }

                    serializer.Serialize(jsonWriter, preparedModelItems);
                }
            }
        }

        #endregion

    }



}
