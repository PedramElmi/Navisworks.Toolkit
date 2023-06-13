using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.ComApi;
using Autodesk.Navisworks.Api.Interop.ComApi;
using Newtonsoft.Json;
using Community.Navisworks.Toolkit.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

using Api = Autodesk.Navisworks.Api;

namespace Community.Navisworks.Toolkit
{
    public static class ModelItemExtensions
    {
        /// <summary>
        /// Returns a BitmapImage object representing the icon of a given model item
        /// </summary>
        /// <param name="modelItem">
        /// </param>
        /// <returns>
        /// </returns>
        public static BitmapImage GetIcon(this Api.ModelItem modelItem)
        {
            // Determine the type of icon needed for the given model item
            var iconType = GetIconType(modelItem);
            // Attempt to retrieve the icon image from the Icons dictionary
            var success = IconImage.Icons.TryGetValue(iconType, out BitmapImage icon);
            // If the retrieval was successful, return the icon image
            if(success)
            {
                return icon;
            }
            // If the retrieval was not successful, return null
            else
            {
                return null;
            }
        }

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

        /// <summary>
        /// Removes a custom property category from a ModelItem.
        /// </summary>
        /// <param name="modelItem">
        /// The ModelItem to remove the category from.
        /// </param>
        /// <param name="customPropertyCategory">
        /// The CustomPropertyCategory to remove.
        /// </param>
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

        /// <summary>
        /// Extension method to serialize a ModelItem to a file
        /// </summary>
        /// <param name="modelItem">
        /// </param>
        /// <param name="filePath">
        /// </param>
        public static void Serialize(this ModelItem modelItem, string filePath)
        {
            // Convert the ModelItem's PropertyCategories to a dictionary
            var data = modelItem.PropertyCategories.ToDictionary();

            // Write the dictionary to the specified file path using a StreamWriter
            using(var writer = new StreamWriter(filePath))
            {
                // Use a JsonSerializer to serialize the data to the file
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, data);
            }
        }

        /// <summary>
        /// Serialize a collection of ModelItem objects to a file
        /// </summary>
        /// <param name="modelItems">
        /// </param>
        /// <param name="filePath">
        /// </param>
        public static void Serialize(this IEnumerable<ModelItem> modelItems, string filePath)
        {
            // Convert each ModelItem's PropertyCategories to a dictionary
            var data = modelItems.Select(item => item.PropertyCategories.ToDictionary());
            // Write the serialized data to the specified file
            using(var writer = new StreamWriter(filePath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, data);
            }
        }

        /// <summary>
        /// Serializes a ModelItem object's hierarchy to a JSON file at the given file path
        /// </summary>
        /// <param name="modelItem">
        /// </param>
        /// <param name="filePath">
        /// </param>
        public static void SerializeHierarchy(this ModelItem modelItem, string filePath)
        {
            // Convert the ModelItem object's hierarchy to a dictionary
            var data = modelItem.ToDictionaryHierarchy();
            // Open a StreamWriter to the file path
            using(var writer = new StreamWriter(filePath))
            {
                // Create a new JsonSerializer
                var serializer = new JsonSerializer();
                // Serialize the dictionary to the file using the serializer
                serializer.Serialize(writer, data);
            }
        }

        /// <summary>
        /// Serialize a collection of ModelItems to a file at the specified file path
        /// </summary>
        /// <param name="modelItems">
        /// </param>
        /// <param name="filePath">
        /// </param>
        public static void SerializeHierarchy(this IEnumerable<ModelItem> modelItems, string filePath)
        {
            // Convert the collection to a dictionary hierarchy
            var data = modelItems.ToDictionaryHierarchy();
            // Write the dictionary to the specified file using a StreamWriter
            using(var writer = new StreamWriter(filePath))
            {
                // Use a JsonSerializer to serialize the dictionary to the file
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, data);
            }
        }

        /// <summary>
        /// Converts a collection of ModelItems into a collection of dictionaries
        /// </summary>
        /// <param name="modelItems">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<IDictionary<string, object>> ToDictionary(this IEnumerable<ModelItem> modelItems)
        {
            return modelItems.Select(item => item.PropertyCategories.ToDictionary());
        }

        /// <summary>
        /// Converts a ModelItem into a dictionary hierarchy
        /// </summary>
        /// <param name="modelItem">
        /// </param>
        /// <returns>
        /// </returns>
        public static IDictionary<string, object> ToDictionaryHierarchy(this ModelItem modelItem)
        {
            // Convert the ModelItem's property categories to a dictionary
            var categories = modelItem.PropertyCategories.ToDictionary() as IDictionary<string, object>;
            // Convert the ModelItem's children to a dictionary hierarchy
            var children = modelItem.Children.Select(child => child.ToDictionaryHierarchy());
            // Add the children to the dictionary
            categories.Add("Children", children);
            // Return the dictionary hierarchy
            return categories;
        }

        /// <summary>
        /// Converts a collection of ModelItems into a collection of dictionary hierarchies
        /// </summary>
        /// <param name="modelItems">
        /// </param>
        /// <returns>
        /// </returns>
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

        /// <summary>
        /// Updates a model item with custom property category information
        /// </summary>
        /// <param name="modelItem">
        /// </param>
        /// <param name="customPropertyCategory">
        /// </param>
        public static void Update(this Api.ModelItem modelItem, CustomPropertyCategory customPropertyCategory)
        {
            // Sort the properties in the category alphabetically by display name
            var properties = customPropertyCategory.Properties.OrderBy(property => property.DisplayName);
            // Set the user-defined properties for the model item using the category's display name, name, and sorted properties
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

        /// <summary>
        /// Sets user-defined properties for a given model item
        /// </summary>
        /// <param name="modelItem">
        /// </param>
        /// <param name="userName">
        /// </param>
        /// <param name="internalName">
        /// </param>
        /// <param name="properties">
        /// </param>
        private static void SetUserDefined(Api.ModelItem modelItem, string userName, string internalName, IEnumerable<DataProperty> properties)
        {
            // Create an empty COM category
            var newComCategory = ComApiBridge.State.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null) as InwOaPropertyVec;
            // Create COM properties and add them to the COM category
            foreach(var property in properties)
            {
                var newCOMProperty = ComApiBridge.State.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null) as InwOaProperty;
                // Set the property name
                newCOMProperty.name = property.Name;
                // Set the property display name
                newCOMProperty.UserName = property.DisplayName;
                // Set the property value
                newCOMProperty.value = property.Value.GetValue();
                // Add the new COM property to the COM category
                newComCategory.Properties().Add(newCOMProperty);
            }
            // Convert the ModelItem to a COM Path
            var comModelItem = ComApiBridge.ToInwOaPath(modelItem);
            // Get the item's COM PropertyCategoryCollection
            var comPropertyCategories = ComApiBridge.State.GetGUIPropertyNode(comModelItem, true) as InwGUIPropertyNode2;
            // Find the index of the user-defined category with the given name
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
            // Overwrite the existing category with the new properties
            comPropertyCategories.SetUserDefined(usingIndex, userName, internalName, newComCategory);
        }
    }
}