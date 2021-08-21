using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Navisworks.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


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
                    if (toDisplayValueAsString)
                    {
                        return variantData.ToDouble().ToString();
                    }
                    else
                    {
                        return variantData.ToDouble();
                    }

                // Unit-less 32 bit integer value
                case VariantDataType.Int32:
                    if (toDisplayValueAsString)
                    {
                        return variantData.ToInt32().ToString();
                    }
                    else
                    {
                        return variantData.ToInt32();
                    }

                // Boolean (true/false) value
                case VariantDataType.Boolean:
                    if (toDisplayValueAsString)
                    {
                        return variantData.ToBoolean().ToString();
                    }
                    else
                    {
                        return variantData.ToBoolean();
                    }

                // String intended for display to the end user (normally localized)
                case VariantDataType.DisplayString:
                    return variantData.ToDisplayString();

                // A specific date and time (usually UTC)
                case VariantDataType.DateTime:
                    if (toDisplayValueAsString)
                    {
                        return variantData.ToDateTime().ToString();
                    }
                    else
                    {
                        return variantData.ToDateTime();
                    }

                // A double that represents a length (specific units depend on context)
                case VariantDataType.DoubleLength:
                    if (toDisplayValueAsString)
                    {
                        return variantData.ToDoubleLength().ToString();
                    }
                    else
                    {
                        return variantData.ToDoubleLength();
                    }

                // A double that represents an angle in radians
                case VariantDataType.DoubleAngle:
                    if (toDisplayValueAsString)
                    {
                        return variantData.ToDoubleAngle().ToString();
                    }
                    else
                    {
                        return variantData.ToDoubleAngle();
                    }

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
                    if (toDisplayValueAsString)
                    {
                        return variantData.ToDoubleArea().ToString();
                    }
                    else
                    {
                        return variantData.ToDoubleArea();
                    }

                // A double that species a volume (specific units depend on context)
                case VariantDataType.DoubleVolume:
                    if (toDisplayValueAsString)
                    {
                        return variantData.ToDoubleVolume().ToString();
                    }
                    else
                    {
                        return variantData.ToDoubleVolume();
                    }

                // A 3D point value
                case VariantDataType.Point3D:
                    if (toDisplayValueAsString)
                    {
                        return variantData.ToPoint3D().ToString();
                    }
                    else
                    {
                        return variantData.ToPoint3D();
                    }

                // A 2D point value
                case VariantDataType.Point2D:
                    if (toDisplayValueAsString)
                    {
                        return variantData.ToPoint2D().ToString();
                    }
                    else
                    {
                        return variantData.ToPoint2D();
                    }

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

        public static StringBuilder SerializeModelItemsProperties(Autodesk.Navisworks.Api.ModelItemCollection modelItems)
        {

            #region setting the data in the serializable classes
            var preparedModelItems = new List<ModelItemCategoriesSerialiableObject>();
            foreach (var modelItem in modelItems)
            {
                var categories = new List<Category>();
                foreach (var category in modelItem.PropertyCategories)
                {
                    var thisCategory = new Category()
                    {
                        Name = category.Name,
                        DisplayName = category.DisplayName,
                        Properties = new List<Property>()
                    };


                    foreach (var property in category.Properties)
                    {

                        var thisProperty = new Property()
                        {
                            Name = property.Name,
                            DisplayName = property.DisplayName,
                            Value = GetCleanedVariantData(property.Value, true) as string,
                            VauleType = property.Value.DataType.ToString()
                        };

                        thisCategory.Properties.Add(thisProperty);

                    }

                    categories.Add(thisCategory);

                }

                preparedModelItems.Add(new ModelItemCategoriesSerialiableObject()
                {
                    DisplayName = modelItem.DisplayName,
                    ClassDisplayName = modelItem.ClassDisplayName,
                    ClassName = modelItem.ClassName,
                    Model = modelItem.HasModel ? modelItem.Model.FileName : "",
                    Categories = categories
                });
            }
            #endregion

            var output = new StringBuilder();

            using (TextWriter textWriter = new StringWriter(output))
            {
                using (JsonWriter jsonWriter = new JsonTextWriter(textWriter))
                {
                    var serializer = new JsonSerializer()
                    {
                        Formatting = Formatting.Indented
                    };

                    serializer.Serialize(jsonWriter, preparedModelItems);
                }
            }

            return output;
        }

        private class ModelItemCategoriesSerialiableObject
        {
            public string DisplayName { get; set; }
            public string ClassDisplayName { get; set; }
            public string ClassName { get; set; }
            public string Model { get; set; }

            public List<Category> Categories { get; set; }
        }

        private class Category
        {
            public string Name { get; set; }

            public string DisplayName { get; set; }

            public List<Property> Properties { get; set; }

        }

        private class Property
        {
            public string Name { get; set; }
            public string DisplayName { get; set; }
            public string Value { get; set; }
            public string VauleType { get; set; }
        }
    }

}
