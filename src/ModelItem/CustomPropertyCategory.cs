using Autodesk.Navisworks.Api;

namespace PedramElmi.Navisworks.Toolkit
{
    /// <summary>
    /// Represents a custom user-defined PropertyCategory that can be added to a ModelItem.
    /// </summary>
    public class CustomPropertyCategory
    {
        /// <summary>
        /// Initializes a new instance of the CustomPropertyCategory class with the specified display name.
        /// </summary>
        /// <param name="displayName">
        /// The display name of the category.
        /// </param>
        public CustomPropertyCategory(string displayName)
        {
            DisplayName = displayName;
        }

        /// <summary>
        /// Gets the combined name of the category.
        /// </summary>
        public NamedConstant CombinedName { get => new NamedConstant(Name, DisplayName); }

        /// <summary>
        /// Gets or sets the display name of the category.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets the name of the category that is suitable for programmatic use. This name cannot be edited in Navisworks.
        /// </summary>
        public string Name { get => "LcOaPropOverrideCat"; }

        /// <summary>
        /// Gets or sets the collection of properties in the category.
        /// </summary>
        public DataPropertyCollection Properties { get; set; } = new DataPropertyCollection();
    }
}