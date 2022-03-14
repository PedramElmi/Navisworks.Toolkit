using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.ComApi;
using Autodesk.Navisworks.Api.Interop.ComApi;
using System.Linq;

namespace NavisworksDevHelper.ModelItemHelpers
{
    /// <summary>
    /// A custom user-defined Category to be added to ModelItems' PropertyCategories
    /// </summary>
    public class CustomPropertyCategory
    {
        #region Public Properties

        /// <summary>
        /// Combined name of category
        /// </summary>
        public NamedConstant CombinedName { get => new NamedConstant(Name, DisplayName); }

        /// <summary>
        /// Display name of category (localized)
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Name of category (suitable for programmatic use) and apparently, it cannot be edited in
        /// Navisworks 2022.1
        /// </summary>
        public string Name { get => "LcOaPropOverrideCat"; }

        /// <summary>
        /// The properties in this category
        /// </summary>
        public DataPropertyCollection Properties { get; set; } = new DataPropertyCollection();

        #endregion Public Properties

        #region Public Methods

        public void AddToModelItems(ModelItemCollection modelItems)
        {
            foreach (var modelItem in modelItems)
            {
                // category does not exist unless it is found.
                var categoryExists = false;

                // define properties
                DataPropertyCollection properties = new DataPropertyCollection();

                // find category if exists
                var category = modelItem.PropertyCategories.FindCategoryByCombinedName(CombinedName);

                // if category exists, add existing properties
                if (category != null)
                {
                    categoryExists = true;

                    // retrieve existing Properties
                    var existingProperties = category.Properties;

                    // merge with new properties
                    var unionProperties = Properties.Union(existingProperties, new DataPropertyComparer());

                    // sort alphabetically
                    var sortedUnionProperties = unionProperties.OrderBy(property => property.DisplayName);

                    properties.AddRange(sortedUnionProperties);
                }

                if (!categoryExists)
                {
                    // sort alphabetically
                    properties.AddRange(Properties.OrderBy(property => property.DisplayName));
                }

                SetUserDefined(modelItem, properties, categoryExists);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void SetUserDefined(ModelItem modelItem, DataPropertyCollection properties, bool overwrite)
        {
            // convert ModelItem to COM Path
            var comModelItem = ComApiBridge.ToInwOaPath(modelItem);

            // Get item's COM PropertyCategoryCollection
            var comPropertyCategories = ComApiBridge.State.GetGUIPropertyNode(comModelItem, true) as InwGUIPropertyNode2;

            // create empty COM category
            var newComCategory = ComApiBridge.State.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null) as InwOaPropertyVec;

            // create COM properties and add them to Com category
            foreach (var property in properties)
            {
                var newCOMProperty = ComApiBridge.State.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null) as InwOaProperty;

                // set property name
                newCOMProperty.name = property.Name;

                // set property display name
                newCOMProperty.UserName = property.DisplayName;

                // set property value
                newCOMProperty.value = property.Value.Cast<object>();

                // add new COM property to COM category
                newComCategory.Properties().Add(newCOMProperty);
            }

            if (overwrite)
            {
                // overwrite the existing category with newly properties
                comPropertyCategories.SetUserDefined(1, DisplayName, Name, newComCategory);
            }
            else
            {
                // adding the new category with newly properties
                comPropertyCategories.SetUserDefined(0, DisplayName, Name, newComCategory);
            }
        }

        #endregion Private Methods
    }
}