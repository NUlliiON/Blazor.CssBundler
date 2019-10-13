using Blazor.CssBundler.Emoji;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Logging
{
    /// <summary>
    /// This logger support Emoticons (Emoji)
    /// </summary>
    class ExtendedLogger : ILogger
    {
        private EmojiManager _emojiManager;
        private readonly string _emoticonsPath = "emoticons.json";

        public ExtendedLogger()
        {
            Console.OutputEncoding = Encoding.UTF8;
            _emojiManager = new EmojiManager();
            _emojiManager.LoadEmoticons(_emoticonsPath);
        }
        
        public void Print(string text)
        {
            text = _emojiManager.GetTextWithEmoticons(text);
            Console.WriteLine(text);
        }

        public void PrintSuccess(string text)
        {
            text = _emojiManager.GetTextWithEmoticons(text);
            Console.WriteLine(_emojiManager.GetEmoji("checkmark") + " " + text);
        }

        public void PrintError(string text)
        {
            text = _emojiManager.GetTextWithEmoticons(text);
            Console.WriteLine(_emojiManager.GetEmoji("crossmark") + " " + text);
        }

        public void PrintWarn(string text)
        {
            text = _emojiManager.GetTextWithEmoticons(text);
            Console.WriteLine(_emojiManager.GetEmoji("warn") + " " + text);
        }
    }
}
