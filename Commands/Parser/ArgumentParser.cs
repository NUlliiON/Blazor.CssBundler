using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Blazor.CssBundler.Commands.Parser
{
    public class ArgumentParser
    {
        public IEnumerable<Argument> Parse(string text)
        {
            Regex nameArgReg = new Regex("-([A-Za-z][A-Za-z0-9-_]*)");
            foreach (Match match in nameArgReg.Matches(text))
            {
                yield return new Argument(name: match.Groups[1].Value, value: null);
            }

            Regex nameValueArgReg = new Regex("--([A-Za-z][A-Za-z0-9-_]*)\\s+\"(.*?)\"");
            foreach (Match match in nameValueArgReg.Matches(text))
            {
                GroupCollection groups = match.Groups;
                yield return new Argument(name: groups[1].Value, value: groups[2].Value);
            }
        }
    }
}
