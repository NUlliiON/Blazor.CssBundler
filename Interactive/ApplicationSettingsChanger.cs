using Blazor.CssBundler.Models.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blazor.CssBundler.Interactive
{
    class ApplicationSettingsChanger : BaseSettingsChanger<ApplicationSettings>
    {
        //protected override PropertySelectionItem[] GetPropertySelectionItems(ApplicationSettings settings)
        //{
        //    return new[]
        //    {
        //        new PropertySelectionItem("ProjectName",           settings.ProjectName,           position: 1),
        //        new PropertySelectionItem("ProjectFilePath",       settings.ProjectFilePath,       position: 2),
        //        new PropertySelectionItem("ProjectDirectory",      settings.ProjectDirectory,      position: 3),
        //        new PropertySelectionItem("OutputCssDirectory",    settings.OutputCssDirectory,    position: 4),
        //        new PropertySelectionItem("GlobalStylesPath",      settings.GlobalStylesPath,      position: 5),
        //        new PropertySelectionItem("CssRazorSearchPattern", settings.CssRazorSearchPattern, position: 6)
        //    };
        //}

        //protected override ApplicationSettings Map(PropertySelectionItem[] propertyItems)
        //{
        //    return new ApplicationSettings
        //    {
        //        ProjectName = propertyItems[0].Value,
        //        ProjectFilePath = propertyItems[1].Value,
        //        ProjectDirectory = propertyItems[2].Value,
        //        OutputCssDirectory = propertyItems[3].Value,
        //        GlobalStylesPath = propertyItems[4].Value,
        //        CssRazorSearchPattern = propertyItems[5].Value
        //    };
        //}
    }
}
