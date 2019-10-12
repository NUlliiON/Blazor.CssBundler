using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.CssBundler
{
    public class NewtonjsonEtensions
    {
        public static bool ValidateJson(string str)
        {
            try
            {
                JToken.Parse(str);
                return true;
            }
            catch (JsonReaderException ex)
            {
                // TODO: return more information about exception
                return false;
            }
        }
    }
}
