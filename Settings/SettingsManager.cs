using Blazor.CssBundler.Models.Settings;
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
        private static readonly string _settingsPath = Path.Combine(Environment.CurrentDirectory, "settings");

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

            byte[] buffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(settings));
            using (FileStream fs = new FileStream(MakeSettingsPath(name), FileMode.OpenOrCreate))
            {
                await fs.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        /// <summary>
        /// Read json file asynchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">file name without extension</param>
        /// <returns></returns>
        private static async Task<T> ReadJsonFileAsync<T>(string fileName) where T : BaseSettings
        {
            try
            {
                byte[] buffer;
                using (FileStream fs = new FileStream(MakeSettingsPath(fileName), FileMode.OpenOrCreate))
                {
                    buffer = new byte[fs.Length];
                    await fs.ReadAsync(buffer, 0, buffer.Length);
                }

                string json = Encoding.Default.GetString(buffer);
                if (NewtonjsonEtensions.ValidateJson(json))
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch
            {

            }
            return default(T);
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

                return await ReadJsonFileAsync<T>(name);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Checking settings with specified name
        /// </summary>
        /// <param name="name">settings name</param>
        /// <returns></returns>
        public static bool Has(string name) => File.Exists(MakeSettingsPath(name));

        /// <summary>
        /// Get all settings
        /// </summary>
        /// <returns></returns>
        public static async IAsyncEnumerable<(string name, string path, SettingsType type, DateTime lastChangingTime)> GetAboutAllSettings()
        {
            foreach (FileInfo file in new DirectoryInfo("settings").GetFiles("*.json", SearchOption.TopDirectoryOnly))
            {
                BaseSettings settings = await ReadSettingsAsync<BaseSettings>(Path.GetFileNameWithoutExtension(file.Name));
                if (settings?.Name != null)
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
            foreach (FileInfo file in new DirectoryInfo("settings").GetFiles($"*.json", SearchOption.TopDirectoryOnly))
            {
                BaseSettings settings = await ReadSettingsAsync<BaseSettings>(Path.GetFileNameWithoutExtension(file.Name));
                if (settings?.Name != null && settings?.Type == settingsType)
                {
                    yield return (settings.Name, file.FullName, settings.Type, file.LastWriteTime);
                }
            }
        }

        /// <summary>
        /// Make settings path
        /// </summary>
        /// <param name="name">settings name</param>
        /// <returns></returns>
        private static string MakeSettingsPath(string name)
        {
            return Path.Combine(_settingsPath, $"{name}.json");
        }
    }
}
