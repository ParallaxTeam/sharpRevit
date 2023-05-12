using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SharpRevit.Classes;

namespace SharpRevit.ViewportPurgeButton
{
    internal class ViewportPurgeModel
    {
        public UIApplication UiApp { get; }
        public Document Doc { get; }
        public UIDocument UiDoc { get; }
        internal ViewportPurgeModel(UIApplication uiApp)
        {
            UiApp = uiApp;
            UiDoc = uiApp.ActiveUIDocument;
            Doc = uiApp.ActiveUIDocument.Document;
        }

        internal ObservableCollection<ViewportTypeWrapper> CollectViewportTypes()
        {
            ElementCategoryFilter viewportFilter = new ElementCategoryFilter(BuiltInCategory.OST_Viewports);

            List<ViewportTypeWrapper> wrappedViewportTypes = new List<ViewportTypeWrapper>();

            var allViewportTypes = new FilteredElementCollector(Doc).OfClass(typeof(ElementType)).Cast<ElementType>().Where(v => v.FamilyName.Equals("Viewport")).ToList();


            foreach (var viewportType in allViewportTypes)
            {
                var dependentViewports = viewportType.GetDependentElements(viewportFilter).ToList();

                var viewports = dependentViewports.Select(d => Doc.GetElement(d) as Viewport);

                ViewportTypeWrapper viewportTypeWrapper = new ViewportTypeWrapper(viewportType, new ObservableCollection<Viewport>(viewports));
                wrappedViewportTypes.Add(viewportTypeWrapper);
            }

            return new ObservableCollection<ViewportTypeWrapper>(wrappedViewportTypes);
        }

        internal ObservableCollection<ElementType> CollectTargetViewportTypes(ObservableCollection<ViewportTypeWrapper> wrappedViewportTypes)
        {
            List<ElementType> targetViewportTypes = new List<ElementType>();

            foreach (var wrappedViewportType in wrappedViewportTypes)
            {
                if (!wrappedViewportType.Delete)
                {
                    targetViewportTypes.Add(wrappedViewportType.ViewportType);
                }
            }

            return new ObservableCollection<ElementType>(targetViewportTypes);
        }

        internal string SwapViewportTypes(List<ViewportTypeWrapper> wrappedViewportTypes,
            ElementType target, ViewportPurgeViewModel vm)
        {
            string log = string.Empty;

            using (Transaction t = new Transaction(Doc, "Swapping Viewports"))
            {
                t.Start();

                foreach (var wrappedViewportType in wrappedViewportTypes)
                {
                    foreach (var viewport in wrappedViewportType.DependentViewports)
                    {
                        try
                        {
                            var potentialLog = $"View: {Doc.GetElement(viewport.ViewId).Name}:{viewport.Id} -  changed to {target.Name}{Environment.NewLine}";

                            viewport.ChangeTypeId(target.Id);

                            log = $"{log}{potentialLog}";
                        }
                        catch (Exception e)
                        {
                            var potentialLog = $"View: {Doc.GetElement(viewport.ViewId).Name} : {viewport.Id}:{viewport.Id} -  failed.";
                            log = $"{log}{potentialLog}";
                        }

                        vm.ProgressValue++;
                    }
                }

                t.Commit();
            }

            return log;
        }

    }
}
