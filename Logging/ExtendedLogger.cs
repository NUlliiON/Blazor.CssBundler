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

        public ExtendedLogger()
        {
            Console.OutputEncoding = Encoding.UTF8;
            _emojiManager = new EmojiManager();
            _emojiManager.LoadEmoticons("emoticons.json"); // TODO: read path from settings
        }
        
        public void Print(string text)
        {
            text = _emojiManager.GetTextWithEmoticons(text);
            Console.WriteLine(text);
        }

        //public void PrintAsTable(st)

        public void PrintSuccess(string text)
        {
            text = _emojiManager.GetTextWithEmoticons(text);
            Console.WriteLine(_emojiManager.GetLoadedEmoji("checkmark") + " " + text);
        }

        public void PrintError(string text)
        {
            text = _emojiManager.GetTextWithEmoticons(text);
            Console.WriteLine(_emojiManager.GetLoadedEmoji("crossmark") + " " + text);
        }

        public void PrintWarn(string text)
        {
            text = _emojiManager.GetTextWithEmoticons(text);
            Console.WriteLine(_emojiManager.GetLoadedEmoji("warn") + " " + text);
        }
    }
}
