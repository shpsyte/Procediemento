using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Context;
using Domain.Entity;
using Services.Functions;

namespace b2yweb_mvc4.Controllers
{
    [AuthFilter]
    public class UsuarioController : Controller
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
        // GET: /Usuario/
         [CustomAuthorize(AccessLevel = "gusuarioIndex")]
        public ActionResult Index()
        {
            var usuario = db.Usuario; //.Include(u => u.GUsuario);
            return View(usuario.ToList());
        }


        // GET: /Usuario/

        [HttpPost]
        [CustomAuthorize(AccessLevel = "gusuarioIndex")]
        public ActionResult Index(string strPesquisa)
        {
            var usuario = db.Usuario; //.Include(u => u.GUsuario);
            return View(usuario.ToList().Where(a => a.NOME.ToUpper().Contains(strPesquisa.ToUpper())));
        }

        //
        // GET: /Usuario/Details/5
        [CustomAuthorize(AccessLevel = "gusuarioIndex")]
        public ActionResult Details(short id = 0)
        {
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        //
        // GET: /Usuario/Create
        [CustomAuthorize(AccessLevel = "gusuarioIndex")]
        public ActionResult Create()
        {
            ViewBag.cd_gusuario = new SelectList(db.GUsuario, "cd_gusuario", "nome");
            return View();
        }

        //
        // POST: /Usuario/Create

        [HttpPost]
        [CustomAuthorize(AccessLevel = "gusuarioIndex")]
        public ActionResult Create(Usuario usuario)
        {
            Usuario oUsuario = new Usuario();
            string strUsuario = usuario.LOGIN.Trim().ToUpper();
            string strEmail = usuario.EMAIL.Trim().ToUpper();

            oUsuario = (Usuario)db.Usuario.Where(s => (s.LOGIN.ToUpper().Equals(strUsuario))).FirstOrDefault();
            if (oUsuario != null)
            {
                if (oUsuario.LOGIN.Trim().ToUpper().Equals(strUsuario))
                {
                    ModelState.AddModelError("login", "Login já existe");
                    return View(usuario);
                }
            }

            oUsuario = (Usuario)db.Usuario.Where(s => (s.EMAIL.ToUpper().Equals(strEmail))).FirstOrDefault();
            if (oUsuario != null)
            {
                if (oUsuario.EMAIL.Trim().ToUpper().Equals(strEmail))
                {
                    ModelState.AddModelError("login", "Email já existe");
                    return View(usuario);
                }
            }

          

            if (ModelState.IsValid)
            {
                Int32? intCD_TIPO = db.Usuario.Max(s => (Int32?)s.CD_USUARIO);

                if (intCD_TIPO != null)
                {
                    intCD_TIPO++;
                }
                else
                {
                    intCD_TIPO = 1;
                }

                usuario.CD_USUARIO = (Int32)intCD_TIPO;
                usuario.SENHA = crypto.Criptografa(usuario.SENHA.Trim().ToUpper());

                
                //db.Usuario.Add(usuario);
                try
                {
                   //db.SaveChanges();
                     string sqlstatament;
                     sqlstatament = string.Format(" INSERT INTO usuario values({0},\'{1}\',\'{2}\',\'{3}\',\'{4}\',{5},{6},\'{7}\',\'{8}\',\'{9}\',\'{10}\',\'{11}\') ",
                        usuario.CD_USUARIO,
                        usuario.NOME,
                        usuario.EMAIL,
                        usuario.LOGIN.ToUpper(),
                        usuario.SENHA,
                        usuario.CD_GUSUARIO,
                        usuario.CD_NL,
                        usuario.APROVA,
                        usuario.REPROVA,
                        usuario.CANCELA,
                        usuario.SITUACAO,
                        usuario.ALT_SENHA);
                       db.Database.ExecuteSqlCommand(sqlstatament);
                     }
                catch (Exception error)
                {
                    throw new Exception(error.ToString());
                }
                return RedirectToAction("Index");
            }

            ViewBag.cd_gusuario = new SelectList(db.GUsuario, "CD_GUSUARIO", "NOME", usuario.CD_GUSUARIO);
            return View(usuario);
        }

        //
        // GET: /Usuario/Edit/5
        [CustomAuthorize(AccessLevel = "operacaofiscalIndex")]
        public JsonResult GetGUsuario()
        {
            return Json(db.GUsuario, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ReadComboAtivo()
        {
            return Json(db.Combo.Where(s => s.TIPO == 2), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReadComboSimouNao()
        {
            return Json(db.Combo.Where(s => s.TIPO == 1), JsonRequestBehavior.AllowGet);
        }


        public JsonResult ReadPagina()
        {
            List<Combo> model = new List<Combo>();
            model.Add(new Combo() { ID = 1, TEXT = "Página com indicadores", VALUE = "1" });
            model.Add(new Combo() { ID = 2, TEXT = "Página em branco", VALUE = "2" });
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        [CustomAuthorize(AccessLevel = "gusuarioIndex")]
        public ActionResult Reset(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                usuario.SENHA = crypto.Criptografa(usuario.SENHA.Trim().ToUpper());
                usuario.ALT_SENHA = "S";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cd_gusuario = new SelectList(db.GUsuario, "cd_gusuario", "nome", usuario.CD_GUSUARIO);
            return View(usuario);
        }

        [CustomAuthorize(AccessLevel = "gusuarioIndex")]
        public ActionResult Reset(short id = 0)
        {
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            
            return View(usuario);
        }

        [CustomAuthorize(AccessLevel = "gusuarioIndex")]
        public ActionResult Edit(short id = 0)
        {
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            //ViewBag.cd_gusuario = new SelectList(db.GUsuario, "cd_gusuario", "nome", usuario.CD_GUSUARIO);
            return View(usuario);
        }

        //
        // POST: /Usuario/Edit/5

        [HttpPost]
        [CustomAuthorize(AccessLevel = "gusuarioIndex")]
        public ActionResult Edit(Usuario usuario)
        {
            Usuario oUsuario = new Usuario();
          
            string strUsuario = usuario.LOGIN.Trim().ToUpper();
            string strEmail = usuario.EMAIL.Trim().ToUpper();


            /* oUsuario = (Usuario)db.Usuario.Where(s => (s.EMAIL.ToUpper().Equals(strEmail))).FirstOrDefault();
            if (oUsuario != null)
            {
                    if (oUsuario.EMAIL.ToUpper().Trim().Equals(strEmail))
                    {
                        ((IObjectContextAdapter)db).ObjectContext.Detach(oUsuario);
                        ModelState.AddModelError("email", "Email Já existe");
                        return View(usuario);
                    }
            }
             * */

            

          

           

            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
               // usuario.SENHA = crypto.Criptografa(usuario.SENHA.Trim().ToUpper());
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cd_gusuario = new SelectList(db.GUsuario, "cd_gusuario", "nome", usuario.CD_GUSUARIO);
            return View(usuario);
        }

        //
        // GET: /Usuario/Delete/5
        [CustomAuthorize(AccessLevel = "gusuarioIndex")]
        public ActionResult Delete(short id = 0)
        {
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        //
        // POST: /Usuario/Delete/5

        [HttpPost, ActionName("Delete")]
        [CustomAuthorize(AccessLevel = "gusuarioIndex")]
        public ActionResult DeleteConfirmed(short id)
        {
            Usuario usuario = db.Usuario.Find(id);
            db.Usuario.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (db != null)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}