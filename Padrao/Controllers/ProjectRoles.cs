using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


using Domain.Entity;
using Data.Context;

namespace Services.Functions
{
    public static class ProjectRoles
    {
        //now constants for the attribute values
        public const string Admin = "Admin";
        public const string Support = "Support";
        public const string User = "User";
        public const string Guest = "Guest";
        public const string List = "List";
        public const string Details = "Details";
        public const string Edit = "Edit";
        public const string Create = "Create";
        public const string Delete = "Delete";
        public const string Index = "Index";
        public const string find = "find";
        public const string ExportXls = "ExportXls";
        public const string Upload = "Upload";
        public const string Grafico = "Grafico";
        public const string Save = "Save";
        public const string Remove = "Remove";


    }
    
    public class CustomAuthorize : AuthorizeAttribute
    {
        private b2yweb_entities db = null;
        readonly Funcoes _Funcoes = new Funcoes();
        //
        // GET: /Procedimento/Departamento/
        /// <summary>
        /// Função Para Verificar se o usuário é autenticado
        /// </summary>
        /// <param name="requestContext"></param>

        //Property to allow array instead of single string.
        private string[] _authorizedRoles;
        public string[] AuthorizedRoles
        {
            get { return _authorizedRoles ?? new string[0]; }
            set { _authorizedRoles = value; }
        }

        public string AccessLevel { get; set; }
        public string Roles { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            //If its an unauthorized/timed out ajax request go to top window and redirect to logon.
            //            if (filterContext.Result is HttpUnauthorizedResult && filterContext.HttpContext.Request.IsAjaxRequest())
            //               filterContext.Result = new JavaScriptResult() 
            //             { Script = top.location = "/Account/LogOn?Expired=1" };

            //If authorization results in HttpUnauthorizedResult, redirect to error page instead of Logon page.
            if (filterContext.Result is HttpUnauthorizedResult)
                filterContext.Result = new RedirectResult("~/Protected/Authorization"); // ("~/Error/Authorization");
        }

        public string GetGrupoForUser(HttpContextBase httpContext)
        {
            db = new b2yweb_entities(httpContext.Session["oEmpresa"].ToString());
            //int cd_usuario = ((Usuario)httpContext.Session["oUsuario"]).CD_USUARIO;
            int cd_grupo = ((Usuario)httpContext.Session["oUsuario"]).CD_GUSUARIO;
            string Nome = (from b in db.GUsuario.Where(a => a.CD_GUSUARIO == cd_grupo) select b.NOME).FirstOrDefault();
            return Nome;
        }

