using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Blazor.CssBundler.Models.Settings
{
    public class ApplicationSettings : BaseSettings
    {
        public override SettingsType Type { get; set; }

        [Required]
        [JsonProperty("globalStylesPath")]
        public string GlobalStylesPath { get; set; }

        [Required]
        [JsonProperty("outputCssDirectory")]
        public string OutputCssDirectory { get; set; }
    }
}
