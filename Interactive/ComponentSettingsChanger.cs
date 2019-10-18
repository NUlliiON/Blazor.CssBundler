using Blazor.CssBundler.Models.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Interactive
{
    class ComponentSettingsChanger : BaseSettingsChanger<ComponentSettings>
    {
        //protected override PropertySelectionItem[] GetPropertySelectionItems(ComponentSettings settings)
        //{
        //    return new[]
        //    {
        //        new PropertySelectionItem("ProjectName",           settings.ProjectName,           position: 1),
        //        new PropertySelectionItem("ProjectFilePath",       settings.ProjectFilePath,       position: 2),
        //        new PropertySelectionItem("ProjectDirectory",      settings.ProjectDirectory,      position: 3),
        //        new PropertySelectionItem("AssemblyPath",          settings.AssemblyPath,          position: 4),
        //        new PropertySelectionItem("CssRazorSearchPattern", settings.CssRazorSearchPattern, position: 5)
        //    };
        //}

        //protected override ComponentSettings Map(PropertySelectionItem[] propertyItems)
        //{
        //    return new ComponentSettings
        //    {
        //        ProjectName = propertyItems[0].Value,
        //        ProjectFilePath = propertyItems[1].Value,
        //        ProjectDirectory = propertyItems[2].Value,
        //        AssemblyPath = propertyItems[3].Value,
        //        CssRazorSearchPattern = propertyItems[4].Value,
        //    };
        //}
    }
}
