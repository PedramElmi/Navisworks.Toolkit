using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.ComApi;
using Autodesk.Navisworks.Api.Interop.ComApi;
using System.Collections.Generic;
using System.Linq;

namespace PedramElmi.Navisworks.Toolkit
{
    /// <summary>
    /// A custom user-defined PropertyCategory to be added to <see cref="Autodesk.Navisworks.Api.ModelItem"/> <seealso cref="PropertyCategory"/>
    /// </summary>
    public class CustomPropertyCategory
    {

        /// <summary>
        /// Combined name of category
        /// </summary>
        public NamedConstant CombinedName { get => new NamedConstant(Name, DisplayName); }

        /// <summary>
        /// Display name of category (localized)
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Name of category (suitable for programmatic use) and apparently, it cannot be edited in Navisworks 2022.1
        /// </summary>
        public string Name { get => "LcOaPropOverrideCat"; }

        /// <summary>
        /// The properties in this category
        /// </summary>
        public DataPropertyCollection Properties { get; set; } = new DataPropertyCollection();

        public CustomPropertyCategory(string displayName)
        {
            DisplayName = displayName;
        }
    }
}