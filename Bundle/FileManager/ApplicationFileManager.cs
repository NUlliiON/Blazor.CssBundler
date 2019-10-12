using System;
using System.IO;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Bundle.FileManager
{
    public class ApplicationFileManager : IFileManager
    {
        public string TempStylesFilePath { get; set; }

        public bool TempCssFileExists() => File.Exists(TempStylesFilePath);


        /// <summary>
        /// Clear temporary css file asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task ClearTmpCssFileAsync()
        {
            if (!File.Exists(TempStylesFilePath))
            {
                throw new FileNotFoundException("Temp styles file not found by path: " + TempStylesFilePath);
            }

            await File.WriteAllTextAsync(TempStylesFilePath, "");
        }

        /// <summary>
        /// Create temporary css file asynchronously
        /// </summary>
        /// <returns></returns>
        public Task CreateTmpCssFileAsync()
        {
            if (File.Exists(TempStylesFilePath))
            {
                throw new Exception("Temp styles exists by path: " + TempStylesFilePath);
            }

            return Task.Run(() =>
            {
                File.Create(TempStylesFilePath).Close();
            });
        }

        /// <summary>
        /// Delete temporary css file asynchronously
        /// </summary>
        /// <returns></returns>
        public Task DeleteTmpCssFileAsync()
        {
            if (!File.Exists(TempStylesFilePath))
            {
                throw new FileNotFoundException("Temp styles file not found by path: " + TempStylesFilePath);
            }

            return Task.Run(() => File.Delete(TempStylesFilePath));
        }
    }
}
