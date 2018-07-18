using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Services.Componentes
{
    public static class HTMLHelpers
    {

      

        /// <summary>
        /// Componente Isco Sistemas para Botoes (Link's)
        /// </summary>
        /// <param name="_Action">Action a Ser Executada</param>
        /// <param name="_Controller">Nome do Controller a Ser Executado</param>
        /// <param name="_Contexto">Contexto, por exemplo Create, Edit, Details, se deixado em branco sera usado a _Action para resolver este nome.</param>
        /// <param name="_Tamanho">Grande;Pequeno;Mini Default: Normal</param>
        /// <param name="_TipoClasse">Representa o Tipo da Classe (Primario;Sucesso;Info;Alerta;Cuidado;Inverso,Link)</param>
        /// <param name="_Icone">Indica o Nome do Ícone a Ser Mostrado</param>
        /// <param name="_UsaIconeBranco">Indica se o icone será branco ou não</param>
        /// <param name="_Disable">Indica se está habilitado ou não.</param>
        /// <returns></returns>
        public static MvcHtmlString ActionLinkB2y(this HtmlHelper html,
                                               string _Action,
                                               string _Controller,
                                               string _Nome,
                                               string _TipoClasse = "",
                                               string _Tamanho = "",
                                               string _Icone = "",
                                               bool _UsaIconeBranco = true,
                                               bool _Disable = false)
        {
            
            string classe = "btn ";
            string icone = "";
            string _Habilitado = "";

            if (!string.IsNullOrEmpty(_TipoClasse))
            { classe += RetornaClasse(_TipoClasse); }

            if (!string.IsNullOrEmpty(_Tamanho))
            { classe += RetornaTamanho(_Tamanho); }

            if (!string.IsNullOrEmpty(_Icone))
            {
                if (_UsaIconeBranco)
                {
                    icone = string.Concat(" <i class='", _Icone, " icon-white'></i>");
                }else
                    icone = string.Concat(" <i class='", _Icone, " '></i>");
            }
            if (_Disable)
            { _Habilitado = " disabled"; }

            classe = string.Concat(classe, _Habilitado);
            return new MvcHtmlString(string.Format("<a class='{0}' href='../../{1}/{2}'>{3}{4}</a>", classe, _Controller, _Action, icone, _Nome));
        }

        private static string RetornaTamanho(string _Tamanho)
        {
            string Class = "";

            /// <param name="_Tamanho">Grande;Pequeno;Mini Default: Normal</param>
            switch (_Tamanho)
                {
                    case "Grande":
                        Class += string.Concat(Class, " btn-large");
                        break;
                    case "Pequeno":
                        Class += string.Concat(Class, " btn-small");
                        break;
                    case "Mini":
                        Class += string.Concat(Class, " btn-mini");
                        break;
                    case "Normal":
                        Class += string.Concat(Class, "");
                        break;
                    default:
                        Class += string.Concat(Class, "");
                        break;
            }
            return Class;

        }

        private static string RetornaClasse(string _TipoClasse)
        {
            string Class = "";

            if (string.IsNullOrEmpty(_TipoClasse))
            {
                Class = "btn-link";
            }
            else
            {
                switch (_TipoClasse)
                {
                    case "Primario":
                            Class += string.Concat(Class, " btn-primary ");
                            break;
                    case "Sucesso":
                            Class += string.Concat(Class, " btn-success ");
                            break;
                    case "Info":
                            Class += string.Concat(Class, " btn-info ");
                            break;
                    case "Alerta":
                            Class += string.Concat(Class, " btn-warning ");
                            break;
                    case "Cuidado":
                            Class += string.Concat(Class, " btn-danger ");
                            break;
                    case "Inverso":
                            Class += string.Concat(Class, " btn-inverse ");
                            break;
                    case "link":
                            string.Concat(Class, " btn-link ");
                            break;
                    default:
                            Class += string.Concat(Class, "");
                            break;
                }



            }
            return Class;

        }
    }
}
