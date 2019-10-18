using JsonSubTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Blazor.CssBundler.Models.Settings
{
    public enum SettingsType
    {
        Unknown = 0,
        Application = 1,
        Component = 2
    }

    [JsonConverter(typeof(JsonSubtypes), "Type")]
    [JsonSubtypes.KnownSubType(typeof(BaseSettings), SettingsType.Unknown)]
    [JsonSubtypes.KnownSubType(typeof(ApplicationSettings), SettingsType.Application)]
    [JsonSubtypes.KnownSubType(typeof(ComponentSettings), SettingsType.Component)]
    public class BaseSettings
    {
        [SettingsProperty]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("type")]
        public virtual SettingsType Type { get; set; }

        [SettingsProperty]
        [JsonProperty("projectName")]
        public string ProjectName { get; set; }

        [SettingsProperty]
        [JsonProperty("projectDirectory")]
        public string ProjectDirectory { get; set; }

        [SettingsProperty]
        [JsonProperty("projectFilePath")]
        public string ProjectFilePath { get; set; }

        [SettingsProperty]
        [JsonProperty("cssRazorSearchPattern")]
        public string CssRazorSearchPattern { get; set; }
    }
}
