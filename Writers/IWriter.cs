using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Writers
{
    interface IWriter
    {
        void Write<T>(string filePath, T data);
        Task WriteAsync<T>(string filePath, T data);
    }
}
