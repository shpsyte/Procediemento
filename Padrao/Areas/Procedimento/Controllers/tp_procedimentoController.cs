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
    
    public class TP_PROCEDIMENTOController : Controller
    {
        private b2yweb_entities db = null;
        readonly Funcoes _Funcoes = new Funcoes();
        //
        // GET: /Procedimento/TP_PROCEDIMENTO/
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
		

        public ActionResult ListaMotivos(int cod_tipo)
        {
         


            var data = db.Tp_Procedimento_Motivos.Where(a => a.COD_TIPO == cod_tipo).ToList();

            return View(data);
        }



        [CustomAuthorize(AccessLevel = "tp_procedimentoCreate")]
        public ActionResult EditMotivo(int cod_tipo, int motivoid)
        {

            Tp_Procedimento_Motivos tp_procedimento = db.Tp_Procedimento_Motivos.Find(cod_tipo, motivoid);
            if (tp_procedimento == null)
            {
                return HttpNotFound();
            }
            return View(tp_procedimento);
            
        }

        //
        // POST: /Procedimento/TP_PROCEDIMENTO/Create

        [HttpPost]
        [CustomAuthorize(AccessLevel = "tp_procedimentoCreate")]
        public ActionResult EditMotivo(int cod_tipo, int motivoid, Tp_Procedimento_Motivos data)
        {
            if (ModelState.IsValid)
            {
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListaMotivos", new { cod_tipo = cod_tipo });
            }
            return View(data);


        }




        [CustomAuthorize(AccessLevel = "tp_procedimentoCreate")]
        public ActionResult CreateMotivo(int cod_tipo)
        {
            return View();
        }

        //
        // POST: /Procedimento/TP_PROCEDIMENTO/Create

        [HttpPost]
        [CustomAuthorize(AccessLevel = "tp_procedimentoCreate")]
        public ActionResult CreateMotivo(int cod_tipo, Tp_Procedimento_Motivos data)
        {
            ModelState.Clear();
            data.MOTIVOID = db.Database.SqlQuery<Int32>("select Tp_Procedimento_MotivosSeq.NextVal from dual ").FirstOrDefault<Int32>();
            data.COD_TIPO = cod_tipo;
            TryValidateModel(data);

            if (ModelState.IsValid)
            {
                try
                {
                    db.Tp_Procedimento_Motivos.Add(data);
                    db.SaveChanges();

                }
                catch (Exception error)
                {
                    throw new Exception(error.ToString());
                }

                return RedirectToAction("ListaMotivos", new { cod_tipo = cod_tipo});
            }

            return View(data);
        }



        /// <summary>
        /// Get
        /// </summary>
        /// <param name="strPesquisa">Informar o Termo da Pesquisa</param>
        /// <returns></returns>
        [HttpGet]
		[CustomAuthorize(AccessLevel = "ADMIN;tp_procedimentoIndex")]
        public ActionResult Index()
        {
            return View(db.TP_PROCEDIMENTO.ToList());
        }

		
		
		
        /// <summary>
        /// Post usada para pesquisa
        /// </summary>
        /// <param name="strPesquisa">Informar o Termo da Pesquisa</param>
        /// <returns></returns>
		[HttpPost]
		[CustomAuthorize(AccessLevel = "tp_procedimentoIndex")]
        public ActionResult Index(string strPesquisa)
        {
			if (string.IsNullOrEmpty(strPesquisa))
            {
                ViewData["termo"] = "";
                return View(db.TP_PROCEDIMENTO.ToList());
            }else
            {
                ViewData["termo"] = strPesquisa;
                return View(db.TP_PROCEDIMENTO.ToList().Where(a => a.DES_TIPO.ToUpper().Contains(strPesquisa.ToUpper())));
                    //Codigo Gerou na Index desta Controller (Final da Página)
            }
        }
		
        /// <summary>
        /// Método para o Grid do kendo UI
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
		[CustomAuthorize(AccessLevel = "tp_procedimentoIndex")]
        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(db.TP_PROCEDIMENTO.ToDataSourceResult(request));
        }

		
		
		
        //
        // GET: /Procedimento/TP_PROCEDIMENTO/Details/5

		[CustomAuthorize(AccessLevel = "tp_procedimentoDetails")]
        public ActionResult Details(int id = 0)
        {
            tp_procedimento tp_procedimento = db.TP_PROCEDIMENTO.Find(id);
            if (tp_procedimento == null)
            {
                return HttpNotFound();
            }
            return View(tp_procedimento);
        }

        //
        // GET: /Procedimento/TP_PROCEDIMENTO/Create

		[CustomAuthorize(AccessLevel = "tp_procedimentoCreate")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Procedimento/TP_PROCEDIMENTO/Create

        [HttpPost]
		[CustomAuthorize(AccessLevel = "tp_procedimentoCreate")]
        public ActionResult Create(tp_procedimento tp_procedimento)
        {
            if (ModelState.IsValid)
            {
				Int32? intCD_TIPO = db.TP_PROCEDIMENTO.Max(s => (Int32?)s.CD_TIPO);
				  
				if (intCD_TIPO != null)
                {
                    intCD_TIPO++;
                }
                else
                {
                    intCD_TIPO = 1;
                }
				  
				tp_procedimento.CD_TIPO = (Int32)intCD_TIPO;


				try
                {
                    //db.TP_PROCEDIMENTO.Add(tp_procedimento);
                    //db.SaveChanges();
                    string sqlstatament;
                     sqlstatament = string.Format(" INSERT INTO TP_PROCEDIMENTO values({0},\'{1}\',\'{2}\',\'{3}\',\'{4}\', {5}) ",
                        tp_procedimento.CD_TIPO,
                        tp_procedimento.DES_TIPO,
                        tp_procedimento.SOL_NF_OBRIGATORIA,
                        tp_procedimento.SOL_NF_CLIENTE_OBRIGATORIA,
                        tp_procedimento.ATIVO,
                        tp_procedimento.TEMPO_PADRAO                        
                        );

                       db.Database.ExecuteSqlCommand(sqlstatament); 

                }
                catch (Exception error)
                {
                    throw new Exception(error.ToString());
                }
				
                /*db.TP_PROCEDIMENTO.Add(tp_procedimento);
				try
                {
                    db.SaveChanges(); << aqui da o erro
                }
                catch (Exception error)
                {
                    throw new Exception(error.ToString());
                }*/
			    
                return RedirectToAction("Index");
            }

            return View(tp_procedimento);
        }

        //
        // GET: /Procedimento/TP_PROCEDIMENTO/Edit/5

		[CustomAuthorize(AccessLevel = "tp_procedimentoEdit")]
        public ActionResult Edit(int id = 0)
        {
            tp_procedimento tp_procedimento = db.TP_PROCEDIMENTO.Find(id);
            if (tp_procedimento == null)
            {
                return HttpNotFound();
            }
            return View(tp_procedimento);
        }

        //
        // POST: /Procedimento/TP_PROCEDIMENTO/Edit/5

        [HttpPost]
		[CustomAuthorize(AccessLevel = "tp_procedimentoEdit")]
        public ActionResult Edit(tp_procedimento tp_procedimento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tp_procedimento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tp_procedimento);
        }

        //
        // GET: /Procedimento/TP_PROCEDIMENTO/Delete/5

		[CustomAuthorize(AccessLevel = "tp_procedimentoDelete")]
        public ActionResult Delete(int id = 0)
        {
            tp_procedimento tp_procedimento = db.TP_PROCEDIMENTO.Find(id);
            if (tp_procedimento == null)
            {
                return HttpNotFound();
            }
            return View(tp_procedimento);
        }

        //
        // POST: /Procedimento/TP_PROCEDIMENTO/Delete/5

        [HttpPost, ActionName("Delete")]
		[CustomAuthorize(AccessLevel = "tp_procedimentoDelete")]
        public ActionResult DeleteConfirmed(int id)
        {
            tp_procedimento tp_procedimento = db.TP_PROCEDIMENTO.Find(id);
            db.TP_PROCEDIMENTO.Remove(tp_procedimento);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        [CustomAuthorize(AccessLevel = "tp_procedimentoDelete")]
        public ActionResult ExcluirMotivo(int cod_tipo, int motivoid)
        {
            
            Tp_Procedimento_Motivos tp_procedimento = db.Tp_Procedimento_Motivos.Find(cod_tipo, motivoid);
            if (tp_procedimento == null)
            {
                return HttpNotFound();
            }
            return View(tp_procedimento);

        }

        //
        // POST: /Procedimento/TP_PROCEDIMENTO/Delete/5

        [HttpPost, ActionName("ExcluirMotivo")]
        [CustomAuthorize(AccessLevel = "tp_procedimentoDelete")]
        public ActionResult ExcluirMotivoConfirmed(int cod_tipo, int motivoid)
        {
            Tp_Procedimento_Motivos tp_procedimento = db.Tp_Procedimento_Motivos.Find(cod_tipo, motivoid);
            db.Tp_Procedimento_Motivos.Remove(tp_procedimento);
            db.SaveChanges();
            return RedirectToAction("ListaMotivos", new { cod_tipo = cod_tipo});
        }

        protected override void Dispose(bool disposing)
        {
            if (db != null)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
		
		[CustomAuthorize(AccessLevel = "tp_procedimentofind")]
        public ActionResult find()
        {
            return View();
        }
		
		
		[CustomAuthorize(AccessLevel = "tp_procedimentoExportXls")]
        public void ExportXls(String strPesquisa)
        {

            if (String.IsNullOrEmpty(strPesquisa))
            {
                strPesquisa = String.Empty;
            }

            ViewData["termo"] = strPesquisa;

            var tp_procedimento  = db.TP_PROCEDIMENTO.ToList();
                     //Codigo Gerou na Index desta Controller (Final da Página)

            var grid = new System.Web.UI.WebControls.GridView();
            grid.DataSource = from _data in tp_procedimento select _data;
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=tp_procedimento.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
		
		[CustomAuthorize(AccessLevel = "tp_procedimentoUpload")]
        public ActionResult Upload()
        {
            return View();
        
        }
		
		[CustomAuthorize(AccessLevel = "tp_procedimentoSave")]
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

                                        tp_procedimento tp_procedimento = new tp_procedimento();
                                        int id;

                                       // colocar as colunas aqui para importacao
										//tentar customizar no .tt
										// na index desta controller ao final do arquivo, gerou um codigo padrao para colocar aqui



                                        try
                                        {

                                            db.TP_PROCEDIMENTO.Add(tp_procedimento);
                                        }
                                        catch (Exception erro)
                                        {
                                           throw new Exception(erro.ToString());
                                            //return RedirectToAction("ErroAoSalvar");
                                        }


                                                tp_procedimento = null;
                                       
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


		
        [CustomAuthorize(AccessLevel = "tp_procedimentoRemove")]
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