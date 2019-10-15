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
        private static readonly IReader _reader = new Readers.JsonReader();
        private static readonly string _settingsFileExtension = "json";

        /// <summary>
        /// Save settings asynchronously
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="name">settings name</param>
        /// <returns></returns>
        public static async Task SaveAsync(object settings, string name)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            string settingsPath = Path.Combine(_rootSettingsDir, name + _settingsFileExtension);
            using (FileStream fs = new FileStream(settingsPath, FileMode.OpenOrCreate))
            {
                byte[] buffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(settings));
                await fs.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        /// <summary>
        /// Read settings in json format asynchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">settings name</param>
        /// <returns></returns>
        public static async Task<T> ReadSettingsAsync<T>(string name) where T : BaseSettings
        {
            try
            {
                string settingsPath = await GetAboutAllSettings()
                    .Where(x => x.name == name)
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
        /// <param name="name">settings name</param>
        /// <returns></returns>
        public static async Task<bool> SettingsExists(string name) => await GetAboutAllSettings().AnyAsync(x => x.name == name);

        /// <summary>
        /// Get all settings
        /// </summary>
        /// <returns></returns>
        public static async IAsyncEnumerable<(string name, string path, SettingsType type, DateTime lastChangingTime)> GetAboutAllSettings()
        {
            foreach (FileInfo file in new DirectoryInfo("settings").GetFiles($"*.{_settingsFileExtension}", SearchOption.TopDirectoryOnly))
            {
                BaseSettings settings = await ReadSettingsAsync<BaseSettings>(Path.GetFileNameWithoutExtension(file.Name));
                if (settings?.Name == Path.GetFileNameWithoutExtension(file.Name))
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
                BaseSettings settings = await ReadSettingsAsync<BaseSettings>(Path.GetFileNameWithoutExtension(file.Name));
                if (settings?.Name == Path.GetFileNameWithoutExtension(file.Name) && 
                    settings?.Type == settingsType)
                {
                    yield return (settings.Name, file.FullName, settings.Type, file.LastWriteTime);
                }
            }
        }
    }
}
