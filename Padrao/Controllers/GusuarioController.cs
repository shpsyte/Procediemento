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

namespace b2yweb_mvc4.Controllers
{
    [AuthFilter]
    public class GusuarioController : Controller
    {
        private b2yweb_entities db = null;
        readonly Funcoes _Funcoes = new Funcoes();
        //
        // GET: /Gusuario/
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
        [CustomAuthorize(AccessLevel = "gusuarioIndex")]
        public ActionResult Index()
        {
            return View(db.GUsuario.ToList());
        }

        [HttpGet]
        [CustomAuthorize(AccessLevel = "gusuarioIndex")]
        public ActionResult Permissao(int cd_grupo)
        {

            return View(db.Permissoes.Where(a => a.CD_GUSUARIO == cd_grupo).ToList());
        }




        public JsonResult ReadModulo(int cd_grupo)
        {

            ///int cd_grupo = Convert.ToInt16(Request["cd_grupo"].ToString());
            var _mod = (from a in db.Permissoes.Where(a => a.CD_GUSUARIO == cd_grupo) select a.MODULO).ToList();
            var query = db.Modulos.Where(a => !_mod.Contains(a.DESC_MODULO)).OrderBy(a => a.DESC_MODULO).ToList();



            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(AccessLevel = "gusuarioCreate")]
        public ActionResult CreatePermissoes(int cd_grupo)
        {

            var _mod = (from a in db.Permissoes.Where(a => a.CD_GUSUARIO == cd_grupo) select a.MODULO).ToList();
            var query = db.Modulos.Where(a => !_mod.Contains(a.DESC_MODULO));


            ViewBag.modulo = new SelectList(query, "CD_MODULO", "DESC_MODULO");
            return View();
        }


        [HttpPost]
        [CustomAuthorize(AccessLevel = "gusuarioCreate")]
        public ActionResult CreatePermissoes(Permissoes permissoes)
        {
            int cd_grupo = Convert.ToInt32(Request["cd_grupo"].ToString());

            int qtde = (db.Permissoes.Where(a => a.CD_GUSUARIO == cd_grupo && a.MODULO.ToUpper() == permissoes.MODULO.ToUpper()).Count());

            if (qtde > 0)
            {
                ModelState.AddModelError("MODULO", "Módulo Já Adicionado ao Grupo");
            }

            if (ModelState.IsValid)
            {

                try
                {
                    string sqlquery = string.Format(" INSERT INTO Permissoes VALUES(ID_PERMISSAO_SEQ.nextval, {0}, \'{1}\', \'{2}',\'{3}',\'{4}\',\'{5}\',\'{6}\' ) ",
                                        cd_grupo,
                                        permissoes.MODULO.ToUpper(),
                                        permissoes.ACESSA.ToUpper(),
                                        permissoes.DETALHA.ToUpper(),
                                        permissoes.EDITA.ToUpper(),
                                        permissoes.DELETA.ToUpper(),
                                        permissoes.CRIA.ToUpper()
                                        );
                    db.Database.ExecuteSqlCommand(sqlquery);
                    return RedirectToAction("Permissao", new { cd_grupo = cd_grupo });
                }
                catch (Exception error)
                {
                    throw new Exception(error.ToString());
                }



            }
            ViewBag.modulo = new SelectList(db.Modulos, "CD_MODULO", "DESC_MODULO");

            return RedirectToAction("Create", new { cd_grupo = cd_grupo });

        }




        [AcceptVerbs(HttpVerbs.Post)]
        [CustomAuthorize(AccessLevel = "gusuarioIndex")]
        public ActionResult UpdatePermissao([DataSourceRequest] DataSourceRequest request, Permissoes em)
        {
            if (em != null)
            {
                int id = em.ID_INSERT;


                Permissoes permissoes = db.Permissoes.Find(id);
                permissoes.ACESSA = em.ACESSA.ToUpper();
                permissoes.DETALHA = em.DETALHA.ToUpper();
                permissoes.EDITA = em.EDITA.ToUpper();
                permissoes.DELETA = em.DELETA.ToUpper();
                permissoes.CRIA = em.CRIA.ToUpper();


                db.SaveChanges();
            }

            return Json(ModelState.ToDataSourceResult());
        }

        /// <summary>
        /// Post usada para pesquisa
        /// </summary>
        /// <param name="strPesquisa">Informar o Termo da Pesquisa</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(AccessLevel = "gusuarioIndex")]
        public ActionResult Index(string strPesquisa)
        {
            if (string.IsNullOrEmpty(strPesquisa))
            {
                ViewData["termo"] = "";
                return View(db.GUsuario.ToList());
            }
            else
            {
                ViewData["termo"] = strPesquisa;
                return View(db.GUsuario.Where(a => a.NOME.ToUpper().Contains(strPesquisa.ToUpper())).ToList());
                //Codigo Gerou na Index desta Controller (Final da Página)
            }
        }

        /// <summary>
        /// Método para o Grid do kendo UI
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [CustomAuthorize(AccessLevel = "gusuarioIndex")]
        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(db.GUsuario.ToDataSourceResult(request));
        }




        //
        // GET: /Gusuario/Details/5

