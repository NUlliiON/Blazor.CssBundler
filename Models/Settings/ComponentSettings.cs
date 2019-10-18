using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Blazor.CssBundler.Models.Settings
{
    public class ComponentSettings : BaseSettings
    {
        public override SettingsType Type { get; set; } = SettingsType.Component;

        [Required]
        [SettingsProperty]
        [JsonProperty("assemblyPath")]
        public string AssemblyPath { get; set; }
    }
}
