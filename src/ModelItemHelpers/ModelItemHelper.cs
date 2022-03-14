using Autodesk.Navisworks.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NavisworksDevHelper.ModelItemHelpers
{
    public static class ModelItemHelper
    {
        public static IEnumerable<ModelItem> GetSpecificModelItems(IEnumerable<ModelItem> modelItems, bool withDecendants, bool onlyHasInstanceGuid, bool includeModels)
        {
            if (withDecendants)
            {
                var temporaryModelItems = new ModelItemCollection();
                temporaryModelItems.AddRange(modelItems);
                modelItems = temporaryModelItems.DescendantsAndSelf;
            }
            if (onlyHasInstanceGuid)
            {
                modelItems = modelItems.Where(modelItem => modelItem.InstanceGuid != Guid.Empty);
            }
            if (!includeModels)
            {
                modelItems = modelItems.Where(modelItem => modelItem.ClassDisplayName != "File");
            }
            return modelItems;
        }
    }
}
