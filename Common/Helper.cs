using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ValueBlue.Web.Common
{
    public class Helper
    {

        public async static Task<string> CleanString(string strSource, string strStart, string strEnd)
        {
            try
            {
                strSource = await Task.FromResult(HttpUtility.HtmlDecode(strSource));
                int Start, End;
                if (strSource.Contains(strStart) && strSource.Contains(strEnd))
                {
                    Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                    End = strSource.IndexOf(strEnd, Start);
                    return (strSource.Substring(Start, End - Start));
                }
                else
                {
                    return (string.Empty);
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
