using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.DocumentParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Navisworks.Toolkit
{
    public static class DocumentSelectionSetsExtentions
    {
        /// <summary>
        /// Returns all <see cref="SelectionSet"/>s inside of the Document
        /// </summary>
        /// <param name="documentSelectionSets"></param>
        /// <returns></returns>
        public static IEnumerable<SelectionSet> GetSelectionSets(this DocumentSelectionSets documentSelectionSets)
        {
            return documentSelectionSets.RootItem.GetSelectionSets();
        }
    }
}
