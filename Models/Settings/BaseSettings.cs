using JsonSubTypes;
using Newtonsoft.Json;

namespace Blazor.CssBundler.Models.Settings
{
    public enum SettingsType
    {
        Application = 1,
        Component = 2
    }

    [JsonConverter(typeof(JsonSubtypes), "Type")]
    [JsonSubtypes.KnownSubType(typeof(ApplicationSettings), SettingsType.Application)]
    [JsonSubtypes.KnownSubType(typeof(ComponentSettings), SettingsType.Component)]
    public class BaseSettings
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        public virtual SettingsType Type { get; set; }

        [JsonProperty("projectName")]
        public string ProjectName { get; set; }

        [JsonProperty("projectDirectory")]
        public string ProjectDirectory { get; set; }

        [JsonProperty("projectFilePath")]
        public string ProjectFilePath { get; set; }

        [JsonProperty("cssRazorSearchPattern")]
        public string CssRazorSearchPattern { get; set; }
    }
}
