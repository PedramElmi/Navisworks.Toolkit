using Api = Autodesk.Navisworks.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace PedramElmi.Navisworks.Toolkit.ModelItem
{
    /// <summary>
    /// Static Helper Methods for CategoryProperties
    /// </summary>
    public static partial class CategoriesProperties
    {
        #region Public Methods

        /// <summary>
        /// Returns the suitable string for displaying data of the VariantData class and Cleaned
        /// version of .ToString()
        /// </summary>
        /// <param name="variantData"></param>
        /// <returns></returns>
        public static string GetCleanedString(Api.VariantData variantData)
        {
            if (variantData.IsDisposed)
            {
                return "Disposed";
            }

            switch (variantData.DataType)
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
        /// Gets the icon type that it is on visual tree.
        /// </summary>
        /// <param name="modelItem"></param>
        /// <returns></returns>
        public static IconType GetIconType(global::Autodesk.Navisworks.Api.ModelItem modelItem)
        {
            var iconProperty = modelItem.PropertyCategories.FindPropertyByName("LcOaNode", "LcOaNodeIcon");

            //var iconValue = GetCleanedString(iconProperty.Value);

            switch (iconProperty.Value.ToNamedConstant().DisplayName)
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
        /// Gets the dynamic value based on its type
        /// </summary>
        /// <param name="variantData">VariantData (Value)</param>
        /// <returns>
        /// dynamic that can be casted to its class (i.e. NamedConstant, double etc...) available in Autodesk.Navisworks.Api.VariantDataType
        /// </returns>
        public static dynamic GetVariantData(Api.VariantData variantData)
        {
            switch (variantData.DataType)
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
                    return variantData.ToNamedConstant();

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

        /// <summary>
        /// Serialize ModelItem Properties to JSON
        /// </summary>
        /// <param name="modelItems"></param>
        /// <returns>JSON as StringBuilder</returns>
        public static StringBuilder SerializeModelItems(Api.ModelItemCollection modelItems, bool sortAlphabetically, bool indentedFormat, NamingStrategy namingStrategy)
        {
            // setting the data in the serializable private classes
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
                    var serializer = GetJsonSerializer(sortAlphabetically, indentedFormat, namingStrategy);
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
        public static void SerializeModelItems(Api.ModelItemCollection modelItems, string filePath, bool sortAlphabetically, bool indentedFormat, NamingStrategy namingStrategy)
        {
            // setting the data in the serializable private classes
            var preparedModelItems = new List<ModelItemSerializable>();
            foreach (var modelItem in modelItems)
            {
                preparedModelItems.Add(new ModelItemSerializable(modelItem));
            }

            using (TextWriter textWriter = new StreamWriter(filePath))
            {
                using (JsonWriter jsonWriter = new JsonTextWriter(textWriter))
                {
                    var serializer = GetJsonSerializer(sortAlphabetically, indentedFormat, namingStrategy);
                    serializer.Serialize(jsonWriter, preparedModelItems);
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private static JsonSerializer GetJsonSerializer(bool sortAlphabetically, bool indentedFormat, NamingStrategy namingStrategy)
        {
            var contractResolver = sortAlphabetically ? new OrderedContractResolver() : new DefaultContractResolver();

            switch (namingStrategy)
            {
                case NamingStrategy.Default:
                    contractResolver.NamingStrategy = new DefaultNamingStrategy();
                    break;

                case NamingStrategy.CamelCase:
                    contractResolver.NamingStrategy = new CamelCaseNamingStrategy();
                    break;

                case NamingStrategy.KebabCase:
                    contractResolver.NamingStrategy = new KebabCaseNamingStrategy();
                    break;

                case NamingStrategy.SnakeCase:
                    contractResolver.NamingStrategy = new SnakeCaseNamingStrategy();
                    break;
            }

            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = contractResolver
            };
            var serializer = JsonSerializer.Create(jsonSerializerSettings);

            if (indentedFormat)
            {
                serializer.Formatting = Formatting.Indented;
            }

            return serializer;
        }

        #endregion Private Methods


        #region Public Methods

        public static HashSet<string> GetCategoriesDisplaName(Api.PropertyCategoryCollection categories)
        {
            return (from category in categories
                    select category.DisplayName).ToHashSet();
        }

        public static HashSet<string> GetIntersectedCategoriesDisplayName(Api.ModelItemCollection modelItems)
        {
            var categories = new HashSet<HashSet<string>>();
            foreach (var modelItem in modelItems)
            {
                categories.Add(new HashSet<string>(GetCategoriesDisplaName(modelItem.PropertyCategories)));
            }

            // intersect all of ModelItem's category name
            // more: https://stackoverflow.com/questions/1674742/intersection-of-multiple-lists-with-ienumerable-intersect
            return categories
                .Skip(1)
                .Aggregate(
                new HashSet<string>(categories.First()),
                (h, e) => { h.IntersectWith(e); return h; });
        }

        public static HashSet<string> GetIntersectedPropertiesDisplayName(Api.ModelItemCollection modelItems, string categoryDisplayName)
        {
            var categories = new HashSet<Api.PropertyCategory>();
            foreach (var item in modelItems)
            {
                categories.Add(item.PropertyCategories.FindCategoryByDisplayName(categoryDisplayName));
            }

            var properties = new HashSet<HashSet<string>>();
            foreach (var category in categories)
            {
                properties.Add(new HashSet<string>(GetPropertiesDisplayName(category)));
            }

            return properties
                .Skip(1)
                .Aggregate(
                new HashSet<string>(properties.First()),
                (h, e) => { h.IntersectWith(e); return h; });
        }

        public static HashSet<string> GetPropertiesDisplayName(Api.PropertyCategory category)
        {
            return (from property in category.Properties select property.DisplayName).ToHashSet();
        }

        public static HashSet<string> GetPropertiesDisplayName(Api.ModelItem modelItem, string categoryDisplayName)
        {
            var category = modelItem.PropertyCategories.FindCategoryByDisplayName(categoryDisplayName);

            return GetPropertiesDisplayName(category);
        }

        #endregion Public Methods
    }
}