using Autodesk.Navisworks.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NavisworksDevHelper.ModelItem
{

    public static partial class CategoriesPropertiesHelper
    {
        /// <summary>
        /// This class is twin of Autodesk.Navisworks.ModelItem class prepared to JSON serialization
        /// </summary>
        private class ModelItemSerializable
        {
            #region Properties
            public string DisplayName { get; set; }
            public string ClassDisplayName { get; set; }
            public string ClassName { get; set; }
            public string Model { get; set; }
            public Guid? InstanceGuid { get; set; }
            public List<PropertyCategorySerializable> PropertyCategories { get; set; }
            public List<ModelItemSerializable> Children { get; set; }

            #endregion

            #region Constructors

            /// <summary>
            /// Basic constructor to create a ModelItem from the Autodesk.Navisworks.Api.ModelItem class
            /// </summary>
            /// <param name="modelItem"></param>
            public ModelItemSerializable(Autodesk.Navisworks.Api.ModelItem modelItem)
            {
                DisplayName = modelItem.DisplayName;
                ClassDisplayName = modelItem.ClassDisplayName;
                ClassName = modelItem.ClassName;
                InstanceGuid = modelItem.InstanceGuid == Guid.Empty ? null : (Guid?)modelItem.InstanceGuid;
                Model = modelItem.HasModel ? modelItem.Model.FileName : null;

                PropertyCategories = new List<PropertyCategorySerializable>();

                try
                {
                    foreach (var category in modelItem.PropertyCategories)
                    {
                        PropertyCategories.Add(new PropertyCategorySerializable(category));
                    }
                }
                catch (Exception)
                {

                    throw;
                }

                //Parallel.ForEach(modelItem.PropertyCategories, (category) =>
                //{
                //    PropertyCategories.Add(new PropertyCategorySerializable(category));
                //});

                Children = new List<ModelItemSerializable>();
                if (modelItem.Children.First != null)
                {
                    foreach (var item in modelItem.Children)
                    {
                        Children.Add(new ModelItemSerializable(item));
                    }

                    //Parallel.ForEach(modelItem.Children, (child) =>
                    //{
                    //    Children.Add(new ModelItemSerializable(child));
                    //});

                }
            }
            #endregion

        }

        /// <summary>
        /// This class is twin of Autodesk.Navisworks.PropertyCategory class prepared to JSON serialization
        /// </summary>
        private class PropertyCategorySerializable
        {
            public string DisplayName { get; set; }
            public string Name { get; set; }
            public List<DataPropertySerializable> Properties { get; set; }

            public PropertyCategorySerializable(Autodesk.Navisworks.Api.PropertyCategory category)
            {
                DisplayName = category.DisplayName;
                Name = category.Name;
                Properties = new List<DataPropertySerializable>();


                try
                {
                    foreach (var property in category.Properties)
                    {
                        Properties.Add(new DataPropertySerializable(property));
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }

        }

        /// <summary>
        /// This class is twin of Autodesk.Navisworks.DataProperty class prepared to JSON serialization
        /// </summary>
        private class DataPropertySerializable
        {
            public string DisplayName { get; set; }
            public string Name { get; set; }
            public object Value { get; set; }
            public string ValueType { get; set; }

            public DataPropertySerializable(DataProperty property)
            {
                DisplayName = property.DisplayName;
                Name = property.Name;


                try
                {
                    Value = GetCleanedVariantData(property.Value);
                }
                catch (Exception)
                {

                    throw;
                }
                

                ValueType = property.Value.DataType.ToString();
            }


        }

        /// <summary>
        /// This class helps to order properties alphabetically in JSON file. use it in serialize settings
        /// </summary>
        private class OrderedContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
        {
            protected override System.Collections.Generic.IList<Newtonsoft.Json.Serialization.JsonProperty> CreateProperties(System.Type type, Newtonsoft.Json.MemberSerialization memberSerialization)
            {
                var @base = base.CreateProperties(type, memberSerialization);
                var ordered = @base
                    .OrderBy(p => p.Order ?? int.MaxValue)
                    .ThenBy(p => p.PropertyName)
                    .ToList();
                return ordered;
            }
        }

    }

}