        [CustomAuthorize(AccessLevel = "gusuarioDetails")]
        public ActionResult Details(short id = 0)
        {
            GUsuario gusuario = db.GUsuario.Find(id);
            if (gusuario == null)
            {
                return HttpNotFound();
            }
            return View(gusuario);
        }

        //
        // GET: /Gusuario/Create


        [CustomAuthorize(AccessLevel = "DepartamentoIndex")]
        public JsonResult GetDepartamento()
        {
            return Json(db.DEPARTAMENTOes.OrderBy(s => s.DESC_DEPARTAMENTO), JsonRequestBehavior.AllowGet);
        }


        [CustomAuthorize(AccessLevel = "gusuarioCreate")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Gusuario/Create





        [HttpPost]
        [CustomAuthorize(AccessLevel = "gusuarioCreate")]
        public ActionResult Create(GUsuario gusuario)
        {
            if (ModelState.IsValid)
            {
                Int32? intcd_gusuario = db.GUsuario.Max(s => (Int32?)s.CD_GUSUARIO);

                if (intcd_gusuario != null)
                {
                    intcd_gusuario++;
                }
                else
                {
                    intcd_gusuario = 1;
                }

                gusuario.CD_GUSUARIO = (Int32)intcd_gusuario;



                //  db.GUsuario.Add(gusuario);
                try
                {
                    if (!gusuario.CD_DEPARTAMENTO.HasValue)
                    {
                        gusuario.CD_DEPARTAMENTO = 0;
                    }
                    string sql;
                    sql = string.Format(" INSERT INTO GUSUARIO VALUES ({0},\'{1}\', \'{2}\', {3}, {4}, {5})",
                        gusuario.CD_GUSUARIO, gusuario.NOME, gusuario.CD_DEPARTAMENTO, gusuario.CD_PAGINA, gusuario.TMP_MESES_PESQUISA, gusuario.CD_DEPARTAMENTO_DEFAULT);
                    db.Database.ExecuteSqlCommand(sql);
                    //db.SaveChanges();
                }
                catch (Exception error)
                {
                    throw new Exception(error.ToString());
                }

                return RedirectToAction("Index");
            }

            return View(gusuario);
        }

        //
        // GET: /Gusuario/Edit/5

        [CustomAuthorize(AccessLevel = "gusuarioEdit")]
        public ActionResult Edit(short id = 0)
        {
            GUsuario gusuario = db.GUsuario.Find(id);
            if (gusuario == null)
            {
                return HttpNotFound();
            }
            return View(gusuario);
        }

        //
        // POST: /Gusuario/Edit/5

        [HttpPost]
        [CustomAuthorize(AccessLevel = "gusuarioEdit")]
        public ActionResult Edit(GUsuario gusuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gusuario).State = EntityState.Modified;
                if (!gusuario.CD_DEPARTAMENTO.HasValue)
                {
                    gusuario.CD_DEPARTAMENTO = 0;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gusuario);
        }

        //
        // GET: /Gusuario/Delete/5

        [CustomAuthorize(AccessLevel = "gusuarioDelete")]
        public ActionResult Delete(short id = 0)
        {
            GUsuario gusuario = db.GUsuario.Find(id);
            if (gusuario == null)
            {
                return HttpNotFound();
            }
            return View(gusuario);
        }

        //
        // POST: /Gusuario/Delete/5

        [HttpPost, ActionName("Delete")]
        [CustomAuthorize(AccessLevel = "gusuarioDelete")]
        public ActionResult DeleteConfirmed(short id)
        {
            GUsuario gusuario = db.GUsuario.Find(id);
            db.GUsuario.Remove(gusuario);
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

        [CustomAuthorize(AccessLevel = "gusuariofind")]
        public ActionResult find()
        {
            return View();
        }


        [CustomAuthorize(AccessLevel = "gusuarioExportXls")]
        public void ExportXls(String strPesquisa)
        {

            if (String.IsNullOrEmpty(strPesquisa))
            {
                strPesquisa = String.Empty;
            }

            ViewData["termo"] = strPesquisa;

            var gusuario = db.GUsuario.ToList();
            //Codigo Gerou na Index desta Controller (Final da Página)

            var grid = new System.Web.UI.WebControls.GridView();
            grid.DataSource = from _data in gusuario select _data;
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=gusuario.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }

        [CustomAuthorize(AccessLevel = "gusuarioUpload")]
        public ActionResult Upload()
        {
            return View();

        }

        [CustomAuthorize(AccessLevel = "gusuarioSave")]
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

                                        GUsuario gusuario = new GUsuario();
                                        int id;

                                        // colocar as colunas aqui para importacao
                                        //tentar customizar no .tt
                                        // na index desta controller ao final do arquivo, gerou um codigo padrao para colocar aqui



                                        try
                                        {

                                            db.GUsuario.Add(gusuario);
                                        }
                                        catch (Exception erro)
                                        {
                                            throw new Exception(erro.ToString());
                                            //return RedirectToAction("ErroAoSalvar");
                                        }


                                        gusuario = null;

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



        [CustomAuthorize(AccessLevel = "gusuarioRemove")]
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