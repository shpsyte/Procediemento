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
    public class DepartamentoController : Controller
    {
        private b2yweb_entities db = null;
        readonly Funcoes _Funcoes = new Funcoes();
        //
        // GET: /Procedimento/Departamento/
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
		[CustomAuthorize(AccessLevel = "Departamento", Roles= "Admin" )]
        public ActionResult Index()
        {
            return View(db.DEPARTAMENTOes.ToList());
        }

		
		
		
        /// <summary>
        /// Post usada para pesquisa
        /// </summary>
        /// <param name="strPesquisa">Informar o Termo da Pesquisa</param>
        /// <returns></returns>
		[HttpPost]
		[CustomAuthorize(AccessLevel = "departamentoIndex")]
        public ActionResult Index(string strPesquisa)
        {
			if (string.IsNullOrEmpty(strPesquisa))
            {
                ViewData["termo"] = "";
                return View(db.DEPARTAMENTOes.ToList());
            }else
            {
                ViewData["termo"] = strPesquisa;
                return View(db.DEPARTAMENTOes.ToList().Where(a => a.DESC_DEPARTAMENTO.ToUpper().Contains(strPesquisa.ToUpper())));
                    //Codigo Gerou na Index desta Controller (Final da Página)
            }
        }
		
        /// <summary>
        /// Método para o Grid do kendo UI
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
		[CustomAuthorize(AccessLevel = "departamentoIndex")]
        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(db.DEPARTAMENTOes.ToDataSourceResult(request));
        }

		
		
		
        //
        // GET: /Procedimento/Departamento/Details/5

		[CustomAuthorize(AccessLevel = "departamentoDetails")]
        public ActionResult Details(int id = 0)
        {
            DEPARTAMENTO departamento = db.DEPARTAMENTOes.Find(id);
            if (departamento == null)
            {
                return HttpNotFound();
            }
            return View(departamento);
        }

        //
        // GET: /Procedimento/Departamento/Create

		[CustomAuthorize(AccessLevel = "departamentoCreate")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Procedimento/Departamento/Create

        [HttpPost]
		[CustomAuthorize(AccessLevel = "departamentoCreate")]
        public ActionResult Create(DEPARTAMENTO departamento)
        {
            if (ModelState.IsValid)
            {
				Int32? intCD_DEPARTAMENTO = db.DEPARTAMENTOes.Max(s => (Int32?)s.CD_DEPARTAMENTO);
				  
				if (intCD_DEPARTAMENTO != null)
                {
                    intCD_DEPARTAMENTO++;
                }
                else
                {
                    intCD_DEPARTAMENTO = 1;
                }
				  
				departamento.CD_DEPARTAMENTO = (Int32)intCD_DEPARTAMENTO;
				
				
				
                db.DEPARTAMENTOes.Add(departamento);
				try
                {
                    string sql = string.Format(" INSERT INTO DEPARTAMENTO VALUES ({0},\'{1}\',{2},\'{3}\',\'{4}\', \'{5}\')",
                        departamento.CD_DEPARTAMENTO,
                        departamento.DESC_DEPARTAMENTO,
                        departamento.TEMPO_PADRAO,
                        departamento.ENVIA_EMAIL,
                        departamento.ATIVO,
                        departamento.NIVEL_SERVICO);
                        db.Database.ExecuteSqlCommand(sql);
                    //db.SaveChanges();
                }
                catch (Exception error)
                {
                    throw new Exception(error.ToString());
                }
			    
                return RedirectToAction("Index");
            }

            return View(departamento);
        }

        //
        // GET: /Procedimento/Departamento/Edit/5

		[CustomAuthorize(AccessLevel = "departamentoEdit")]
        public ActionResult Edit(int id = 0)
        {
            DEPARTAMENTO departamento = db.DEPARTAMENTOes.Find(id);
            if (departamento == null)
            {
                return HttpNotFound();
            }
            return View(departamento);
        }

        //
        // POST: /Procedimento/Departamento/Edit/5

        [HttpPost]
		[CustomAuthorize(AccessLevel = "departamentoEdit")]
        public ActionResult Edit(DEPARTAMENTO departamento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(departamento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(departamento);
        }

        //
        // GET: /Procedimento/Departamento/Delete/5

		[CustomAuthorize(AccessLevel = "departamentoDelete")]
        public ActionResult Delete(int id = 0)
        {
            DEPARTAMENTO departamento = db.DEPARTAMENTOes.Find(id);
            if (departamento == null)
            {
                return HttpNotFound();
            }
            return View(departamento);
        }

        //
        // POST: /Procedimento/Departamento/Delete/5

        [HttpPost, ActionName("Delete")]
		[CustomAuthorize(AccessLevel = "departamentoDelete")]
        public ActionResult DeleteConfirmed(int id)
        {
            DEPARTAMENTO departamento = db.DEPARTAMENTOes.Find(id);
            db.DEPARTAMENTOes.Remove(departamento);
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
		
		[CustomAuthorize(AccessLevel = "departamentofind")]
        public ActionResult find()
        {
            return View();
        }
		
		
		[CustomAuthorize(AccessLevel = "departamentoExportXls")]
        public void ExportXls(String strPesquisa)
        {

            if (String.IsNullOrEmpty(strPesquisa))
            {
                strPesquisa = String.Empty;
            }

            ViewData["termo"] = strPesquisa;

            var departamento  = db.DEPARTAMENTOes.ToList();
                     //Codigo Gerou na Index desta Controller (Final da Página)

            var grid = new System.Web.UI.WebControls.GridView();
            grid.DataSource = from _data in departamento select _data;
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=departamento.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
		
		[CustomAuthorize(AccessLevel = "departamentoUpload")]
        public ActionResult Upload()
        {
            return View();
        
        }
		
		[CustomAuthorize(AccessLevel = "departamentoSave")]
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

                                        DEPARTAMENTO departamento = new DEPARTAMENTO();
                                        int id;

                                       // colocar as colunas aqui para importacao
										//tentar customizar no .tt
										// na index desta controller ao final do arquivo, gerou um codigo padrao para colocar aqui



                                        try
                                        {

                                            db.DEPARTAMENTOes.Add(departamento);
                                        }
                                        catch (Exception erro)
                                        {
                                           throw new Exception(erro.ToString());
                                            //return RedirectToAction("ErroAoSalvar");
                                        }


                                                departamento = null;
                                       
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


		
        [CustomAuthorize(AccessLevel = "departamentoRemove")]
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