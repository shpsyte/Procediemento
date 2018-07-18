using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Functions
{
    public class md5
    {
        /// <summary>
        /// Método para codificar uma String em MD5
        /// </summary>
        /// <param name="strTexto">String a ser codificada</param>
        /// <returns>String codificada em MD5</returns>
        public static string Encode(String strTexto)
        {
            var hash = System.Security.Cryptography.MD5.Create();
            var encoder = new System.Text.ASCIIEncoding();
            var combined = encoder.GetBytes(strTexto ?? "");
            return BitConverter.ToString(hash.ComputeHash(combined)).ToLower().Replace("-", "");
        }
    }
}
