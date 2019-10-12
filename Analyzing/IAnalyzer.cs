using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Analyzing
{
    interface IAnalyzer<T>
    {
        T Analyze(string path);
    }
}
