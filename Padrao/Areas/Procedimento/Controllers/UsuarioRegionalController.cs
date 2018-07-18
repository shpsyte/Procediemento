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
#endregion

using Domain.Entity;
using Data.Context;

namespace b2yweb_mvc4.Areas.Procedimento.Controllers
{
    [AuthFilter]
    public class UsuarioRegionalController : Controller
    {
        private b2yweb_entities db = null;
        readonly Funcoes _Funcoes = new Funcoes();
        //
        // GET: /Procedimento/UsuarioRegional/
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
		[CustomAuthorize(AccessLevel = "usuarioregionalIndex")]
        public ActionResult Index(int cd_usuario)
        {
            var usuarioregional = db.UsuarioRegional.Include("Regional").Where(a => a.CD_USUARIO == cd_usuario);
            if (usuarioregional.Count() == 0)
            {
                return RedirectToAction("Create", new { cd_usuario = cd_usuario });

            }


            return View(usuarioregional.ToList());
        }

		
		
		
        /// <summary>
        /// Post usada para pesquisa
        /// </summary>
        /// <param name="strPesquisa">Informar o Termo da Pesquisa</param>
        /// <returns></returns>
		[HttpPost]
		[CustomAuthorize(AccessLevel = "usuarioregionalIndex")]
        public ActionResult Index(int cd_usuario, string strPesquisa)
        {
			if (string.IsNullOrEmpty(strPesquisa))
            {
                ViewData["termo"] = "";
                var usuarioregional = db.UsuarioRegional.Include("Regional").Where(a => a.CD_USUARIO == cd_usuario);
                return View(usuarioregional.ToList());
            }else
            {
                ViewData["termo"] = strPesquisa;
                var usuarioregional = db.UsuarioRegional.Include("Regional").Where(a => a.CD_USUARIO == cd_usuario && a.Usuario.NOME.ToUpper().Contains(strPesquisa.ToUpper()));
                return View(usuarioregional.ToList());
                    //Codigo Gerou na Index desta Controller (Final da Página)
            }
        }
		
        /// <summary>
        /// Método para o Grid do kendo UI
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
		[CustomAuthorize(AccessLevel = "usuarioregionalIndex")]
        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(db.UsuarioRegional.ToDataSourceResult(request));
        }


        public JsonResult ReadRegional()
        {
            return Json(db.Regional.OrderBy(s => s.DESCRICAO), JsonRequestBehavior.AllowGet);
        }

		
        //
        // GET: /Procedimento/UsuarioRegional/Details/5

		[CustomAuthorize(AccessLevel = "usuarioregionalDetails")]
        public ActionResult Details(int id = 0)
        {
            UsuarioRegional usuarioregional = db.UsuarioRegional.Find(id);
            if (usuarioregional == null)
            {
                return HttpNotFound();
            }
            return View(usuarioregional);
        }

        //
        // GET: /Procedimento/UsuarioRegional/Create

		[CustomAuthorize(AccessLevel = "usuarioregionalCreate")]
        public ActionResult Create()
        {
            ViewBag.CD_USUARIO = new SelectList(db.Usuario, "CD_USUARIO", "NOME");
            return View();
        }