        public bool Acessa(string cd_empresa, int cd_grupo, string modulo, string action)
        {
            db = new b2yweb_entities(cd_empresa);
            int qtde = (db.Permissoes.Where(a => a.CD_GUSUARIO == cd_grupo && a.MODULO.ToUpper() == modulo.ToUpper()).Count());
            bool retorno = false;


          



            if (qtde == 0)
            {
                string sqlquery = string.Format(" INSERT INTO Permissoes VALUES(ID_PERMISSAO_SEQ.nextval, {0}, \'{1}\', \'N',\'N',\'N\',\'N\',\'N\' ) ",
                 cd_grupo, modulo.ToUpper());
                
                db.Database.ExecuteSqlCommand(sqlquery);
            }


            if (cd_grupo == 1)
            {
                return true;
            }


            if ((action.ToUpper() != "INDEX") && (action.ToUpper() != "DETAILS") && (action.ToUpper() != "DELETE")
               && (action.ToUpper() != "CREATE") && (action.ToUpper() != "EDIT"))
            {
                if (modulo.ToUpper() == "RELATORIOS")
                {
                    retorno = (from a in db.Permissoes.Where(
                                                            a => a.CD_GUSUARIO == cd_grupo &&
                                                            a.MODULO.ToUpper() == modulo.ToUpper() &&
                                                            a.ACESSA == "S")
                               select a.ACESSA).FirstOrDefault() == "S";
                }
                else
                {
                    retorno = true;
                }
            }
            else
                if (modulo.ToUpper() == "RELATORIOS")
                {
                    retorno = (from a in db.Permissoes.Where(
                                                            a => a.CD_GUSUARIO == cd_grupo &&
                                                            a.MODULO.ToUpper() == modulo.ToUpper() &&
                                                            a.ACESSA == "S")
                               select a.ACESSA).FirstOrDefault() == "S";
                }
                else
                    {




                        if (action.ToUpper() == "INDEX")
                        {
                            retorno = (from a in db.Permissoes.Where(
                                                  a => a.CD_GUSUARIO == cd_grupo &&
                                                       a.MODULO.ToUpper() == modulo.ToUpper() &&
                                                       a.ACESSA == "S")
                                       select a.ACESSA).FirstOrDefault() == "S";

                        }

                        if (action.ToUpper() == "DETAILS")
                        {
                            retorno = (from a in db.Permissoes.Where(
                                                  a => a.CD_GUSUARIO == cd_grupo &&
                                                       a.MODULO.ToUpper() == modulo.ToUpper() &&
                                                       a.DETALHA == "S")
                                       select a.DETALHA).FirstOrDefault() == "S";

                        }

                        if (action.ToUpper() == "EDIT")
                        {
                            retorno = (from a in db.Permissoes.Where(
                                                  a => a.CD_GUSUARIO == cd_grupo &&
                                                       a.MODULO.ToUpper() == modulo.ToUpper() &&
                                                       a.EDITA == "S")
                                       select a.EDITA).FirstOrDefault() == "S";

                        }

                        if (action.ToUpper() == "CREATE")
                        {
                            retorno = (from a in db.Permissoes.Where(
                                                  a => a.CD_GUSUARIO == cd_grupo &&
                                                       a.MODULO.ToUpper() == modulo.ToUpper() &&
                                                       a.CRIA == "S")
                                       select a.CRIA).FirstOrDefault() == "S";

                        }

                        if (action.ToUpper() == "DELETA")
                        {
                            retorno = (from a in db.Permissoes.Where(
                                                  a => a.CD_GUSUARIO == cd_grupo &&
                                                       a.MODULO.ToUpper() == modulo.ToUpper() &&
                                                       a.DELETA == "S")
                                       select a.DELETA).FirstOrDefault() == "S";

                        }
                    }


            return retorno;
        }

        public string[] GetRolesForUser(HttpContextBase httpContext)
        {
            db = new b2yweb_entities(httpContext.Session["oEmpresa"].ToString());

            //int cd_usuario = ((Usuario)httpContext.Session["oUsuario"]).CD_USUARIO;
            int cd_grupo = ((Usuario)httpContext.Session["oUsuario"]).CD_GUSUARIO;

            List<String> grupo = db.Permissoes.Where(a => a.CD_GUSUARIO == cd_grupo).Select(p => p.MODULO).ToList();
            return grupo.ToArray();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var routeData = httpContext.Request.RequestContext.RouteData;
            var controller = routeData.GetRequiredString("controller");
            var action = routeData.GetRequiredString("action");

            //string[] GrupoAcesso = this.AccessLevel.Split(';');
            //string TipoAcesso = this.Roles;
            
            
            
            //string Roles = this.Roles;
            //string[] RolesDaView = Roles.Split(';').FirstOrDefault();
            //string[] GrupoAdmin = new string[] {"ADMIN", "ADMINISTRADORES"};


          //  string Grupo = GetGrupoForUser(httpContext);

            
            // Verifica se o usuario possui esta em alguma role especifica se sim nao processa o resto
           // if (GrupoAcesso.Contains(Grupo))
           // {
           //     return true;
          //  }
            if (httpContext.Session["oEmpresa"] == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(httpContext.Session["oEmpresa"].ToString()))
            {
                return false;
            }


            db = new b2yweb_entities(httpContext.Session["oEmpresa"].ToString());
            int cd_grupo = ((Usuario)httpContext.Session["oUsuario"]).CD_GUSUARIO;
          





            if (Acessa(httpContext.Session["oEmpresa"].ToString(), cd_grupo, controller, action))
            {
                return true;
            } else
            {
                return false;
            }
        


            //string[] RolesUser = GetRolesForUser(httpContext);

            
            
            
            

            //return true;

            var isAuthorized = base.AuthorizeCore(httpContext);
            
            
            /*if (!isAuthorized)
            {
                return false;
            }*/

            //string privilegeLevels = string.Join("", GetUserRights(httpContext.User.Identity.Name.ToString())); // Call another method to get rights of the user from DB
            string privilegeLevels = string.Join("", "Index", "Admin");

            if (privilegeLevels.Contains(this.AccessLevel))
            {
                return true;
            }
            else
            {
                return false;
            }           
        }

    }
}
