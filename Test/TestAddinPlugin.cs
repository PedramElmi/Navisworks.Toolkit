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
        public int age = 10;
        
        public override int Execute(params string[] parameters)
        {

            var selectedModelItems = Application.ActiveDocument.CurrentSelection.SelectedItems;

            var dataProperty = new DataProperty("PersonInternal", "First Name", new VariantData("Mohammad"));
            var dataProperty2 = new DataProperty("AgeInternal", "Age", new VariantData(age));

            var category = new CustomPropertyCategory
            {
                DisplayName = "Person"
            };

            category.Properties.Add(dataProperty);
            category.Properties.Add(dataProperty2);

            selectedModelItems.AddCustomPropertyCategory(category);
            
            age++;
            return 0;

        }
    }
}
