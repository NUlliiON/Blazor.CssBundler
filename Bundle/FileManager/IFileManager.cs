using System.Threading.Tasks;

namespace Blazor.CssBundler.Bundle.FileManager
{
    public interface IFileManager
    {
        string TempStylesFilePath { get; set; }

        bool TempCssFileExists();
        Task ClearTmpCssFileAsync();
        Task CreateTmpCssFileAsync();
        Task DeleteTmpCssFileAsync();
    }
}
