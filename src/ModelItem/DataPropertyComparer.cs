using Autodesk.Navisworks.Api;
using System.Collections.Generic;

namespace Community.Navisworks.Toolkit
{
    /// <summary>
    /// A DataProperty Comparer which checks both <see cref="DataProperty.Name"/> and <see cref="DataProperty.DisplayName"/> match
    /// in both <see cref="DataProperty"/> objects. It does not consider <see cref="DataProperty.Value"/>.
    /// </summary>
    public class DataPropertyComparer : IEqualityComparer<DataProperty>
    {
        public bool Equals(DataProperty x, DataProperty y)
        {
            //Check whether the compared objects reference the same data.
            if(object.ReferenceEquals(x, y))
                return true;

            //Check whether any of the compared objects is null.
            if(x is null || y is null)
                return false;

            //Check whether the products' properties are equal.
            return x.Name == y.Name && x.DisplayName == y.DisplayName;
        }

        // If Equals() returns true for a pair of objects then GetHashCode() must return the same value for these objects.

        public int GetHashCode(DataProperty obj)
        {
            //Check whether the object is null
            if(obj is null)
                return 0;

            //Get hash code for the Name field if it is not null.
            int hasDataPropertyName = (obj.Name?.GetHashCode()) ?? 0;

            //Get hash code for the DisplayName field.
            int hasDataPropertyDisplayName = obj.DisplayName.GetHashCode();

            //Calculate the hash code for the product.
            return hasDataPropertyName ^ hasDataPropertyDisplayName;
        }
    }
}