using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Configuration;


namespace Services.Functions
{
    public static class Config
    {

        /// <summary>
        /// Verifica se a entidade passada por parâmetro é válida (existe no arquivo web.config)
        /// </summary>
        /// <param name="strEntity">Entidade</param>
        /// <returns>True se é válida, False se inválida</returns>
        public static bool isValidEntity(String strEntity)
        {
            bool bReturn = false;

            Configuration oConfiguration = WebConfigurationManager.OpenWebConfiguration("/");
            ConnectionStringSettingsCollection oConnectionStrings = oConfiguration.ConnectionStrings.ConnectionStrings;

            if (oConnectionStrings.Count > 0)
            {
                ConnectionStringSettings oConnectionString = oConnectionStrings[String.Concat(strEntity, "_entities")];

                if (oConnectionString != null)
                {
                    if (String.IsNullOrEmpty(oConnectionString.ConnectionString))
                    {
                        bReturn = false;
                    }
                    else
                    {
                        bReturn = true;
                    }
                }
                else
                {
                    bReturn = false;
                }
            }
            else
            {
                bReturn = false;
            }

            return bReturn;
        }


      

    }
}