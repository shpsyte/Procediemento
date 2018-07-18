using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IntlTexto.Intl
{

    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        public LocalizedDisplayNameAttribute(string resourceId)
            : base(GetMessageFromResource(resourceId))
        { }

        private static string GetMessageFromResource(string _nome)
        {
            String sRetorno = strings.ResourceManager.GetString(_nome.Trim());
            String sRetornoU = strings.ResourceManager.GetString(_nome.Trim().ToUpper());
            String sRetornoL = strings.ResourceManager.GetString(_nome.Trim().ToLower());
            
            if (!String.IsNullOrEmpty(sRetorno))
            {
                return sRetorno;
            }
            else 
            {
                if (!String.IsNullOrEmpty(sRetornoU))
                {
                    return sRetornoU;
                }
                else
                {
                    if (!String.IsNullOrEmpty(sRetornoL))
                    {
                        return sRetornoL;
                    }
                    else
                    {
                        return "### ERROR ###" + _nome ;
                    
                    }
                }
            }
        }
    }

    public static class LocalizeString
    {
        public static MvcHtmlString T(this HtmlHelper helper, string _nome)
        {
            String sRetorno = strings.ResourceManager.GetString(_nome.Trim());
            String sRetornoU = strings.ResourceManager.GetString(_nome.Trim().ToUpper());
            String sRetornoL = strings.ResourceManager.GetString(_nome.Trim().ToLower());

            if (!String.IsNullOrEmpty(sRetorno))
            {
                return MvcHtmlString.Create(String.Format("{0}", sRetorno));
            }
            else
            {
                if (!String.IsNullOrEmpty(sRetornoU))
                {
                    return MvcHtmlString.Create(String.Format("{0}", sRetornoU));
                }
                else
                {
                    if (!String.IsNullOrEmpty(sRetornoL))
                    {
                        return MvcHtmlString.Create(String.Format("{0}", sRetornoL));
                    }
                    else
                    {
                        return MvcHtmlString.Create(String.Format("{0}", "### ERROR ###" + _nome));

                    }
                }

            }
        }
    }
}
