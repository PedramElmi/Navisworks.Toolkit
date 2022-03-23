using Autodesk.Navisworks.Api.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NavisworksDevHelper;
using NavisworksDevHelper.ModelItemHelpers;
using Autodesk.Navisworks.Api;
using System.Diagnostics;

namespace Test
{
    [Plugin("NavisworksDevHelperTest", "PedramElmi", DisplayName = "Navisworks Development Helper Test")]
    [AddInPlugin(AddInLocation.AddIn)]
    public class TestAddinPlugin : AddInPlugin
    {        
        public override int Execute(params string[] parameters)
        {

            var selectedModelItems = Application.ActiveDocument.CurrentSelection.SelectedItems;

            CategoriesPropertiesHelper.SerializeModelItems(selectedModelItems, "C:\\Users\\Pedram\\Desktop\\devhelperserialization.json", sortAlphabetically: true, indentedFormat: false, namingStrategy: NamingStrategySerialization.CamelCase);

            return 0;

        }
    }
}