        //
        // POST: /Procedimento/UsuarioRegional/Create
        [AcceptVerbs(HttpVerbs.Post)]
        [CustomAuthorize(AccessLevel = "usuarioregionalCreate")]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, UsuarioRegional DpUsu)
        {
            if (DpUsu != null)
            {
                int id = DpUsu.ID;


                UsuarioRegional equipemembro = db.UsuarioRegional.Find(id);
                db.UsuarioRegional.Remove(equipemembro);
                db.SaveChanges();

                //                SessionProductRepository.Delete(product);
            }

            return Json(ModelState.ToDataSourceResult());
        }

        [HttpPost]
		[CustomAuthorize(AccessLevel = "usuarioregionalCreate")]
        public ActionResult Create(UsuarioRegional usuarioregional, int cd_usuario)
        {
            if (ModelState.IsValid)
            {
				Int32? intID = db.UsuarioRegional.Max(s => (Int32?)s.ID);
				  
				if (intID != null)
                {
                    intID++;
                }
                else
                {
                    intID = 1;
                }
				  
				usuarioregional.ID = (Int32)intID;
                usuarioregional.CD_USUARIO = cd_usuario;
				
				
				
//                db.UsuarioRegional.Add(usuarioregional);
				try
                {
                    string sql = string.Format(" INSERT INTO USUARIOREGIONAL VALUES ({0},{1},{2})",
                         usuarioregional.ID,
                         usuarioregional.CD_REGIONAL,
                         usuarioregional.CD_USUARIO);
                    db.Database.ExecuteSqlCommand(sql);
                }
                catch (Exception error)
                {
                    throw new Exception(error.ToString());
                }

                return RedirectToAction("Index", new { cd_usuario = usuarioregional.CD_USUARIO});
            }

            
            return View(usuarioregional);
        }

        //
        // GET: /Procedimento/UsuarioRegional/Edit/5

		[CustomAuthorize(AccessLevel = "usuarioregionalEdit")]
        public ActionResult Edit(int id = 0)
        {
            UsuarioRegional usuarioregional = db.UsuarioRegional.Find(id);
            if (usuarioregional == null)
            {
                return HttpNotFound();
            }
            ViewBag.CD_USUARIO = new SelectList(db.Usuario, "CD_USUARIO", "NOME", usuarioregional.CD_USUARIO);
            return View(usuarioregional);
        }

        //
        // POST: /Procedimento/UsuarioRegional/Edit/5

        [HttpPost]
		[CustomAuthorize(AccessLevel = "usuarioregionalEdit")]
        public ActionResult Edit(UsuarioRegional usuarioregional)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuarioregional).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CD_USUARIO = new SelectList(db.Usuario, "CD_USUARIO", "NOME", usuarioregional.CD_USUARIO);
            return View(usuarioregional);
        }

        //
        // GET: /Procedimento/UsuarioRegional/Delete/5

		[CustomAuthorize(AccessLevel = "usuarioregionalDelete")]
        public ActionResult Delete(int id = 0)
        {
            UsuarioRegional usuarioregional = db.UsuarioRegional.Find(id);
            if (usuarioregional == null)
            {
                return HttpNotFound();
            }
            return View(usuarioregional);
        }

        //
        // POST: /Procedimento/UsuarioRegional/Delete/5

        [HttpPost, ActionName("Delete")]
		[CustomAuthorize(AccessLevel = "usuarioregionalDelete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UsuarioRegional usuarioregional = db.UsuarioRegional.Find(id);
            db.UsuarioRegional.Remove(usuarioregional);
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
		
		[CustomAuthorize(AccessLevel = "usuarioregionalfind")]
        public ActionResult find()
        {
            return View();
        }
		
		
		[CustomAuthorize(AccessLevel = "usuarioregionalExportXls")]
        public void ExportXls(String strPesquisa)
        {

            if (String.IsNullOrEmpty(strPesquisa))
            {
                strPesquisa = String.Empty;
            }

            ViewData["termo"] = strPesquisa;

            var usuarioregional  = db.UsuarioRegional.ToList();
                     //Codigo Gerou na Index desta Controller (Final da Página)

            var grid = new System.Web.UI.WebControls.GridView();
            grid.DataSource = from _data in usuarioregional select _data;
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=usuarioregional.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
		
		[CustomAuthorize(AccessLevel = "usuarioregionalUpload")]
        public ActionResult Upload()
        {
            return View();
        
        }
		
		[CustomAuthorize(AccessLevel = "usuarioregionalSave")]
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

                                        UsuarioRegional usuarioregional = new UsuarioRegional();
                                        int id;

                                       // colocar as colunas aqui para importacao
										//tentar customizar no .tt
										// na index desta controller ao final do arquivo, gerou um codigo padrao para colocar aqui



                                        try
                                        {

                                            db.UsuarioRegional.Add(usuarioregional);
                                        }
                                        catch (Exception erro)
                                        {
                                           throw new Exception(erro.ToString());
                                            //return RedirectToAction("ErroAoSalvar");
                                        }


                                                usuarioregional = null;
                                       
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


		
        [CustomAuthorize(AccessLevel = "usuarioregionalRemove")]
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