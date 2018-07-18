using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    using Services.Functions;


namespace Services.Functions
{
    public static class Extend
    {
        public static string FormatToB2y(this string s)
        {
            
            if (string.IsNullOrEmpty(s))
            {
                return String.Empty;
            }
            
            string sourceInFormD = s.Trim().ToUpper().Normalize(NormalizationForm.FormD);

            var output = new StringBuilder();
            foreach (char c in sourceInFormD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                    output.Append(c);
            }

            return (output.ToString().Normalize(NormalizationForm.FormC));
        }
    }
}
