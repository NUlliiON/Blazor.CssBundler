using Blazor.CssBundler.Models.Settings;
using JsonSubTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Settings
{
    public class SettingsManager
    {
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

            byte[] jsonBytes = Encoding.Default.GetBytes(JsonConvert.SerializeObject(settings));
            using (FileStream fs = new FileStream(MakeSettingsFileName(name), FileMode.OpenOrCreate))
            {
                await fs.WriteAsync(jsonBytes, 0, jsonBytes.Length);
            }
        }

        /// <summary>
        /// Read settings from file asynchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">full path to settings</param>
        /// <returns></returns>
        public static async Task<T> ReadAsync<T>(string filePath) where T : BaseSettings
        {
            try
            {
                byte[] jsonBytes;
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    jsonBytes = new byte[fs.Length];
                    await fs.ReadAsync(jsonBytes, 0, jsonBytes.Length);

                }
                string json = Encoding.Default.GetString(jsonBytes);
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
        /// Checking settings with specified name
        /// </summary>
        /// <param name="name">settings name</param>
        /// <returns></returns>
        public static bool Has(string name) => File.Exists(Path.Combine("settings", MakeSettingsFileName(name)));

        /// <summary>
        /// Get all settings
        /// </summary>
        /// <returns></returns>
        public static async IAsyncEnumerable<(string name, SettingsType type, DateTime lastChangingTime)> GetAboutAllSettings()
        {
            foreach (FileInfo file in new DirectoryInfo("settings").GetFiles("*.settings.json", SearchOption.TopDirectoryOnly))
            {
                BaseSettings settings = await ReadAsync<BaseSettings>(file.FullName);
                if (settings?.Name != null)
                {
                    yield return (settings.Name, settings.Type, file.LastWriteTime);
                }
            }
        }

        /// <summary>
        /// Get specified settings
        /// </summary>
        /// <param name="settingsTypeEnum">settings type</param>
        ///// <returns></returns>
        public static async IAsyncEnumerable<(string name, SettingsType type, DateTime lastChaningTime)> GetAboutSettings(SettingsType settingsTypeEnum)
        {
            foreach (FileInfo file in new DirectoryInfo("settings").GetFiles($"*.settings.json", SearchOption.TopDirectoryOnly))
            {
                BaseSettings settings = await ReadAsync<BaseSettings>(file.FullName);
                if (settings?.Type == settingsTypeEnum && 
                    settings?.Name != null
                    )
                {
                    yield return (settings.Name, settings.Type, file.LastWriteTime);
                }
            }
        }

        /// <summary>
        /// Make settings path
        /// </summary>
        /// <param name="name">settings name</param>
        /// <param name="type">settings type</param>
        /// <returns></returns>
        private static string MakeSettingsFileName(string name)
        {
            return $"{name}.settings.json";
        }
    }
}
