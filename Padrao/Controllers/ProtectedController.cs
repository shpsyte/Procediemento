using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Context;
using Domain.Entity;
using Services.Functions;


namespace b2yweb_mvc4.Controllers
{

    public class ProtectedController : Controller
    {

        private b2yweb_entities db = null;
        readonly Funcoes _Funcoes = new Funcoes();



        [AuthFilter]
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.Session["oEmpresa"] != null)
            {
                db = new b2yweb_entities(requestContext.HttpContext.Session["oEmpresa"].ToString());
            }
        }

        //
        [HttpGet]
        public ActionResult Authorization()
        {
            return View();
        }


        //
        // GET: /Login/
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //
        // POST: /Login
        [HttpPost]
        public ActionResult Login(Usuario usuario, string StrEmpresa = "")
        {
            string strUsuario = usuario.NOME.Trim().ToUpper();
            string strSenha = crypto.Criptografa(usuario.SENHA.Trim().ToUpper());
            string strEmpresa = StrEmpresa.Trim().ToLower();
            strEmpresa = "oracle";
           // string strEmail = usuario.EMAIL.Trim();
            

            Usuario oUsuario = new Usuario();
            UsuarioRegional oRegional = new UsuarioRegional();


            if (String.IsNullOrEmpty(strUsuario))
            {
                ModelState.AddModelError("", "Atenção! Preencha o campo [Usuário].");
                return View();
            }

            if (String.IsNullOrEmpty(strSenha))
            {
                ModelState.AddModelError("", "Atenção! Preencha o campo [Senha].");
                return View();
            }

            if (String.IsNullOrEmpty(strEmpresa))
            {
                ModelState.AddModelError("", "Atenção! Preencha o campo [Empresa].");
                return View();
            }
            else
            {
                // verifica se a empresa existe nas strings de conexão
                if (!Config.isValidEntity(strEmpresa))
                {
                    ModelState.AddModelError("", "Atenção! A empresa [" + strEmpresa + "] não existe.");
                    return View();
                }
            } 

            // instancia a entidade com a conexão do cliente
            db = new b2yweb_entities(strEmpresa);



            oUsuario = (Usuario)db.Usuario.Where(s => s.LOGIN.ToUpper().Equals(strUsuario) && s.SENHA.Equals(strSenha)).FirstOrDefault();
            if (oUsuario == null)
            {
                oUsuario = (Usuario)db.Usuario.Where(s => s.EMAIL.ToLower().Equals(strUsuario.ToLower()) && s.SENHA.Equals(strSenha)).FirstOrDefault();
            }

            if (oUsuario != null)
            {
                if (oUsuario.SITUACAO != null)
                {

                  

                    if (oUsuario.SITUACAO.Equals("X"))
                    {
                        Session["oUsuario"] = null;
                        Session["oEmpresa"] = null;
                        Session["oRegional"] = null;
                        Session["cd_grupo"] = null;
                        Session["cd_pagina"] = null;



                        Session.Abandon();

                        ModelState.AddModelError("", "Atenção! O usuário [" + strUsuario + "] não tem permissão para acesso a este sistema.");

                        return View();
                    }
                    else
                    {

                        var ListaUnidadePorUsuario = (from a in db.UsuarioRegional where a.CD_USUARIO == oUsuario.CD_USUARIO select a.CD_REGIONAL).ToList();
                        
                        
                        Session["oUsuario"] = oUsuario;
                        Session["usuario"] = oUsuario.NOME;
                        Session["cd_usuario"] = oUsuario.CD_USUARIO;
                        Session["oEmpresa"] = strEmpresa;
                        Session["oRegional"] = ListaUnidadePorUsuario.ToList();
                        Session["cd_grupo"] = oUsuario.CD_GUSUARIO;
                        var cd_pagina = (from a in db.GUsuario.Where(a => a.CD_GUSUARIO == oUsuario.CD_GUSUARIO) select a.CD_PAGINA).FirstOrDefault();
                        Session["cd_pagina"] = cd_pagina == null ? 0 : cd_pagina;
                        System.Web.Security.FormsAuthentication.SetAuthCookie(oUsuario.NOME, false);

                        Session.Timeout = 5600; 

                        if (oUsuario.ALT_SENHA.Equals("S"))
                        {
                            return RedirectToAction("ChangePassword", "Protected", new  {id = oUsuario.CD_USUARIO });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
            


                        
                    }



                }
                else
                {
                    Session["oUsuario"] = null;
                    Session["usuario"] = null;
                    Session["oEmpresa"] = null;
                    Session["oRegional"] = null;
                    Session["cd_grupo"] = null;
                    Session["cd_pagina"] = null;
                    Session["cd_usuario"] = null;

                    Session.Abandon();

                    ModelState.AddModelError("", "Atenção! O usuário [" + strUsuario + "] não tem permissão para acesso a este sistema.");

                    return View();
                }
            }
            else
            {
                Session["oUsuario"] = null;
                Session["oEmpresa"] = null;
                Session["oRegional"] = null;
                Session["cd_grupo"] = null;
                Session["cd_pagina"] = null;
                Session["usuario"] = null;
                Session["cd_usuario"] = null;


                Session.Abandon();

                ModelState.AddModelError("", "Dados de login incorretos!");

                return View();
            }
        }


        // GET: /Logout
        [HttpGet]
        public ActionResult ChangePassword(short id = 0)
        {
        
            int cd_usuario = ((Usuario)Session["oUsuario"]).CD_USUARIO;

            if (id != cd_usuario)
            {

                return RedirectToAction("Logout","Protected");
            }

            Usuario usuario = db.Usuario.Find(id);


            if (usuario == null)
            {
                return HttpNotFound();
            }
            
            return View(usuario);
        }


        [HttpPost]
        public ActionResult ChangePassword(Usuario usuario, string CNFSENHA)
        {
            if (string.IsNullOrEmpty(CNFSENHA))
            {
                ModelState.AddModelError("NOME", "É necessário confirmar a senha");
            }

            if (!usuario.SENHA.Equals(CNFSENHA))
            {
                ModelState.AddModelError("NOME", "As Senhas não são idêncticas");
            }


            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                usuario.SENHA = crypto.Criptografa(usuario.SENHA.Trim().ToUpper());
                usuario.ALT_SENHA = "N";
                db.SaveChanges();
                return RedirectToAction("Index","home");
            }
            

            return View();
        }

        //
        // GET: /Logout
        [HttpGet]
        public ActionResult Logout()
        {
            Session["oUsuario"] = null;
            Session["oRegional"] = null;
            Session["oEmpresa"] = null;
            Session["cd_grupo"] = null;
            Session["cd_pagina"] = null;
            Session["usuario"] = null;
            Session["usuario"] = null;
            Session["cd_usuario"] = null;


            Session.Abandon();
            System.Web.Security.FormsAuthentication.SignOut();
            foreach (var cookie in Request.Cookies.AllKeys)
            {
                Request.Cookies.Remove(cookie);
            }
            foreach (var cookie in Response.Cookies.AllKeys)
            {
                Response.Cookies.Remove(cookie);
            }
            return RedirectToAction("Login", "Protected");
        }


        // GET: /CriarUsuario
        [HttpGet]
        public ActionResult CriarUsuario()
        {
            return View();
        }

        /* requisição por ajax / json */
        //
        // POST: /CriarUsuario
        [HttpPost]
        public JsonResult CriarUsuario(String strUsuario, String strNome, String strSenha, String strConfirmarSenha, String strEmail, String strConfirmarEmail)
        {

            Usuario oUsuario = new Usuario();

            strUsuario = strUsuario.Trim().ToUpper();
            strNome = strNome.Trim().ToUpper();
            strSenha = crypto.Criptografa(strSenha.Trim().ToUpper());
            strConfirmarSenha = crypto.Criptografa(strConfirmarSenha.Trim().ToUpper());
            strEmail = strEmail.Trim().ToUpper();
            strConfirmarEmail = strConfirmarEmail.Trim().ToUpper();

            if (String.IsNullOrEmpty(strUsuario))
            {
                return Json(new Retorno('S', 'N', "Atenção! Preencha o campo [Usuário].", "Protected", "CriarUsuario", "CriarUsuario"));
            }
            else
            {
                oUsuario = (Usuario)db.Usuario.Where(s => (s.LOGIN.Equals(strUsuario))).FirstOrDefault();

                if (oUsuario != null)
                {
                    if (oUsuario.LOGIN.Trim().ToUpper().Equals(strUsuario))
                    {
                        return Json(new Retorno('S', 'N', "Atenção! O usuário [" + strUsuario + "] já existe na base de dados.", "Protected", "CriarUsuario", "CriarUsuario"));
                    }
                }
            }

            if (String.IsNullOrEmpty(strNome))
            {
                return Json(new Retorno('S', 'N', "Atenção! Preencha o campo [Nome].", "Protected", "CriarUsuario", "CriarUsuario"));
            }

            if (String.IsNullOrEmpty(strSenha))
            {
                return Json(new Retorno('S', 'N', "Atenção! Preencha o campo [Senha].", "Protected", "CriarUsuario", "CriarUsuario"));
            }

            if (String.IsNullOrEmpty(strConfirmarSenha))
            {
                return Json(new Retorno('S', 'N', "Atenção! Preencha o campo [Confirmar senha].", "Protected", "CriarUsuario", "CriarUsuario"));
            }

            if (!strSenha.Equals(strConfirmarSenha))
            {
                return Json(new Retorno('S', 'N', "Atenção! Os campos [Senha] e [Confirmar Senha] não podem ser diferentes.", "Protected", "CriarUsuario", "CriarUsuario"));
            }

            if (String.IsNullOrEmpty(strEmail))
            {
                return Json(new Retorno('S', 'N', "Atenção! Preencha o campo [E-Mail].", "Protected", "CriarUsuario", "CriarUsuario"));
            }
            else
            {
                oUsuario = (Usuario)db.Usuario.Where(s => (s.EMAIL.Equals(strEmail))).FirstOrDefault();

                if (oUsuario != null)
                {
                    if (oUsuario.EMAIL.Trim().ToUpper().Equals(strEmail))
                    {
                        return Json(new Retorno('S', 'N', "Atenção! O E-Mail [" + strEmail + "] já existe na base de dados.", "Protected", "CriarUsuario", "CriarUsuario"));
                    }
                }
            }

            if (String.IsNullOrEmpty(strConfirmarEmail))
            {
                return Json(new Retorno('S', 'N', "Atenção! Preencha o campo [Confirmar E-Mail].", "Protected", "CriarUsuario", "CriarUsuario"));
            }

            if (!strEmail.Equals(strConfirmarEmail))
            {
                return Json(new Retorno('S', 'N', "Atenção! Os campos [E-Mail] e [Confirmar E-Mail] não podem ser diferentes.", "Protected", "CriarUsuario", "CriarUsuario"));
            }

            Int32? intCdUsuario = db.Usuario.Max(s => (Int32?)s.CD_USUARIO);

            if (intCdUsuario != null)
            {
                intCdUsuario++;
            }
            else
            {
                intCdUsuario = 1;
            }

            oUsuario = new Usuario();

            oUsuario.CD_USUARIO = (short)intCdUsuario;
            oUsuario.LOGIN = strUsuario;
            oUsuario.NOME = strNome;
            oUsuario.SENHA = strSenha;
            oUsuario.EMAIL = strEmail;
            oUsuario.CD_GUSUARIO = 1;
            oUsuario.SITUACAO = "A";
            oUsuario.REPROVA = "N";
            oUsuario.APROVA = "N";
            oUsuario.ALT_SENHA = "S";


            db.Usuario.Add(oUsuario);
            db.SaveChanges();

            return Json(new Retorno('N', 'S', "O usuário [" + strUsuario + "] foi criado com sucesso!", "Protected", "CriarUsuario", "CriarUsuario"));

        }

    }
}
