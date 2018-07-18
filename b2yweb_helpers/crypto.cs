using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace b2yweb_helpers
{
    public class crypto
    {
        /* constante para criptografia */
        private const string strChave = "ACJ0027";

        /// <summary>
        /// Criptografa a senha passada por parâmetro
        /// </summary>
        /// <param name="strSenha">Senha</param>
        /// <returns>String Criptografada</returns>
        public static String Criptografa(String strSenha)
        {

            String strRetorno = String.Empty;

            Int32 intContador = 0;
            Int32 intTamanhoSenha = 0;
            Int32 intTamanhoChave = 0;

            Char chrC;
            Int32 intC = 0;

            Char[] chrChave = strChave.ToCharArray();
            Char[] chrSenha = strSenha.ToCharArray();

            intTamanhoSenha = chrSenha.Length - 1;
            intTamanhoChave = chrChave.Length - 1;

            for (intContador = 0; intContador <= intTamanhoSenha; intContador++)
            {

                if (intContador <= intTamanhoChave)
                {
                    chrC = chrChave[intContador];
                }
                else
                {
                    chrC = chrChave[(intContador - 1) % intTamanhoChave];
                }

                intC = ((Int32)chrSenha[intContador]) - 64 + (Int32)chrC - 64;

                intC += 64;

                if (intC >= 32)
                {
                    intC++;
                }

                strRetorno += ((char)intC).ToString();

            }

            strRetorno = Convert.ToBase64String(Encoding.ASCII.GetBytes(strRetorno.ToCharArray()), Base64FormattingOptions.None);

            return strRetorno;
        }

        /// <summary>
        /// Descriptografa a senha passada por parâmetro em base64
        /// </summary>
        /// <param name="strSenha">Senha</param>
        /// <returns>Senha Descriptografada</returns>
        public static String Descriptografa(String strSenha)
        {

            String strRetorno = String.Empty;

            Int32 intContador = 0;
            Int32 intTamanhoSenha = 0;
            Int32 intTamanhoChave = 0;

            Char chrC;
            Int32 intC = 0;

            Char[] chrChave = strChave.ToCharArray();
            Char[] chrSenha = (Encoding.ASCII.GetString(Convert.FromBase64String(strSenha))).ToCharArray();

            intTamanhoSenha = chrSenha.Length;
            intTamanhoChave = chrChave.Length;

            for (intContador = 0; intContador <= intTamanhoSenha - 1; intContador++)
            {
                if (intContador <= (intTamanhoChave - 1))
                {
                    chrC = chrChave[intContador];
                }
                else
                {
                    chrC = chrChave[(intContador - 1) % intTamanhoChave + 1];
                }

                intC = (((Int32)chrSenha[intContador]) - 64) - ((Int32)chrC - 64);

                intC += 64;

                if (intC > 32)
                {
                    intC--;
                }

                strRetorno += ((char)intC).ToString();
            }

            return strRetorno;

        }
    }
}
