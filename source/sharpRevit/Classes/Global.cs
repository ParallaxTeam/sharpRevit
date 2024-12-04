using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Autodesk.Revit.UI;

namespace SharpRevit.Classes
{
    /// <summary>
    /// Class for global settings and stored items
    /// </summary>
    internal class Global
    {
        internal static Assembly ExecutingAssembly = Assembly.GetExecutingAssembly();
        internal static List<RibbonButton> RibbonButtons = new List<RibbonButton>();
        internal static string ExecutingPath = Path.GetDirectoryName(ExecutingAssembly.Location);
        internal static string TempPath = Environment.GetEnvironmentVariable("TMP", EnvironmentVariableTarget.User);
        internal static string UITheme { get; set; } = "-Light";

        internal static string Version = ExecutingAssembly.GetName().Version.ToString();
    }
}
