using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Blazor.CssBundler.Emoji
{
    class EmojiManager
    {
        private const string _openBracket = "</";
        private const string _closeBracket = "/>";

        private EmojiLoader _emojiLoader;
        private Dictionary<string, string> _emoticonsDict;
        private Regex _regex;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmojiManager()
        {
            // TODO: move some functions to EmojiParser
            _emojiLoader = new EmojiLoader();
            _regex = new Regex(@"(</)([A-Za-z0-9_]+)(/>)");
        }

        /// <summary>
        /// Load emoticons from json file
        /// </summary>
        /// <param name="filePath">json file path</param>
        public void LoadEmoticons(string filePath)
        {
            _emoticonsDict = _emojiLoader.Load(filePath);
        }

        /// <summary>
        /// Get emoticons from text
        /// </summary>
        /// <param name="text"></param>
        /// <returns>emoticons</returns>
        private List<string> ParseEmoticons(string text)
        {
            List<string> uniqueEmoticonsList = new List<string>();

            MatchCollection emoticonsMatches = _regex.Matches(text);
            foreach (Match emoji in emoticonsMatches)
            {
                string emojiName = emoji.Groups[2].Value;
                if (_emoticonsDict.ContainsKey(emojiName))
                {
                    if (!uniqueEmoticonsList.Exists(x => x == emojiName))
                    {
                        uniqueEmoticonsList.Add(emojiName);
                    }
                }
            }

            return uniqueEmoticonsList;
        }

        /// <summary>
        /// Get emoji
        /// </summary>
        /// <param name="name">emoji name</param>
        /// <returns></returns>
        public string GetEmoji(string name)
        {
            if (_emoticonsDict.ContainsKey(name))
                return _emoticonsDict[name];
            return null;
        }

        /// <summary>
        /// Replace emoticons keys to emoticons chars
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string ReplaceToEmoticons(string text, IList<string> emoticons)
        {
            foreach (var emojiName in emoticons)
            {
                text = text.Replace(_openBracket + emojiName + _closeBracket, _emoticonsDict[emojiName]);
            }
            return text;
        }

        /// <summary>
        /// Returning text which emoticons keys replaced to emoticons chars
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string GetTextWithEmoticons(string text)
        {
            List<string> emoticons = ParseEmoticons(text);
            return ReplaceToEmoticons(text, emoticons);
        }
    }
}
