using System.Reflection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using SharpRevit.Classes;
using SharpRevit.Utilities;

namespace sharpRevit.PlaceOriginMarkerButton
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class PlaceOriginMarkerCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = commandData.Application.ActiveUIDocument;
            var doc = uiDoc.Document;

            try
            {
                //get active view
                var activeView = doc.ActiveView;

                if (activeView.ViewType is ViewType.AreaPlan or ViewType.DraftingView or ViewType.FloorPlan or ViewType.CeilingPlan or ViewType.Legend or ViewType.Elevation or ViewType.Section or ViewType.EngineeringPlan or ViewType.ThreeD)
                {
                    var viewOriginPoint = activeView.Origin;
                    var viewOrientation = activeView.ViewDirection;


                    var origin = XYZ.Zero;

                    XYZ lineOneStart = new XYZ(origin.X - 1, origin.Y, origin.Z);
                    XYZ lineOneEnd = new XYZ(origin.X + 1, origin.Y, origin.Z);

                    XYZ lineTwoStart = new XYZ(origin.X, origin.Y - 1, origin.Z);
                    XYZ lineTwoEnd = new XYZ(origin.X, origin.Y + 1, origin.Z);


                    var firstLine = Line.CreateBound(lineOneStart, lineOneEnd);
                    var secondLine = Line.CreateBound(lineTwoStart, lineTwoEnd);

                    var firstPlan = new FilteredElementCollector(doc).OfClass(typeof(ViewPlan)).WhereElementIsNotElementType().Cast<ViewPlan>().First(p => !p.IsTemplate);

                    //we drew the cross hair on the floor, so transform it
                    var tForm = ElementTransformUtils.GetTransformFromViewToView(firstPlan, activeView);

                    var firstCurve = firstLine.CreateTransformed(tForm);
                    var secondCurve = secondLine.CreateTransformed(tForm);

                    using (Transaction createLinesTransaction = new Transaction(doc, $"Creating cross hairs at origin of {activeView.Name}."))
                    {
                        createLinesTransaction.Start();
                        if (doc.IsFamilyDocument)
                        {
                            var plane = Plane.CreateByNormalAndOrigin(viewOrientation, viewOriginPoint);
                            var sketchPlane = SketchPlane.Create(doc, plane);

                            var firstSymbolicLine = doc.FamilyCreate.NewSymbolicCurve(firstCurve, sketchPlane);
                            var secondSymbolicLine = doc.FamilyCreate.NewSymbolicCurve(secondCurve, sketchPlane);

                        }
                        else
                        {
                            var firstDetailLine = doc.Create.NewDetailCurve(activeView, firstCurve);
                            var secondDetailLine = doc.Create.NewDetailCurve(activeView, secondCurve);
                        }
                      
                        createLinesTransaction.Commit();
                    }
                   
                }
            }
            catch (Exception)
            {
              return Result.Failed;
            }

            return Result.Succeeded;
        }
        public static void CreateButton(RibbonPanel panel)
        {
            var assembly = Global.ExecutingAssembly;

            var pushButtonData = new PushButtonData(
                MethodBase.GetCurrentMethod().DeclaringType?.Name,
                "Place Origin" + Environment.NewLine + "Marker",
                assembly.Location,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName)
            {
                ToolTip = "Place cross hairs at origin of the active view.",
                LargeImage = ImageUtils.LoadImage(assembly, $"SharpRevit.PlaceOriginMarker_32{Global.UITheme}.png")
            };

            var pushButton = panel.AddItem(pushButtonData) as PushButton;

            Global.RibbonButtons.Add(pushButton);
        }
    }
}
