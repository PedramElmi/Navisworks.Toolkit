using Autodesk.Navisworks.Api;
using Community.Navisworks.Toolkit.Helper;
using System.Collections.Generic;
using System.Linq;

namespace Community.Navisworks.Toolkit
{
    public static class FolderItemExtentions
    {
        /// <summary>
        /// Returns the SelectionSets s inside of the <see cref="FolderItem"/> object
        /// </summary>
        /// <param name="folderItem"></param>
        /// <returns></returns>
        public static IEnumerable<SelectionSet> GetSelectionSets(this FolderItem folderItem)
        {
            // get this item children's selection set
            var thisSelectionSets = folderItem.Children.OfType<SelectionSet>();

            var otherSelectionSets = folderItem.Children.OfType<FolderItem>().Select(child => child.GetSelectionSets());
            
            // intersect
            return otherSelectionSets.Append(thisSelectionSets).IntersectAll();
        }
    }
}