using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Readers
{
    interface IReader
    {
        T Read<T>(string filePath);
        Task<T> ReadAsync<T>(string filePath);
    }
}
