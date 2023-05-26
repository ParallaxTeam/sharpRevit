using Autodesk.Revit.UI;
using SharpRevit.Classes;
using SharpRevit.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using Prlx.TemplateTools.TemplateButton;
using SharpRevit.TemplateButton;

namespace SharpRevit
{
    internal class AppCommand : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            CreateRibbon(application);

            #if Revit2024
            application.ThemeChanged += ThemeChanged;

            Global.UITheme = UIThemeManager.CurrentTheme == UITheme.Light
                ? "-Light"
                : "-Dark";
            #endif

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }


        /// <summary>
        /// Create our panel on the the addin tab
        /// </summary>
        /// <param name="app"></param>
        internal void CreateRibbon(UIControlledApplication app)
        {
            //try
            //{
            //    app.CreateRibbonTab("sharpRevit");
            //}
            //catch (Exception)
            //{
            //    //ribbon already exists, continue
            //}

            //add to add-ins for now
            var ribbonPanel = app.CreateRibbonPanel( $"sharpRevit v.{Global.Version}");


            ViewportPurgeButton.ViewportPurgeCommand.CreateButton(ribbonPanel);

            //worksharing display commands aka "team" from pyRevit
            SplitButtonData splitButtonData = new SplitButtonData("Team", "Team")
            {
                ToolTip = "Tools to find out what is happening",
                LargeImage = ImageUtils.LoadImage(Global.ExecutingAssembly, $"SharpRevit.Team_32{Global.UITheme}.png")
            };
            var splitButton = ribbonPanel.AddItem(splitButtonData) as PulldownButton;
            Global.RibbonButtons.Add(splitButton);

            WhoCreatedThatCommand.CreateButton(splitButton);
            WhoCreatedThisViewCommand.CreateButton(splitButton);
        }

#if Revit2024
        /// <summary>
        /// Changes image based on theme setting
        /// </summary>
        /// <param name="mode"></param>
        private void SetButtonImage(string mode)
        {
            foreach (var ribbonButton in Global.RibbonButtons)
            {
                string commandName = ribbonButton.Name.Replace("Command", "");
                ribbonButton.LargeImage = ImageUtils.LoadImage(Global.ExecutingAssembly, $"SharpRevit.{commandName}_32{mode}.png");
            }

          
        }
        /// <summary>
        /// Handler for theme changes to update ribbon icons
        /// </summary>
        private void UpdateImageByTheme()
        {
            UITheme theme = UIThemeManager.CurrentTheme;
            switch (theme)
            {
                case UITheme.Dark:
                    SetButtonImage("-Dark");
                    Global.UITheme = "-Dark";
                    break;
                case UITheme.Light:
                    SetButtonImage("-Light");
                    Global.UITheme = "-Light";
                    break;
            }
        }
        /// <summary>
        /// Theme changed event handler 2024+ only
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThemeChanged(object sender, Autodesk.Revit.UI.Events.ThemeChangedEventArgs e)
        {
            UpdateImageByTheme();
        }
#endif
    }
}
