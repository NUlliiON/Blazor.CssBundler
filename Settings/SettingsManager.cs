using Blazor.CssBundler.Exceptions;
using Blazor.CssBundler.Models.Settings;
using Blazor.CssBundler.Readers;
using JsonSubTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Settings
{
    public class SettingsManager
    {
        private static readonly string _rootSettingsDir = Path.Combine(Environment.CurrentDirectory, "settings");
        private static readonly string _settingsFileExtension = "json";
        private static readonly IReader _reader = new Readers.JsonReader();

        /// <summary>
        /// Save settings asynchronously
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="settingsName">settings name</param>
        /// <returns></returns>
        public static async Task SaveAsync(object settings, string settingsName)
        {
            if (settings == null) 
                throw new ArgumentNullException("settings");

            if (settingsName == null) 
                throw new ArgumentNullException("settingsName");

            if (!await SettingsExists(settingsName)) 
                throw new SettingsNotFoundException(settingsName);

            string settingsPath = MakeSettingsPath(settingsName);
            string json = JsonConvert.SerializeObject(settings);
            await File.WriteAllTextAsync(settingsPath, json);
        }

        /// <summary>
        /// Create new settings asynchronously
        /// </summary>
        /// <param name="settingsName"></param>
        /// <returns></returns>
        public static async Task CreateAsync(string settingsName)
        {
            if (settingsName == null)
                throw new ArgumentNullException("settingsName");

            if (await SettingsExists(settingsName))
                throw new SettingsAlreadyExistsException(settingsName);
                
            await File.WriteAllTextAsync(MakeSettingsPath(settingsName), "");
        }

        /// <summary>
        /// Read settings asynchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingsName">settings name</param>
        /// <returns></returns>
        public static async Task<T> ReadAsync<T>(string settingsName) where T : BaseSettings
        {
            try
            {
                string settingsPath = await GetAboutAllSettings()
                    .Where(x => x.name == settingsName)
                    .Select(x => x.path).FirstOrDefaultAsync();

                return await _reader.ReadAsync<T>(settingsPath);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Checking for the existence of settings with the specified name
        /// </summary>
        /// <param name="settingsName">settings name</param>
        /// <returns></returns>
        public static async Task<bool> SettingsExists(string settingsName) => await GetAboutAllSettings().AnyAsync(x => x.name == settingsName);

        /// <summary>
        /// Get all settings
        /// </summary>
        /// <returns></returns>
        public static async IAsyncEnumerable<(string name, string path, SettingsType type, DateTime lastChangingTime)> GetAboutAllSettings()
        {
            foreach (FileInfo file in new DirectoryInfo("settings").GetFiles($"*.{_settingsFileExtension}", SearchOption.TopDirectoryOnly))
            {
                string fileName = Path.GetFileNameWithoutExtension(file.Name);
                BaseSettings settings = await ReadAsync<BaseSettings>(fileName);
                if (settings?.Name == fileName)
                {
                    yield return (settings.Name, file.FullName, settings.Type, file.LastWriteTime);
                }
            }
        }

        /// <summary>
        /// Get specified settings
        /// </summary>
        /// <param name="settingsType">settings type</param>
        ///// <returns></returns>
        public static async IAsyncEnumerable<(string name, string path, SettingsType type, DateTime lastChaningTime)> GetAboutSettings(SettingsType settingsType)
        {
            foreach (FileInfo file in new DirectoryInfo("settings").GetFiles($"*.{_settingsFileExtension}", SearchOption.TopDirectoryOnly))
            {
                string fileName = Path.GetFileNameWithoutExtension(file.Name);
                BaseSettings settings = await ReadAsync<BaseSettings>(fileName);
                if (settings?.Name == fileName && settings?.Type == settingsType)
                {
                    yield return (settings.Name, file.FullName, settings.Type, file.LastWriteTime);
                }
            }
        }

        /// <summary>
        /// Make full path to settings
        /// </summary>
        /// <param name="settingsName">settings name</param>
        /// <returns></returns>
        private static string MakeSettingsPath(string settingsName)
        {
            return Path.Combine(_rootSettingsDir, $"{settingsName}.{_settingsFileExtension}");
        }
    }
}
