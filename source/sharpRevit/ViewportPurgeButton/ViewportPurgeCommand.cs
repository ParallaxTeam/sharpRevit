using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SharpRevit.Classes;
using SharpRevit.Utilities;
using SharpRevit.ViewportPurgeButton;

namespace SharpRevit.ViewportPurgeButton
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class ViewportPurgeCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = commandData.Application.ActiveUIDocument;
            var doc = uiDoc.Document;

            var m = new ViewportPurgeModel(uiApp);
            var vm = new ViewportPurgeViewModel(m);
            var v = new ViewportPurgeView()
            {
                DataContext = vm
            };

            v.ShowDialog();

            return Result.Succeeded;
        }
        public static void CreateButton(RibbonPanel panel)
        {
            var assembly = Global.ExecutingAssembly;

            var pushButtonData = new PushButtonData(
                MethodBase.GetCurrentMethod().DeclaringType?.Name,
                "Viewport" + Environment.NewLine + "Purge",
                assembly.Location,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName)
            {
                ToolTip = "Wipe Unpurgeable Viewports",
                LargeImage = ImageUtils.LoadImage(assembly, $"SharpRevit.ViewportPurge_32{Global.UITheme}.png")
            };

            var pushButton = panel.AddItem(pushButtonData) as PushButton;

            Global.RibbonButtons.Add(pushButton);
        }
    }
}
