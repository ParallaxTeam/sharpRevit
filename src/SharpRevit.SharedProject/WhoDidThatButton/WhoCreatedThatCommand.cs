using System;
using System.Reflection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using SharpRevit.Classes;
using SharpRevit.Utilities;

namespace SharpRevit.TemplateButton
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class WhoCreatedThatCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = commandData.Application.ActiveUIDocument;
            var doc = uiDoc.Document;

            try
            {
                var currentSelection =
                    uiDoc.Selection.PickObject(ObjectType.Element, "Select an element to find out who made it");

                var tooltipInfo = WorksharingUtils.GetWorksharingTooltipInfo(doc, currentSelection.ElementId);

                var result = doc.IsWorkshared ? $"{doc.GetElement(currentSelection).Name} created by {tooltipInfo.Creator}. Last updated by {tooltipInfo.LastChangedBy}. Owned by {tooltipInfo.Owner}." : $"{doc.GetElement(currentSelection).Name} created by {tooltipInfo.Creator}.";

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
                "Who Created" + Environment.NewLine + "Selected Element",
                assembly.Location,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName)
            {
                ToolTip = "Find out who created a selected element.",
                LargeImage = ImageUtils.LoadImage(assembly, $"SharpRevit.WhoCreatedThat_32{Global.UITheme}.png")
            };

             var pushButton = splitButton.AddPushButton(pushButtonData) as PushButton;

             Global.RibbonButtons.Add(pushButton);
        }
    }
}
