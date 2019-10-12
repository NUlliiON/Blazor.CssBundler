using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Blazor.CssBundler.Emoji
{
    class EmojiLoader
    {
        /// <summary>
        /// Load emoticons from json file
        /// </summary>
        /// <param name="filePath">json file path</param>
        public Dictionary<string, string> Load(string filePath)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filePath));
        }
    }
}
