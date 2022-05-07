using Autodesk.Navisworks.Api;
using System.Collections.Generic;
using System.Linq;

namespace PedramElmi.Navisworks.Toolkit
{
    public static class FolderItemExtentions
    {
        /// <summary>
        /// Returns the SelectionSets s inside of the <see cref="FolderItem"/> object
        /// </summary>
        /// <param name="folderItem"></param>
        /// <returns></returns>
        public static HashSet<SelectionSet> GetSelectionSets(this FolderItem folderItem)
        {
            // get this item children's selection set
            var thisSelectionSets = folderItem.Children.Where(child => child is SelectionSet).Select(child => child as SelectionSet);

            var selectionSets = folderItem.Children.Where(child => child is FolderItem).Select(child => child as FolderItem).Select(child => child.GetSelectionSets()).Append(thisSelectionSets);

            // intersect
            return selectionSets
                .Skip(1)
                .Aggregate(
                new HashSet<SelectionSet>(selectionSets.First()),
                (h, e) => { h.IntersectWith(e); return h; });
        }
    }
}