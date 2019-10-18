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

        public static async Task ChangeSettingsNameAsync(string oldName, string newName)
        {
            if (!await SettingsExists(oldName))
                throw new SettingsNotFoundException(oldName);

            BaseSettings settings = await ReadAsync<BaseSettings>(oldName);
            settings.Name = newName;

            string oldSettingsPath = MakeSettingsPath(oldName);
            string newSettingsPath = MakeSettingsPath(newName);

            File.Move(oldSettingsPath, newSettingsPath);

            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            await File.WriteAllTextAsync(newSettingsPath, json);
        }

        /// <summary>
        /// Save settings asynchronously
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static async Task SaveAsync(BaseSettings settings)
        {
            if (settings == null) 
                throw new ArgumentNullException("settings");

            if (!await SettingsExists(settings.Name)) 
                throw new SettingsNotFoundException(settings.Name);

            string settingsPath = MakeSettingsPath(settings.Name);
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
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

        public static async Task DeleteAsync(string settingsName)
        {
            if (settingsName == null)
                throw new ArgumentNullException("settingsName");

            if (await SettingsExists(settingsName))
                throw new SettingsAlreadyExistsException(settingsName);

            File.Delete(MakeSettingsPath(settingsName));
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
                return await _reader.ReadAsync<T>(MakeSettingsPath(settingsName));
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
        /// Get all settings info
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
        /// Get specified settings info
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
