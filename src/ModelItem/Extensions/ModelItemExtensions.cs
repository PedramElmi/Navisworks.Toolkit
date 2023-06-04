using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.ComApi;
using Autodesk.Navisworks.Api.Interop.ComApi;
using PedramElmi.Navisworks.Toolkit.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

using Api = Autodesk.Navisworks.Api;

namespace PedramElmi.Navisworks.Toolkit
{
    public static class ModelItemExtensions
    {
        /// <summary>
        /// Gets the icon type that it is on the visual tree.
        /// </summary>
        /// <param name="modelItem">
        /// </param>
        /// <returns>
        /// </returns>
        public static IconType GetIconType(this Api.ModelItem modelItem)
        {
            switch(modelItem.PropertyCategories.FindPropertyByName("LcOaNode", "LcOaNodeIcon").Value.ToNamedConstant().DisplayName)
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
        /// Returns the intersected of category names
        /// </summary>
        /// <param name="modelItems">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<string> GetIntersectedCategoriesDisplayName(this IEnumerable<Api.ModelItem> modelItems)
        {
            return (from modelItem in modelItems
                    let categories = (from category in modelItem.PropertyCategories select category.DisplayName).Distinct()
                    select categories).IntersectAll();
        }

        /// <summary>
        /// Returns the intersected of property names
        /// </summary>
        /// <param name="modelItems">
        /// </param>
        /// <param name="categoryDisplayName">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<string> GetIntersectedPropertiesDisplayName(this IEnumerable<Api.ModelItem> modelItems, string categoryDisplayName)
        {
            var categories =
                from item in modelItems
                let category = item.PropertyCategories.FindCategoryByDisplayName(categoryDisplayName)
                where category != null
                select category;

            //var properties = from category in categories select category.GetPropertiesDisplayName();
            var properties = from category in categories select category.Properties.Select(property => property.DisplayName);

            return properties.IntersectAll();
        }

        public static void Remove(this Api.ModelItem modelItem, CustomPropertyCategory customPropertyCategory)
        {
            // convert ModelItem to COM Path
            var comModelItem = ComApiBridge.ToInwOaPath(modelItem);

            // Get item's COM PropertyCategoryCollection
            var comPropertyCategories = ComApiBridge.State.GetGUIPropertyNode(comModelItem, true) as InwGUIPropertyNode2;

            // Find the index of the category
            var index = 0;
            var usingIndex = 0;
            foreach(InwGUIAttribute2 attribute in comPropertyCategories.GUIAttributes())
            {
                if(attribute.UserDefined)
                {
                    index++;
                    if(attribute.ClassUserName.Equals(customPropertyCategory.DisplayName))
                    {
                        usingIndex = index;
                        break;
                    }
                }
            }

            // Remove the category if found
            if(usingIndex == 0)
            {
                throw new Exception($"{customPropertyCategory.DisplayName} does not exist in categories");
            }
            else
            {
                comPropertyCategories.RemoveUserDefined(usingIndex);
            }
        }

        public static void Serialize(this ModelItem modelItem, string filePath)
        {
            var data = modelItem.PropertyCategories.ToDictionary();

            using(var writer = new StreamWriter(filePath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, data);
            }
        }

        public static void Serialize(this IEnumerable<ModelItem> modelItems, string filePath)
        {
            var data = modelItems.Select(item => item.PropertyCategories.ToDictionary());

            using(var writer = new StreamWriter(filePath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, data);
            }
        }

        public static void SerializeHierarchy(this ModelItem modelItem, string filePath)
        {
            var data = modelItem.ToDictionaryHierarchy();

            using(var writer = new StreamWriter(filePath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, data);
            }
        }

        public static void SerializeHierarchy(this IEnumerable<ModelItem> modelItems, string filePath)
        {
            var data = modelItems.ToDictionaryHierarchy();

            using(var writer = new StreamWriter(filePath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, data);
            }
        }

        public static IEnumerable<IDictionary<string, object>> ToDictionary(this IEnumerable<ModelItem> modelItems)
        {
            return modelItems.Select(item => item.PropertyCategories.ToDictionary());
        }

        public static IDictionary<string, object> ToDictionaryHierarchy(this ModelItem modelItem)
        {
            var categories = modelItem.PropertyCategories.ToDictionary() as IDictionary<string, object>;

            var children = modelItem.Children.Select(child => child.ToDictionaryHierarchy());

            categories.Add("Children", children);

            return categories;
        }

        public static IEnumerable<IDictionary<string, object>> ToDictionaryHierarchy(this IEnumerable<ModelItem> modelItems)
        {
            return modelItems.Select(item => item.ToDictionaryHierarchy());
        }

        /// <summary>
        /// Returns a <see cref="Api.ModelItemCollection"/>
        /// </summary>
        /// <param name="modelItems">
        /// </param>
        /// <returns>
        /// </returns>
        public static ModelItemCollection ToModelItemCollection(this IEnumerable<Api.ModelItem> modelItems)
        {
            var collection = new Api.ModelItemCollection();
            collection.AddRange(modelItems);
            return collection;
        }

        public static void Update(this Api.ModelItem modelItem, CustomPropertyCategory customPropertyCategory)
        {
            // sort alphabetically define properties
            var properties = customPropertyCategory.Properties.OrderBy(property => property.DisplayName);

            SetUserDefined(modelItem, customPropertyCategory.DisplayName, customPropertyCategory.Name, properties);
        }

        /// <summary>
        /// Updates a Custom Category Property (User-Defined). If this category exists, it will overwrites the new value of
        /// properties and remains the existing properties
        /// </summary>
        /// <param name="modelItem">
        /// </param>
        /// <param name="customCategory">
        /// This category and its properties will be added to this ModelItem
        /// </param>
        public static void Upsert(this Api.ModelItem modelItem, CustomPropertyCategory customCategory)
        {
            // find category if exists
            var category = modelItem.PropertyCategories.FindCategoryByCombinedName(customCategory.CombinedName);

            // if category exists, add existing properties retrieve existing Properties
            var existingProperties = category?.Properties ?? Enumerable.Empty<DataProperty>();

            // merge with new properties
            var unionProperties = customCategory.Properties.Union(existingProperties, new DataPropertyComparer());

            // sort alphabetically define properties
            var properties = unionProperties.OrderBy(property => property.DisplayName);

            SetUserDefined(modelItem, customCategory.DisplayName, customCategory.Name, properties);
        }

        private static void SetUserDefined(Api.ModelItem modelItem, string userName, string internalName, IEnumerable<DataProperty> properties)
        {
            // create empty COM category
            var newComCategory = ComApiBridge.State.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null) as InwOaPropertyVec;

            // create COM properties and add them to Com category
            foreach(var property in properties)
            {
                var newCOMProperty = ComApiBridge.State.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null) as InwOaProperty;

                // set property name
                newCOMProperty.name = property.Name;

                // set property display name
                newCOMProperty.UserName = property.DisplayName;

                // set property value
                newCOMProperty.value = property.Value.GetValue();

                // add new COM property to COM category
                newComCategory.Properties().Add(newCOMProperty);
            }

            // convert ModelItem to COM Path
            var comModelItem = ComApiBridge.ToInwOaPath(modelItem);

            // Get item's COM PropertyCategoryCollection
            var comPropertyCategories = ComApiBridge.State.GetGUIPropertyNode(comModelItem, true) as InwGUIPropertyNode2;

            var index = 0;
            var usingIndex = 0;
            foreach(InwGUIAttribute2 attribute in comPropertyCategories.GUIAttributes())
            {
                if(attribute.UserDefined)
                {
                    index++;
                    if(attribute.ClassUserName.Equals(userName))
                    {
                        usingIndex = index;
                        break;
                    }
                }
            }

            // overwrite the existing category with newly properties
            comPropertyCategories.SetUserDefined(usingIndex, userName, internalName, newComCategory);
        }
    }
}