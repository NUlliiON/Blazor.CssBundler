﻿using Blazor.CssBundler.Models.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Interactive
{
    interface ISettingsChanger<T> where T : BaseSettings
    {
        T Map(PropertySelectionItem[] propertyItems);
    }
}