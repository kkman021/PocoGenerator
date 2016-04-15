using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoGenerator
{
    public class StringExtension
    {
        public string GetStr(string str)
        {
            var strArray = str.ToCharArray();
            var result = "";
            var nextUppler = true;
            for (int i = 0; i < str.Length; i++)
            {
                var item = strArray[i];

                if (item == '_')
                {
                    nextUppler = true;
                    continue;
                }

                if (nextUppler)
                    result += item.ToString().ToUpper();
                else
                    result += item.ToString().ToLower();

                nextUppler = false;
            }

            return result;
        }
    }
}
