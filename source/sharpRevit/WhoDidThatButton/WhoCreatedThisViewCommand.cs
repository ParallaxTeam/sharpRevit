using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using SharpRevit.Classes;
using SharpRevit.Utilities;

namespace Prlx.TemplateTools.TemplateButton
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class WhoCreatedThisViewCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = commandData.Application.ActiveUIDocument;
            var doc = uiDoc.Document;



            try
            {
                var activeView = doc.ActiveView;

                var tooltipInfo = WorksharingUtils.GetWorksharingTooltipInfo(doc, activeView.Id);

                var result = doc.IsWorkshared ? $"{activeView.Name} created by {tooltipInfo.Creator}. Last updated by {tooltipInfo.LastChangedBy}. Owned by {tooltipInfo.Owner}." : $"{activeView.Name} created by {tooltipInfo.Creator}.";
               

                TaskDialog.Show("Who did that?", result);
            }
            catch (Exception)
            {
              return Result.Failed;
            }

            return Result.Succeeded;
        }
        public static void CreateButton(PulldownButton splitButton)
        {
            var assembly = Global.ExecutingAssembly;

             var pushButtonData = new PushButtonData(
                MethodBase.GetCurrentMethod().DeclaringType?.Name,
                "Who Created" + Environment.NewLine + "This View",
                assembly.Location,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName)
            {
                ToolTip = "Find out who created the active view.",
                LargeImage = ImageUtils.LoadImage(assembly, $"SharpRevit.WhoCreatedThisView_32{Global.UITheme}.png")
            };

             var pushButton = splitButton.AddPushButton(pushButtonData) as PushButton;

             Global.RibbonButtons.Add(pushButton);
        }
    }
}
