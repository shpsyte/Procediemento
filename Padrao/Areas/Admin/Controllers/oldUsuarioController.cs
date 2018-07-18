#region GrupoFiscalController ClassesUsadas
        using System;
        using System.Collections.Generic;
        using System.Data;
        using System.Data.Entity.Validation;
        using System.Data.OleDb;
        using System.IO;
        using System.Linq;
        using System.Web;
        using System.Web.Mvc;
        using System.Web.UI;
        using Kendo.Mvc.Extensions;
        using Kendo.Mvc.UI;
        using Services.Functions;
        using Domain.Entity;
        using Data.Context;
        using System.Data.Entity;
using System.Web.Script.Serialization;
#endregion



namespace b2yweb_mvc4.Areas.Admin.Controllers
{
    [AuthFilter]
    public class oldUsuarioController : Controller
    {
        private b2yweb_entities db = null;
        readonly Funcoes _Funcoes = new Funcoes();
        //
        // GET: /Admin/Usuario/
		/// <summary>
        /// Função Para Verificar se o usuário é autenticado
        /// </summary>
        /// <param name="requestContext"></param>
        [AuthFilter]
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.Session["oEmpresa"] != null)
            {
                db = new b2yweb_entities(requestContext.HttpContext.Session["oEmpresa"].ToString());
            }
        }
		
		
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="strPesquisa">Informar o Termo da Pesquisa</param>
        /// <returns></returns>
		[HttpGet]
		[CustomAuthorize(AccessLevel = "usuarioIndex")]
        public ActionResult Index()
        {
            var usuario = db.Usuario; //.Include(u => u.GUsuario);
            return View(usuario.ToList());
        }

		
		
		
        /// <summary>
        /// Post usada para pesquisa
        /// </summary>
        /// <param name="strPesquisa">Informar o Termo da Pesquisa</param>
        /// <returns></returns>
		[HttpPost]
		[CustomAuthorize(AccessLevel = "usuarioIndex")]
        public ActionResult Index(string strPesquisa)
        {
			if (string.IsNullOrEmpty(strPesquisa))
            {
                ViewData["termo"] = "";
                return View(db.Usuario.ToList());
            }else
            {
                ViewData["termo"] = strPesquisa;
                return View(db.Usuario.ToList()
                     .Where(a => a.NOME.ToUpper().Contains(strPesquisa.ToUpper())  
                               || a.LOGIN.ToUpper().Contains(strPesquisa.ToUpper()) ));
                    //Codigo Gerou na Index desta Controller (Final da Página)
            }
        }
		
        /// <summary>
        /// Método para o Grid do kendo UI
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
		[CustomAuthorize(AccessLevel = "usuarioIndex")]
        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(db.Usuario.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            
        }

		
		
		
        //
        // GET: /Admin/Usuario/Details/5

		[CustomAuthorize(AccessLevel = "usuarioDetails")]
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
        // GET: /Admin/Usuario/Create

		[CustomAuthorize(AccessLevel = "usuarioCreate")]
        public ActionResult Create()
        {
            ViewBag.cd_gusuario = new SelectList(db.GUsuario, "cd_gusuario", "nome");
            return View();
        }

        //
        // POST: /Admin/Usuario/Create

        [HttpPost]
		[CustomAuthorize(AccessLevel = "usuarioCreate")]
        public ActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
				Int32? intcd_usuario = db.Usuario.Max(s => (Int32?)s.CD_USUARIO);
				  
				if (intcd_usuario != null)
                {
                    intcd_usuario++;
                }
                else
                {
                    intcd_usuario = 1;
                }
				  
				usuario.CD_USUARIO = (Int16)intcd_usuario;
				
				
				
                db.Usuario.Add(usuario);
				try
                {
                    db.SaveChanges();
                }
                catch (Exception error)
                {
                    throw new Exception(error.ToString());
                }
			    
                return RedirectToAction("Index");
            }

            ViewBag.cd_gusuario = new SelectList(db.GUsuario, "cd_gusuario", "nome", usuario.CD_GUSUARIO);
            return View(usuario);
        }

        //
        // GET: /Admin/Usuario/Edit/5

		[CustomAuthorize(AccessLevel = "usuarioEdit")]
        public ActionResult Edit(short id = 0)
        {
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.cd_gusuario = new SelectList(db.GUsuario, "cd_gusuario", "nome", usuario.CD_GUSUARIO);
            return View(usuario);
        }

        //
        // POST: /Admin/Usuario/Edit/5

        [HttpPost]
		[CustomAuthorize(AccessLevel = "usuarioEdit")]
        public ActionResult Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cd_gusuario = new SelectList(db.GUsuario, "cd_gusuario", "nome", usuario.CD_GUSUARIO);
            return View(usuario);
        }

        //
        // GET: /Admin/Usuario/Delete/5

		[CustomAuthorize(AccessLevel = "usuarioDelete")]
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
        // POST: /Admin/Usuario/Delete/5

        [HttpPost, ActionName("Delete")]
		[CustomAuthorize(AccessLevel = "usuarioDelete")]
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
		
		[CustomAuthorize(AccessLevel = "usuariofind")]
        public ActionResult find()
        {
            return View();
        }
		
		
		[CustomAuthorize(AccessLevel = "usuarioExportXls")]
        public void ExportXls(String strPesquisa)
        {

            if (String.IsNullOrEmpty(strPesquisa))
            {
                strPesquisa = String.Empty;
            }

            ViewData["termo"] = strPesquisa;

            var usuario  = db.Usuario.ToList();
                     //Codigo Gerou na Index desta Controller (Final da Página)

            var grid = new System.Web.UI.WebControls.GridView();
            grid.DataSource = from _data in usuario select _data;
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=usuario.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
		
		[CustomAuthorize(AccessLevel = "usuarioUpload")]
        public ActionResult Upload()
        {
            return View();
        
        }
		
		[CustomAuthorize(AccessLevel = "usuarioSave")]
		public ActionResult Save(IEnumerable<HttpPostedFileBase> attachments)
        {
            // The Name of the Upload component is "attachments" 
            foreach (var file in attachments)
            {
                // Some browsers send file names with full path. This needs to be stripped.
                var fileName = Path.GetFileName(file.FileName);
                var physicalPath = Path.Combine(Server.MapPath("~/App_Imports"), fileName);
                string exteension = Path.GetExtension(fileName);
                // The files are not actually saved in this demo
                int counter = 1;
                int verifica = 0;
                while (System.IO.File.Exists(physicalPath))
                {
                    counter++;
                    physicalPath = Path.Combine(HttpContext.Server.MapPath("~/App_Imports/"),
                    Path.GetFileNameWithoutExtension(fileName) + counter.ToString() + Path.GetExtension(fileName));
                }
                file.SaveAs(physicalPath);


                DataSet dss = new DataSet();
                string ConnectionString = "";
                if (exteension == ".xls")
                {
                    ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + physicalPath + ";Extended Properties=Excel 8.0;";
                    verifica = 1;
                }

                if (exteension == ".xlsx")
                {
                    ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + physicalPath + ";Extended Properties=Excel 12.0;";
                    verifica = 1;
                }

                if (verifica == 0)
                {
                     throw new Exception("Extensão não suportadaErro ao Salvar");
                }




                using (OleDbConnection conn = new System.Data.OleDb.OleDbConnection(ConnectionString))
                {

                    conn.Open();
                    using (DataTable dtExcelSchema = conn.GetSchema("Tables"))
                    {
                        string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                        string query = "SELECT * FROM [" + sheetName + "]";
                        OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);

                        adapter.Fill(dss, "Items");

                        if (dss.Tables.Count > 0)
                        {
                            if (dss.Tables[0].Rows.Count > 0)
                            {
                                try
                                {

                                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                                    {

                                        Usuario usuario = new Usuario();
                                        int id;

                                       // colocar as colunas aqui para importacao
										//tentar customizar no .tt
										// na index desta controller ao final do arquivo, gerou um codigo padrao para colocar aqui



                                        try
                                        {

                                            db.Usuario.Add(usuario);
                                        }
                                        catch (Exception erro)
                                        {
                                           throw new Exception(erro.ToString());
                                            //return RedirectToAction("ErroAoSalvar");
                                        }


                                                usuario = null;
                                       
                                    }
                                }
                                catch (Exception erro)
                                {
                                    string err = "<b>Erro Gerado na importação do arquivo, consulte os detalhes para mais informações </b> ";
                                    err += "</br>";
                                    err += _Funcoes.TrataErro(erro);
                                    err += "</br>";
                                    throw new Exception(err.ToString());
                                }

                                try
                                {

                                    db.SaveChanges();
                                    return RedirectToAction("Index");
                                }
                                catch (Exception dbEx)
                                {

                                    if (dbEx is System.Data.Entity.Validation.DbEntityValidationException)
                                    {
                                        string errors = "O Arquivo não é válido, verifique as propriedades abaixo para mais detalhes </br> "; // dbEx.EntityValidationErrors.First(); //.ValidationErrors.First();
                                        errors += "<b> Nenhum registro foi gravado.</b> A importação só será possível com o arquivo 100% correto. </br> ";

                                        DbEntityValidationException ex = (DbEntityValidationException)dbEx;
                                        foreach (var validationErrors in ex.EntityValidationErrors)
                                        {
                                            foreach (var validationError in validationErrors.ValidationErrors)
                                            {
                                                errors += string.Format(" A propriedade : <b>{0}</b> não foi validado devido ao erro: <b> {1} </b>", validationError.PropertyName, validationError.ErrorMessage) + "</br>";
                                            }
                                        }
                                       throw new Exception(errors.ToString());
                                    }
                                    else if (dbEx is System.Data.Entity.Infrastructure.DbUpdateException)
                                    {
                                        string err = "<b>Erro Gerado, consulte os detalhes para mais informações </b> ";
                                        err += "</br>";
                                        err += _Funcoes.TrataErro(dbEx);
                                        err += "</br>";
                                        err += dbEx.InnerException.InnerException.ToString();
                                       throw new Exception(err.ToString());

                                    }
                                    else
                                    {
                                        string err = "<b>Erro Gerado, consulte os detalhes para mais informações </b> ";
                                        err += "</br>";
                                        err += _Funcoes.TrataErro(dbEx);
                                        err += "</br>";
                                       throw new Exception(err.ToString());
                                    }

                                }
                            }
                        }
                    }
                }

                // Return an empty string to signify success

            }
            return Content("");
        }


		
        [CustomAuthorize(AccessLevel = "usuarioRemove")]
		public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"
            foreach (var fullName in fileNames)
            {
                var fileName = Path.GetFileName(fullName);
                var physicalPath = Path.Combine(Server.MapPath("~/App_Imports"), fileName);

                // TODO: Verify user permissions
                if (System.IO.File.Exists(physicalPath))
                {
                    // The files are not actually removed in this demo
                    System.IO.File.Delete(physicalPath);
                }
            }
            // Return an empty string to signify success
            return Content("");
        }		
		
		
		
    }
}