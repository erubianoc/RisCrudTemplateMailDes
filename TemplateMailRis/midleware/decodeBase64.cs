using System;
using System.Collections.Generic;
using System.Text;

namespace TemplateMailRis.midleware
{
    public class decodeBase64
    {
        public static string Base64Decode(string str)
        {
            return Encoding.Default.GetString(Convert.FromBase64String(str));
        }
    }
}
