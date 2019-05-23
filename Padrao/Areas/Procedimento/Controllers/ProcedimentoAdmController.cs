#region GrupoFiscalController ClassesUsadas
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Services.Functions;
using Domain.Entity;
using Data.Context;
using b2yweb_mvc4.Extends;
using Ionic.Zip;
#endregion

namespace b2yweb_mvc4.Areas.Procedimento.Controllers
{
    [AuthFilter]
    public class ProcedimentoAdmController : Controller
    {
        private b2yweb_entities db = null;
        readonly Funcoes _Funcoes = new Funcoes();
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }
        //
        // GET: /Procedimento/ProcedimentoAdm/
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
        [CustomAuthorize(AccessLevel = "procedimentoadmIndex")]
        public ActionResult Pesquisa()
        {
            return View();
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="strPesquisa">Informar o Termo da Pesquisa</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(AccessLevel = "procedimentoadmIndex")]
        public ActionResult Index()
        {

            



            var procedimentoadm = db.ProcedimentoAdm.Include("Clientes").Include("Regional").Include("TP_PROCEDIMENTO").Include("Usuario");
            return View(procedimentoadm.ToList());
        }


        [HttpPost]
        [CustomAuthorize(AccessLevel = "procedimentoadmIndex")]
        public ActionResult FindDocResult
            (int nr_procedimento = 0
            , string dt_inicial = ""
            , string dt_final = ""
            , int CD_CADASTRO = 0
            , int CD_TIPO = 0
            , int CD_DEPARTAMENTO = 0
            , int CD_USUARIO = 0
            , int ID_SITUACAO = 0
            , int cd_regional = 0
            , string Destino = ""
            )
        {
            //int nr_procedimento = Convert.ToInt32(formCollection["nr_procedimento"].DefaultIfEmpty("0"));
            var list_regional = (List<int>)Session["oRegional"];
            int cd_usuario = ((Usuario)Session["oUsuario"]).CD_USUARIO;
            var list_departamento = (from a in db.DepartamentoUsuario.Where(a => a.CD_USUARIO == cd_usuario) select a.CD_DEPARTAMENTO).ToList();
            int cd_grupo = ((Usuario)Session["oUsuario"]).CD_GUSUARIO;

            DateTime _dt_inicial;
            DateTime _dt_final;
            int _nr_procedimento_ini;
            int _nr_procedimento_fim;
            int _cd_cadastro_ini;
            int _cd_cadastro_fim;
            int _cd_tipo_ini;
            int _cd_tipo_fim;
            int _cd_dep_ini;
            int _cd_dep_fim;
            int _cd_usuario_ini;
            int _cd_usuario_fim;
            int _situacao_ini;
            int _situacao_fim;
            int cd_regional_ini;
            int cd_regional_fim;

            if (cd_regional > 0)
            {
                cd_regional_ini = Convert.ToInt32(cd_regional.ToString());
                cd_regional_fim = Convert.ToInt32(cd_regional.ToString());

            }
            else
            {
                cd_regional_ini = 0;
                cd_regional_fim = 99999999;
            }

            if (ID_SITUACAO > 0)
            {
                _situacao_ini = Convert.ToInt32(ID_SITUACAO.ToString());
                _situacao_fim = Convert.ToInt32(ID_SITUACAO.ToString());

            }
            else
            {
                _situacao_ini = 0;
                _situacao_fim = 99999999;

            }


            if (CD_USUARIO > 0)
            {
                _cd_usuario_ini = Convert.ToInt32(CD_USUARIO.ToString());
                _cd_usuario_fim = Convert.ToInt32(CD_USUARIO.ToString());

            }
            else
            {
                _cd_usuario_ini = 0;
                _cd_usuario_fim = 99999999;

            }



            if (dt_inicial.HasValue())
            {
                _dt_inicial = Convert.ToDateTime(Convert.ToDateTime(dt_inicial.ToString()).ToString("yyyy-MM-dd 00:00:00"));
            }
            else
            {
                _dt_inicial = DateTime.Now.AddYears(-1);
            }


            if (dt_final.HasValue())
            {
                _dt_final = Convert.ToDateTime(Convert.ToDateTime(dt_final.ToString()).ToString("yyyy-MM-dd 23:59:59"));
            }
            else
            {
                _dt_final = DateTime.Now.AddYears(1);
            }

            if (CD_DEPARTAMENTO > 0)
            {
                _cd_dep_ini = Convert.ToInt32(CD_DEPARTAMENTO.ToString());
                _cd_dep_fim = Convert.ToInt32(CD_DEPARTAMENTO.ToString());

            }
            else
            {
                _cd_dep_ini = 0;
                _cd_dep_fim = 99999999;

            }

            if (nr_procedimento > 0)
            {
                _nr_procedimento_ini = Convert.ToInt32(nr_procedimento.ToString());
                _nr_procedimento_fim = Convert.ToInt32(nr_procedimento.ToString());

            }
            else
            {
                _nr_procedimento_ini = 0;
                _nr_procedimento_fim = 99999999;

            }

            if (CD_CADASTRO > 0)
            {
                _cd_cadastro_ini = Convert.ToInt32(CD_CADASTRO.ToString());
                _cd_cadastro_fim = Convert.ToInt32(CD_CADASTRO.ToString());
            }
            else
            {
                _cd_cadastro_ini = 0;
                _cd_cadastro_fim = 9999999;

            }


            if (CD_TIPO > 0)
            {
                _cd_tipo_ini = Convert.ToInt32(CD_TIPO.ToString());
                _cd_tipo_fim = Convert.ToInt32(CD_TIPO.ToString());
            }
            else
            {
                _cd_tipo_ini = 0;
                _cd_tipo_fim = 99999999;

            }


            var ProcedimentoAdm = (
                    from a in db.wProcedimento.Where
                        (c =>
                            (c.CD_PROCEDIMENTO >= _nr_procedimento_ini) &&
                            (c.CD_PROCEDIMENTO <= _nr_procedimento_fim) &&

                            (c.DTA_ABERTURA >= _dt_inicial) &&
                            (c.DTA_ABERTURA <= _dt_final) &&

                            (c.CD_CADASTRO >= _cd_cadastro_ini) &&
                            (c.CD_CADASTRO <= _cd_cadastro_fim) &&

                            (c.CD_TIPO >= _cd_tipo_ini) &&
                            (c.CD_TIPO <= _cd_tipo_fim) &&

                            (c.CD_DEPARTAMENTO >= _cd_dep_ini) &&
                            (c.CD_DEPARTAMENTO <= _cd_dep_fim) &&

                            (c.ID_SITUACAO >= _situacao_ini) &&
                            (c.ID_SITUACAO <= _situacao_fim) &&

                            (c.CD_USUARIO >= _cd_usuario_ini) &&
                            (c.ID_SITUACAO <= _cd_usuario_fim) &&

                            (c.CD_REGIONAL >= cd_regional_ini) &&
                            (c.CD_REGIONAL <= cd_regional_fim))



                    select a).OrderByDescending(a => a.CD_PROCEDIMENTO).ToList();

            if (ProcedimentoAdm.Count() == 0)
            {
                return View("Pesquisa");
            }


            if (ProcedimentoAdm.Count == 0)
            {
                return RedirectToAction("Error", new { Stid = "SemProc" });
            }
            else
            {
                return View(ProcedimentoAdm.OrderByDescending(c => c.CD_PROCEDIMENTO));
            }




        }

        /// <summary>
        /// Post usada para pesquisa
        /// </summary>
        /// <param name="strPesquisa">Informar o Termo da Pesquisa</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(AccessLevel = "procedimentoadmIndex")]
        public ActionResult Index
            (int nr_procedimento = 0
            , string dt_inicial = ""
            , string dt_final = ""
            , int CD_CADASTRO = 0
            , int CD_TIPO = 0
            , int CD_DEPARTAMENTO = 0
            , int CD_USUARIO = 0
            , int ID_SITUACAO = 0
            , int cd_regional = 0
            , string Destino = ""
            )
        {
            //int nr_procedimento = Convert.ToInt32(formCollection["nr_procedimento"].DefaultIfEmpty("0"));
            var list_regional = (List<int>)Session["oRegional"];
            int cd_usuario = ((Usuario)Session["oUsuario"]).CD_USUARIO;
            var list_departamento = (from a in db.DepartamentoUsuario.Where(a => a.CD_USUARIO == cd_usuario) select a.CD_DEPARTAMENTO).ToList();
            int cd_grupo = ((Usuario)Session["oUsuario"]).CD_GUSUARIO;

            DateTime _dt_inicial;
            DateTime _dt_final;
            int _nr_procedimento_ini;
            int _nr_procedimento_fim;
            int _cd_cadastro_ini;
            int _cd_cadastro_fim;
            int _cd_tipo_ini;
            int _cd_tipo_fim;
            int _cd_dep_ini;
            int _cd_dep_fim;
            int _cd_usuario_ini;
            int _cd_usuario_fim;
            int _situacao_ini;
            int _situacao_fim;
            int cd_regional_ini;
            int cd_regional_fim;

            if (cd_regional > 0)
            {
                cd_regional_ini = Convert.ToInt32(cd_regional.ToString());
                cd_regional_fim = Convert.ToInt32(cd_regional.ToString());

            }
            else
            {
                cd_regional_ini = 0;
                cd_regional_fim = 99999999;
            }

            if (ID_SITUACAO > 0)
            {
                _situacao_ini = Convert.ToInt32(ID_SITUACAO.ToString());
                _situacao_fim = Convert.ToInt32(ID_SITUACAO.ToString());

            }
            else
            {
                _situacao_ini = 0;
                _situacao_fim = 99999999;

            }


            if (CD_USUARIO > 0)
            {
                _cd_usuario_ini = Convert.ToInt32(CD_USUARIO.ToString());
                _cd_usuario_fim = Convert.ToInt32(CD_USUARIO.ToString());

            }
            else
            {
                _cd_usuario_ini = 0;
                _cd_usuario_fim = 99999999;

            }



            if (dt_inicial.HasValue())
            {
                _dt_inicial = Convert.ToDateTime(dt_inicial.ToString()).AddDays(-1);
            }
            else
            {
                _dt_inicial = DateTime.Now.AddYears(-1);
            }


            if (dt_final.HasValue())
            {
                _dt_final = Convert.ToDateTime(dt_final.ToString()).AddDays(1);
            }
            else
            {
                _dt_final = DateTime.Now.AddYears(1);
            }

            if (CD_DEPARTAMENTO > 0)
            {
                _cd_dep_ini = Convert.ToInt32(CD_DEPARTAMENTO.ToString());
                _cd_dep_fim = Convert.ToInt32(CD_DEPARTAMENTO.ToString());

            }
            else
            {
                _cd_dep_ini = 0;
                _cd_dep_fim = 99999999;

            }

            if (nr_procedimento > 0)
            {
                _nr_procedimento_ini = Convert.ToInt32(nr_procedimento.ToString());
                _nr_procedimento_fim = Convert.ToInt32(nr_procedimento.ToString());

            }
            else
            {
                _nr_procedimento_ini = 0;
                _nr_procedimento_fim = 99999999;

            }

            if (CD_CADASTRO > 0)
            {
                _cd_cadastro_ini = Convert.ToInt32(CD_CADASTRO.ToString());
                _cd_cadastro_fim = Convert.ToInt32(CD_CADASTRO.ToString());
            }
            else
            {
                _cd_cadastro_ini = 0;
                _cd_cadastro_fim = 9999999;

            }


            if (CD_TIPO > 0)
            {
                _cd_tipo_ini = Convert.ToInt32(CD_TIPO.ToString());
                _cd_tipo_fim = Convert.ToInt32(CD_TIPO.ToString());
            }
            else
            {
                _cd_tipo_ini = 0;
                _cd_tipo_fim = 99999999;

            }


            var ProcedimentoAdm = (
                    from a in db.wProcedimento.Where
                        (c =>
                            (c.CD_PROCEDIMENTO >= _nr_procedimento_ini) &&
                            (c.CD_PROCEDIMENTO <= _nr_procedimento_fim) &&

                            (c.DTA_ABERTURA >= _dt_inicial) &&
                            (c.DTA_ABERTURA <= _dt_final) &&

                            (c.CD_CADASTRO >= _cd_cadastro_ini) &&
                            (c.CD_CADASTRO <= _cd_cadastro_fim) &&

                            (c.CD_TIPO >= _cd_tipo_ini) &&
                            (c.CD_TIPO <= _cd_tipo_fim) &&

                            (c.CD_DEPARTAMENTO >= _cd_dep_ini) &&
                            (c.CD_DEPARTAMENTO <= _cd_dep_fim) &&

                            (c.ID_SITUACAO >= _situacao_ini) &&
                            (c.ID_SITUACAO <= _situacao_fim) &&

                            (c.CD_USUARIO >= _cd_usuario_ini) &&
                            (c.ID_SITUACAO <= _cd_usuario_fim) &&

                            (c.CD_REGIONAL >= cd_regional_ini) &&
                            (c.CD_REGIONAL <= cd_regional_fim) &&

                            (list_regional.Contains(c.CD_REGIONAL)) &&
                            (list_departamento.Contains(c.CD_DEPARTAMENTO)))

                    select a).OrderByDescending(a => a.CD_PROCEDIMENTO).ToList();

            //if (ProcedimentoAdm.Count() == 0)
            //{
            //    return View("Pesquisa");
            //}


            if (ProcedimentoAdm.Count == 0)
            {
                return RedirectToAction("Error", new { Stid = "SemProc" });
            }
            else
            {
                return View(ProcedimentoAdm.OrderByDescending(c => c.CD_PROCEDIMENTO));
            }




        }


        [CustomAuthorize(AccessLevel = "procedimentoadmIndex")]
        public void SetSession(int CD_CADASTRO = 0)
        {
            if (CD_CADASTRO > 0)
            { Session["CadUser"] = CD_CADASTRO; }
            else
            { Session["CadUser"] = 0; }
        }

        [HttpGet]
        [CustomAuthorize(AccessLevel = "procedimentoadmIndex")]
        public ActionResult NFLookup()
        {
            int cd_grupo = ((Usuario)Session["oUsuario"]).CD_GUSUARIO;
            int tempo = db.GUsuario.Where(a => a.CD_GUSUARIO == cd_grupo).Select(a => a.TMP_MESES_PESQUISA).FirstOrDefault();
            DateTime dt_referencia = Convert.ToDateTime(System.DateTime.Now.AddMonths(-tempo).ToString("yyyy-MM-dd 00:00:00"));

            int cd_cadastro = 0;

            try
            {
                cd_cadastro = Convert.ToInt32(Session["CadUser"]);
            }
            catch
            {
                cd_cadastro = 0;
            }



            var list = (List<int>)Session["oRegional"];
            //            int cd_cadastro = Convert.ToInt32(Session["cd_cadastro"].ToString());

            if (cd_cadastro > 0)
            {
                var Notas = db.eNota.Where(c => c.CD_CADASTRO == cd_cadastro).ToList().OrderByDescending(a => a.NR_NOTA).Take(tempo);
                return PartialView("NFLookup", Notas);
            }
            else
            {
                var Notas = db.eNota.ToList().Where(a => a.NR_NOTA == -1).OrderByDescending(a => a.NR_NOTA).Take(0);
                return PartialView("NFLookup", Notas);
            }



        }


        [CustomAuthorize(AccessLevel = "procedimentoadmIndex")]
        public ActionResult PesquisarNF(string NFLookupPesquisa, string aux_cd_cadastro)
        {
            int cd_grupo = ((Usuario)Session["oUsuario"]).CD_GUSUARIO;
            int tempo = db.GUsuario.Where(a => a.CD_GUSUARIO == cd_grupo).Select(a => a.TMP_MESES_PESQUISA).FirstOrDefault();
            DateTime dt_referencia = Convert.ToDateTime(System.DateTime.Now.AddMonths(-tempo).ToString("dd/MM/yyyy 00:00:00"));

            NFLookupPesquisa = NFLookupPesquisa.FormatToB2y();
            int cd_cadastro;

            if (string.IsNullOrEmpty(aux_cd_cadastro))
            {
                cd_cadastro = 0;
            }
            else
            {
                cd_cadastro = Convert.ToInt32(aux_cd_cadastro);
            }



            var list = (List<int>)Session["oRegional"];
            //            int cd_cadastro = Convert.ToInt32(Session["cd_cadastro"].ToString());

            if (cd_cadastro > 0)
            {
                var Notas = db.eNota.Where(c => (c.Clientes.FANTASIA.Contains(NFLookupPesquisa) ||
                                                            c.Clientes.RAZAO.Contains(NFLookupPesquisa))
                                                            && list.Contains(c.CD_REGIONAL)
                                                            && c.CD_CADASTRO == cd_cadastro


                                                     ).ToList().OrderByDescending(a => a.NR_NOTA).Take(tempo);
                return PartialView("NFListaPartial", Notas);
            }
            else
            {
                var Notas = db.eNota.Where(c => (c.Clientes.FANTASIA.Contains(NFLookupPesquisa) ||
                                                            c.Clientes.RAZAO.Contains(NFLookupPesquisa))
                                                            && list.Contains(c.CD_REGIONAL)

                                                     // && c.CD_CADASTRO == cd_cadastro

                                                     ).ToList().OrderByDescending(a => a.NR_NOTA).Take(tempo);
                return PartialView("NFListaPartial", Notas);
            }

        }



        [CustomAuthorize(AccessLevel = "procedimentoadmIndex")]
        public JsonResult ReadNF(int nr_nota, int cd_cadastro)
        {
            var list = (List<int>)Session["oRegional"];

            var Notas = db.eNota.Where(a => a.NR_NOTA == nr_nota && a.CD_CADASTRO == cd_cadastro && list.Contains(a.CD_REGIONAL)).FirstOrDefault();
            //var Notas = db.eNota.Find(nr_nota);
            //eNota Notas = db.eNota.Find(nr_nota);

            if (Notas == null) { Notas = new eNota(); }
            return Json(Notas, JsonRequestBehavior.AllowGet);

        }








        [HttpGet]
        [CustomAuthorize(AccessLevel = "procedimentoadmIndex")]
        public ActionResult TransportadorLookup()
        {
            return PartialView("TransportadorLookup");
        }

        [HttpGet]
        [CustomAuthorize(AccessLevel = "procedimentoadmIndex")]
        public ActionResult CadastroLookup()
        {
            return PartialView("CadastroLookup");
        }


        [CustomAuthorize(AccessLevel = "procedimentoadmCreate")]
        public ActionResult PesquisarDocumentos(int cd_procedimento)
        {
            var Arquivos = db.ProcedimentoAdmArq.Where(a => a.CD_PROCEDIMENTO == cd_procedimento).ToList();
            Session["cd_procedimento"] = cd_procedimento;
            return PartialView("Documentos", Arquivos);


        }



        public ActionResult DownloadDoc(int id)
        {
            var docs = (from a in db.ProcedimentoAdmArq
                        where a.ID_ARQ == id
                        select new
                        {
                            doc = a.DES_IMAGEM,
                            tipo = a.DES_CONTENTTYPE
                        }).ToList();

            byte[] imagedata = (byte[])docs[0].doc;

            return File(imagedata, docs[0].tipo);


            //if (imagedata == null)
            //{
            //    return File("blank.gif", "image/png");
            //}
            //else
            //return File(imagedata, "image/png");


        }

        [CustomAuthorize(AccessLevel = "procedimentoadmIndex")]
        public ActionResult PesquisarTransportador(string CadastroLookupPesquisa)
        {
            CadastroLookupPesquisa = CadastroLookupPesquisa.FormatToB2y();
            var list = (List<int>)Session["oRegional"];

            //var Cadastros_OLD = db.Clientes.ToList();

            var Cadastros = db.TRANSPORTADOR.Where(c => (c.RAZAO.Contains(CadastroLookupPesquisa) ||
                                                        c.FANTASIA.Contains(CadastroLookupPesquisa))).ToList();

            return PartialView("TransportadorListaPartial", Cadastros);
        }


        [CustomAuthorize(AccessLevel = "procedimentoadmIndex")]
        public ActionResult PesquisarCadastro(string CadastroLookupPesquisa)
        {
            CadastroLookupPesquisa = CadastroLookupPesquisa.FormatToB2y();
            var list = (List<int>)Session["oRegional"];

            //var Cadastros_OLD = db.Clientes.ToList();

            var Cadastros = db.Clientes.Where(c => (c.RAZAO.Contains(CadastroLookupPesquisa) ||
                                                        c.FANTASIA.Contains(CadastroLookupPesquisa) ||
                                                        c.CGC_CPF.Contains(CadastroLookupPesquisa))
                                                        && list.Contains(c.CD_REGIONAL)

                                                 ).ToList();

            return PartialView("CadastroListaPartial", Cadastros);
        }


        [CustomAuthorize(AccessLevel = "operacaofiscalIndex")]
        public JsonResult GetCadastro()
        {


            var list = (List<int>)Session["oRegional"];

            var Cadastros = db.Clientes.Where(c => c.CD_CADASTRO == c.CD_CADASTRO && list.Contains(c.CD_REGIONAL)).ToList();
            //var cadastro = (from a in db.Clientes
            //                where list.Contains(a.CD_CADASTRO)
            //                select new {
            //                    CD_CADASTRO = a.CD_CADASTRO,
            //                    RAZAO = a.RAZAO
            //                }).ToList();

            var jsonResult = Json(Cadastros, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }



        [CustomAuthorize(AccessLevel = "operacaofiscalIndex")]
        public JsonResult GetTipoProcedimento()
        {

            var Cadastros = db.TP_PROCEDIMENTO.OrderBy(c => c.DES_TIPO).ToList();

            return Json(Cadastros, JsonRequestBehavior.AllowGet);
        }



        [CustomAuthorize(AccessLevel = "operacaofiscalIndex")]
        public JsonResult GetUsuario()
        {

            var Cadastros = db.Usuario.OrderBy(c => c.NOME).ToList();

            return Json(Cadastros, JsonRequestBehavior.AllowGet);
        }



        [CustomAuthorize(AccessLevel = "operacaofiscalIndex")]
        public JsonResult GetSituacao()
        {

            var Cadastros = db.Situacao.OrderBy(c => c.ID_SITUACAO).ToList();

            return Json(Cadastros, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(AccessLevel = "operacaofiscalIndex")]
        public JsonResult GetRegional()
        {

            var Cadastros = db.Regional.OrderBy(c => c.CD_REGIONAL).ToList();

            return Json(Cadastros, JsonRequestBehavior.AllowGet);
        }



        [CustomAuthorize(AccessLevel = "operacaofiscalIndex")]
        public JsonResult GetStatus()
        {

            var Cadastros = db.Combo.Where(c => c.TIPO == 4).OrderBy(c => c.ID).ToList();

            return Json(Cadastros, JsonRequestBehavior.AllowGet);
        }



        [CustomAuthorize(AccessLevel = "procedimentoadmIndex")]
        public JsonResult ReadCadastro(int cd_cadastro)
        {
            var list = (List<int>)Session["oRegional"];

            var Cadastros = db.Clientes.Where(a => a.CD_CADASTRO == cd_cadastro && list.Contains(a.CD_REGIONAL)).FirstOrDefault();

            if (Cadastros == null) { Cadastros = new Clientes(); }
            return Json(Cadastros, JsonRequestBehavior.AllowGet);
        }


        [CustomAuthorize(AccessLevel = "procedimentoadmIndex")]
        public JsonResult ReadTransportador(int cd_cadastro)
        {
            var Cadastros = db.TRANSPORTADOR.Where(a => a.CD_CADASTRO == cd_cadastro).FirstOrDefault();

            if (Cadastros == null) { Cadastros = new TRANSPORTADOR(); }
            return Json(Cadastros, JsonRequestBehavior.AllowGet);
        }








        [HttpGet]
        [CustomAuthorize(AccessLevel = "procedimentoadmIndex")]
        public ActionResult TipoLookup()
        {

            var tp_procedimento = db.TP_PROCEDIMENTO.Where(c => c.ATIVO == "S").ToList();


            return PartialView("TipoLookup", tp_procedimento);
        }

        [CustomAuthorize(AccessLevel = "procedimentoadmIndex")]
        public ActionResult PesquisarTipo(string TipoLookupPesquisa)
        {
            TipoLookupPesquisa = TipoLookupPesquisa.FormatToB2y();

            var tp_procedimento = db.TP_PROCEDIMENTO.Where(c => (c.DES_TIPO.Contains(TipoLookupPesquisa) && c.ATIVO == "S")).ToList();

            return PartialView("TipoListaPartial", tp_procedimento);
        }





        [CustomAuthorize(AccessLevel = "procedimentoadmIndex")]
        public JsonResult ReadTipo(int cd_tipo)
        {

            var Tipos = db.TP_PROCEDIMENTO.Where(a => a.CD_TIPO == cd_tipo && a.ATIVO == "S").FirstOrDefault();


            if (Tipos == null) { Tipos = new tp_procedimento(); }
            return Json(Tipos, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// Método para o Grid do kendo UI
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [CustomAuthorize(AccessLevel = "procedimentoadmIndex")]
        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(db.ProcedimentoAdm.ToDataSourceResult(request));
        }





        //
        // GET: /Procedimento/ProcedimentoAdm/Create
        // GET: /Usuario/Edit/5
        [CustomAuthorize(AccessLevel = "operacaofiscalIndex")]
        public JsonResult GetDepartamento()
        {
            int cd_usuario = ((Usuario)Session["oUsuario"]).CD_USUARIO;
            int cd_grupo = ((Usuario)Session["oUsuario"]).CD_GUSUARIO;
            int dep_padrao = (from a in db.GUsuario.Where(a => a.CD_GUSUARIO == cd_grupo) select a.CD_DEPARTAMENTO).FirstOrDefault().Value;

            if (dep_padrao == 0)
            {
                return Json(db.DEPARTAMENTOes.Where(a => a.ATIVO == "S").OrderBy(s => s.DESC_DEPARTAMENTO), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(db.DEPARTAMENTOes.Where(a => a.CD_DEPARTAMENTO == dep_padrao && a.ATIVO == "S").OrderBy(s => s.DESC_DEPARTAMENTO), JsonRequestBehavior.AllowGet);
            }
        }


        public class SelectListItemCat
        {


            public bool Selected { get; set; }
            public string Text { get; set; }
            public int Value { get; set; }
        }


        public JsonResult GetMotivos(int id)
        {


            var states = (from a in
                              db.Tp_Procedimento_Motivos.Where(p => p.COD_TIPO == id)
                          select new SelectListItemCat
                          {
                              Value = a.MOTIVOID,
                              Text = a.DES_NOME,
                              Selected = false
                          });


            return Json(new SelectList(states, "Value", "Text"));
        }


        [CustomAuthorize(AccessLevel = "procedimentoadmCreate")]
        public ActionResult Create()
        {
           return Redirect("http://sac.grupofoxlux.com.br/Sac/Create/");


            ViewBag.CD_CADASTRO = new SelectList(db.Clientes, "CD_CADASTRO", "RAZAO");
            ViewBag.CD_REGIONAL = new SelectList(db.Regional, "CD_REGIONAL", "DESCRICAO");
            ViewBag.CD_TIPO = new SelectList(db.TP_PROCEDIMENTO, "CD_TIPO", "DES_TIPO");
            ViewBag.CD_USUARIO = new SelectList(db.Usuario, "CD_USUARIO", "NOME");
            ViewBag.MOTIVOID = new SelectList(db.Tp_Procedimento_Motivos.ToList(), "MOTIVOID", "DES_NOME");
            return View();
        }

        //
        // POST: /Procedimento/ProcedimentoAdm/Create
        public ActionResult Download(string id)
        {
            var path = Path.Combine(Server.MapPath("~/Arquivos/uploads"), id);
            try
            {
                var fs = System.IO.File.OpenRead(path);
                return File(fs, "application/zip", id);
            }
            catch
            {
                throw new HttpException(404, "Couldn't find " + id);
            }
        }



        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [CustomAuthorize(AccessLevel = "procedimentoadmCreate")]
        public JsonResult CreateDocumento(string cd_procedimento, IEnumerable<HttpPostedFileBase> files)
        {

            if (files != null)
            {
                foreach (var a in files)
                {
                    Int32? intCD_PROCEDIMENTOARQ = db.ProcedimentoAdmArq.Max(s => (Int32?)s.ID_ARQ);

                    if (intCD_PROCEDIMENTOARQ != null)
                    {
                        intCD_PROCEDIMENTOARQ++;
                    }
                    else
                    {
                        intCD_PROCEDIMENTOARQ = 1;
                    }
                    string NomeArquivo = cd_procedimento + '_' + intCD_PROCEDIMENTOARQ.ToString() + '_' + string.Format("{0}", Path.GetFileName(a.FileName));
                    var path = Path.Combine(Server.MapPath("~/Arquivos/uploads"), NomeArquivo);
                    string sqlFile = string.Format(" INSERT INTO PROCEDIMENTOARQ (ID_ARQ,CD_PROCEDIMENTO,CAMINHO) VALUES ({0},{1},\'{2}\')",
                        intCD_PROCEDIMENTOARQ,
                        cd_procedimento,
                        NomeArquivo);
                    db.Database.ExecuteSqlCommand(sqlFile);
                    a.SaveAs(path);
                }
            }

            return Json(new Retorno('S', 'N', "OK", "", "", ""));


        }


        [HttpPost]
        [CustomAuthorize(AccessLevel = "procedimentoadmCreate")]
        public ActionResult Create(ProcedimentoAdm procedimentoadm, IEnumerable<HttpPostedFileBase> files)
        {

            return Redirect("http://sac.grupofoxlux.com.br/Sac/Create/");





            ViewBag.CD_CADASTRO = new SelectList(db.Clientes, "CD_CADASTRO", "RAZAO", procedimentoadm.CD_CADASTRO);
            ViewBag.CD_REGIONAL = new SelectList(db.Regional, "CD_REGIONAL", "DESCRICAO", procedimentoadm.CD_REGIONAL);
            ViewBag.CD_TIPO = new SelectList(db.TP_PROCEDIMENTO, "CD_TIPO", "DES_TIPO", procedimentoadm.CD_TIPO);
            ViewBag.CD_USUARIO = new SelectList(db.Usuario, "CD_USUARIO", "NOME", procedimentoadm.CD_USUARIO);
            ViewBag.MOTIVOID = new SelectList(db.Tp_Procedimento_Motivos.ToList(), "MOTIVOID", "DES_NOME", procedimentoadm.MOTIVOID);



            int cd_tipo_procedimento = procedimentoadm.CD_TIPO;
            int? nf_fox = procedimentoadm.NF_FOX.GetValueOrDefault(0);
            int? nf_cliente = procedimentoadm.NF_CLIENTE.GetValueOrDefault(0);

            string NFFOXOBRIGATORIA = (from a in db.TP_PROCEDIMENTO.Where(a => a.CD_TIPO == cd_tipo_procedimento) select a.SOL_NF_OBRIGATORIA).FirstOrDefault().ToString();
            string NFCLIOBRIGATORIA = (from a in db.TP_PROCEDIMENTO.Where(a => a.CD_TIPO == cd_tipo_procedimento) select a.SOL_NF_CLIENTE_OBRIGATORIA).FirstOrDefault().ToString();

            if (procedimentoadm.MOTIVOID == null || procedimentoadm.MOTIVOID == 0)
            {
                ModelState.AddModelError("MOTIVOID", "*");
                return View(procedimentoadm);
            }

            if (NFFOXOBRIGATORIA == "S" && nf_fox == 0)
            {
                ModelState.AddModelError("NF_FOX", "*");
                return View(procedimentoadm);
            }

            if (NFCLIOBRIGATORIA == "S" && nf_cliente == 0)
            {
                ModelState.AddModelError("NF_CLIENTE", "*");
                return View(procedimentoadm);
            }



            if (ModelState.IsValid)
            {


                procedimentoadm.NF_FOX = nf_fox;
                string dta_nf = "";
                string cod_oper = "";

                if (procedimentoadm.NF_FOX != 0)
                {
                    dta_nf = (from a in db.eNota.Where(a => a.NR_NOTA == procedimentoadm.NF_FOX) select a.DT_EMISSAO).FirstOrDefault().ToString();
                    cod_oper = (from a in db.eNota.Where(a => a.NR_NOTA == procedimentoadm.NF_FOX) select a.COD_OPER).FirstOrDefault().ToString();
                }
                else
                {
                    dta_nf = "";
                    cod_oper = "";
                }

                Int32? intCD_PROCEDIMENTO = db.ProcedimentoAdm.Max(s => (Int32?)s.CD_PROCEDIMENTO);

                if (intCD_PROCEDIMENTO != null)
                {
                    intCD_PROCEDIMENTO++;
                }
                else
                {
                    intCD_PROCEDIMENTO = 1;
                }

                procedimentoadm.CD_PROCEDIMENTO = (Int32)intCD_PROCEDIMENTO;
                procedimentoadm.DTA_ABERTURA = DateTime.Now;
                procedimentoadm.ID_SITUACAO = 1;



                procedimentoadm.DTA_NF_FOX = dta_nf;
                procedimentoadm.COD_OPER = cod_oper;

                if (files != null)
                {
                    foreach (var a in files)
                    {
                        Int32? intCD_PROCEDIMENTOARQ = db.ProcedimentoAdmArq.Max(s => (Int32?)s.ID_ARQ);

                        if (intCD_PROCEDIMENTOARQ != null)
                        {
                            intCD_PROCEDIMENTOARQ++;
                        }
                        else
                        {
                            intCD_PROCEDIMENTOARQ = 1;
                        }
                        string NomeArquivo = procedimentoadm.CD_PROCEDIMENTO.ToString() + '_' + intCD_PROCEDIMENTOARQ.ToString() + '_' + string.Format("{0}", Path.GetFileName(a.FileName));
                        var path = Path.Combine(Server.MapPath("~/Arquivos/uploads"), NomeArquivo);
                        string sqlFile = string.Format(" INSERT INTO PROCEDIMENTOARQ VALUES ({0},{1},\'{2}\')",
                            intCD_PROCEDIMENTOARQ,
                            procedimentoadm.CD_PROCEDIMENTO,
                            NomeArquivo);
                        db.Database.ExecuteSqlCommand(sqlFile);
                        a.SaveAs(path);
                    }
                }


                int cd_usuario = ((Usuario)Session["oUsuario"]).CD_USUARIO;
                int cd_regional = (from a in db.Clientes.Where(a => a.CD_CADASTRO == procedimentoadm.CD_CADASTRO) select a.CD_REGIONAL).FirstOrDefault();


                procedimentoadm.CD_USUARIO = cd_usuario;
                procedimentoadm.CD_USUARIO_ALTERACAO = cd_usuario;
                procedimentoadm.OBSATENDIMENTO = "";
                procedimentoadm.CD_REGIONAL = cd_regional;


                // arrumar
                procedimentoadm.CD_ANEXO = 1;



                //                db.ProcedimentoAdm.Add(procedimentoadm);
                try
                {

                    string sql = string.Format(" INSERT INTO PROCEDIMENTO VALUES ({0},{1},{2},{3},{4},{5},{6}, " +
                        " {7} ,\'{8}\',{9},\'{10}\',{11},{12}," +
                       " {13},{14},{15},\'{16}\',{17},{18},\'{19}\',{20},{21},{22},{23},\'{24}\')",
                       procedimentoadm.CD_PROCEDIMENTO,
                       procedimentoadm.CD_CADASTRO,
                       procedimentoadm.CD_USUARIO,
                       procedimentoadm.CD_REGIONAL,
                       procedimentoadm.CD_DEPARTAMENTO,
                       procedimentoadm.CD_TIPO,
                       procedimentoadm.CD_ANEXO,
                       "SYSDATE",
                       procedimentoadm.DTA_FECHAMENTO,
                       procedimentoadm.NF_FOX.GetValueOrDefault(0),
                       procedimentoadm.DTA_NF_FOX,
                       procedimentoadm.NF_CLIENTE.GetValueOrDefault(0),
                       procedimentoadm.VL_TRANSPORTADORA.GetValueOrDefault(0).ToString().Replace(",", "."),
                       procedimentoadm.VL_REPRESENTANTE.GetValueOrDefault(0).ToString().Replace(",", "."),
                       procedimentoadm.VL_FOXLUX.GetValueOrDefault(0).ToString().Replace(",", "."),
                       procedimentoadm.VL_CLIENTE.GetValueOrDefault(0).ToString().Replace(",", "."),
                       procedimentoadm.OBS.Replace("\'", "").Replace("\'\'", ""),
                       procedimentoadm.ID_SITUACAO,
                       procedimentoadm.CD_USUARIO_ALTERACAO,
                       procedimentoadm.OBSATENDIMENTO,
                       procedimentoadm.CD_TRANSPORTADOR,
                       procedimentoadm.VL_DCLIENTE.GetValueOrDefault(0).ToString().Replace(",", "."),
                       procedimentoadm.MOTIVOID,
                       0,
                       procedimentoadm.COD_OPER
                       );
                    db.Database.ExecuteSqlCommand(sql);

                    //insere a linha da troca



                }
                catch (Exception error)
                {
                    throw new Exception(error.ToString());
                }


                SendEmail email = new SendEmail();
                email.EnviarEmail(procedimentoadm.CD_PROCEDIMENTO, "Create");

                return RedirectToAction("inclusao", new { cd_procedimento = procedimentoadm.CD_PROCEDIMENTO });
            }

            return View(procedimentoadm);
        }

        //
        // GET: /Procedimento/ProcedimentoAdm/Edit/5

        [HttpGet]
        [CustomAuthorize(AccessLevel = "procedimentoadmIndex")]
        public ActionResult Alteracao(int cd_procedimento = 0)
        {
            return View();
        }


        [HttpGet]
        [CustomAuthorize(AccessLevel = "procedimentoadmIndex")]
        public ActionResult inclusao(int cd_procedimento = 0)
        {
            return View();
        }



        //
        // GET: /Procedimento/ProcedimentoAdm/Details/5

        [CustomAuthorize(AccessLevel = "procedimentoadmEdit")]
        public ActionResult Error(string Stid = "")
        {
            return View();
        }


        [CustomAuthorize(AccessLevel = "procedimentoadmEdit")]
        public ActionResult FindDoc(string Stid = "")
        {
            return View();
        }



        [CustomAuthorize(AccessLevel = "procedimentoadmEdit")]
        public ActionResult Edit(int id = 0)
        {
            int cd_usuario = ((Usuario)Session["oUsuario"]).CD_USUARIO;
            int cd_grupo = ((Usuario)Session["oUsuario"]).CD_GUSUARIO;
            var list_regional = (List<int>)Session["oRegional"];
            var list_departamento = (from a in db.DepartamentoUsuario.Where(a => a.CD_USUARIO == cd_usuario) select a.CD_DEPARTAMENTO).ToList();

            var procedimentoadmaux = db.ProcedimentoAdm.Where(c => list_regional.Contains(c.CD_REGIONAL) && list_departamento.Contains(c.CD_DEPARTAMENTO) && c.CD_PROCEDIMENTO == id).ToList();
            if (procedimentoadmaux.Count() == 0)
            {
                return RedirectToAction("Error", new { Stid = "ErroNotFound" });

            }


            ProcedimentoAdm procedimentoadm = db.ProcedimentoAdm.Find(id);


            if (procedimentoadm == null)
            {
                return HttpNotFound();
            }

            int situacao = procedimentoadm.ID_SITUACAO;

            if ((situacao == 2) || (situacao == 3) || (situacao == 4))
            {
                return RedirectToAction("Details", new { id = procedimentoadm.CD_PROCEDIMENTO });

            }


            string aprova = (from a in db.Usuario.Where(a => a.CD_USUARIO == cd_usuario) select a.APROVA).FirstOrDefault().ToString();
            string reprova = (from a in db.Usuario.Where(a => a.CD_USUARIO == cd_usuario) select a.REPROVA).FirstOrDefault().ToString();
            string cancela = (from a in db.Usuario.Where(a => a.CD_USUARIO == cd_usuario) select a.CANCELA).FirstOrDefault().ToString();


            List<Situacao> situacoess = db.Situacao.ToList();

            var itemToRemove_2 = situacoess.Single(r => r.ID_SITUACAO == 2);
            var itemToRemove_3 = situacoess.Single(r => r.ID_SITUACAO == 3);
            var itemToRemove_4 = situacoess.Single(r => r.ID_SITUACAO == 4);

            if (aprova == "N")
            {
                situacoess.Remove(itemToRemove_2);
            }

            if (reprova == "N")
            {
                situacoess.Remove(itemToRemove_3);
            }

            if (cancela == "N")
            {
                situacoess.Remove(itemToRemove_4);
            }
            //if (reprova == "N" || aprova == "N")
            //{
            //    situacoess.Remove(itemToRemove_4);
            //}
            //resultList.Remove(itemToRemove);


            ViewBag.ID_SITUACAO = new SelectList(situacoess, "ID_SITUACAO", "DESCRICAO", procedimentoadm.ID_SITUACAO);
            ViewBag.CD_DEPARTAMENTO_DEBITO = new SelectList(db.DEPARTAMENTOes.Where(a => a.ATIVO == "S"), "CD_DEPARTAMENTO", "DESC_DEPARTAMENTO", 1);


            //ViewBag.ID_STATUS = new SelectList(db.Combo.Where(a => a.TIPO == 4 && a.ID == 9), "VALUE", "TEXT", procedimentoadm.ID_STATUS);




            /*            if (reprova == "S")
                        {
                            ViewBag.ID_STATUS = new SelectList(db.Combo.Where(a => a.TIPO == 4 && (a.ID == 9 || a.ID == 11)), "VALUE", "TEXT", procedimentoadm.ID_STATUS);
                        }

                        if (reprova == "S" && aprova == "S")
                        {
                            ViewBag.ID_STATUS = new SelectList(db.Combo.Where(a => a.TIPO == 4 && (a.ID == 9 || a.ID == 10 || a.ID == 11 || a.ID == 12)), "VALUE", "TEXT", procedimentoadm.ID_STATUS);
                        }
                         */




            //if (aprova == "S")
            //{
            //  ViewBag.STATUS = new SelectList(db.Combo.Where(a => a. ID == 9 && a.ID == 10 && a.TIPO == 4) Situacao, "VALUE", "TEXT", procedimentoadm.STATUS);
            //}




            ViewBag.CD_DEPARTAMENTO = new SelectList(db.DEPARTAMENTOes.Where(a => a.ATIVO == "S"), "CD_DEPARTAMENTO", "DESC_DEPARTAMENTO", procedimentoadm.CD_DEPARTAMENTO);

            return View(procedimentoadm);
        }


        [CustomAuthorize(AccessLevel = "procedimentoadmDetails")]
        public ActionResult Details(int id = 0)
        {
            ProcedimentoAdm procedimentoadm = db.ProcedimentoAdm.Find(id);
            if (procedimentoadm == null)
            {
                return View("Pesquisa");
            }
            ViewBag.CD_DEPARTAMENTO = new SelectList(db.DEPARTAMENTOes.Where(a => a.ATIVO == "S"), "CD_DEPARTAMENTO", "DESC_DEPARTAMENTO", procedimentoadm.CD_DEPARTAMENTO);
            ViewBag.ID_SITUACAO = new SelectList(db.Situacao, "ID_SITUACAO", "DESCRICAO", procedimentoadm.ID_SITUACAO);


            int cd_pro = procedimentoadm.CD_PROCEDIMENTO;
            string _obs = "";
            int _cd_usuario = 0;


            if (db.pa_troca_departamentos.Where(a => a.CD_PROCEDIMENTO == cd_pro).Count() > 0)
            {
                _obs = (from a in db.pa_troca_departamentos.Where(a => a.CD_PROCEDIMENTO == cd_pro).OrderByDescending(a => a.NUM_SEQ) select a.OBS).FirstOrDefault().ToString();
                _cd_usuario = (from a in db.pa_troca_departamentos.Where(a => a.CD_PROCEDIMENTO == cd_pro).OrderByDescending(a => a.NUM_SEQ) select a.CD_USUARIO).FirstOrDefault();
            }
            else
            {
                _obs = procedimentoadm.OBS;
                _cd_usuario = procedimentoadm.CD_USUARIO_ALTERACAO;
            }

            string nome = (from a in db.Usuario.Where(a => a.CD_USUARIO == _cd_usuario) select a.NOME).FirstOrDefault().ToString();

            ViewData["ObsFinal"] = _obs;
            ViewData["UsuarioAprovou"] = nome;

            ViewBag.cod_gerado = RetornaDocGeradoSac(id);
            ViewBag.cod_gerado_vinculado = RetornaDocGeradoGat(1, id);
            ViewBag.cod_gerado_final     = RetornaDocGeradoGat(2, id);



            string ssql = $"SELECT DISTINCT  B.DATA_ENTREGA, D.DTEXPEDICAO_DOCEMBARQUE dta_expedicao FROM TB_FRTNFDESPACHADAS A LEFT JOIN TB_FRTDATASENTREGA B   ON(A.DOC_NFDESPACHADAS = B.DOC_NFDESPACHADAS AND B.DATA_ENTREGA IS NOT NULL) LEFT JOIN TB_FRTNFDOCEMBARQUE C   ON(A.DOC_NFDESPACHADAS = C.DOC_NFDESPACHADAS) INNER JOIN TB_FRTDOCEMBARQUE D    ON(C.NUM_DOCEMBARQUE = D.NUM_DOCEMBARQUE) WHERE(NVL(UPPER(D.PLACA_DOCEMBARQUE), 0) NOT LIKE 'DEV%' OR A.CNPJ_EMISSOR LIKE '01723086000%') " +
                $" and A.NUMNF_NFDESPACHADAS = {procedimentoadm.NF_FOX}";

            var dataEntregraExpedicao = db.Database.SqlQuery<DataEntregraExpedicao>(ssql);

            try
            {
                if (dataEntregraExpedicao != null)
                {
                    ViewBag.dta_entrega = dataEntregraExpedicao.First().dta_entrega.HasValue ? dataEntregraExpedicao.First().dta_entrega.Value.ToShortDateString() : "";
                    ViewBag.dta_expedicao = dataEntregraExpedicao.First().dta_expedicao.HasValue ? dataEntregraExpedicao.First().dta_expedicao.Value.ToShortDateString() : "";
                }

            }
            catch (Exception)
            {

                ;
            }


            return View(procedimentoadm);
        }




        public int RetornaDocGeradoGat(int tipo, int cod_procedimento)
        {
            int retorno = 0;

            try { 
            if (tipo == 1)
            {
                Garantia procedimento = db.Garantia.Where(p => p.COD_PROCEDIMENTO_VINCULADO == cod_procedimento).FirstOrDefault();

                if (procedimento != null)
                {
                    retorno = procedimento.GARANTIAID;
                }
            }

            if (tipo == 2)
            {
                Garantia procedimento = db.Garantia.Where(p => p.COD_PROCEDIMENTO_FINAL == cod_procedimento).FirstOrDefault();

                if (procedimento != null)
                {
                    retorno = procedimento.GARANTIAID;
                }
            }
            }catch(Exception e)
            {
                retorno = 0;
            }

            //int doc = procedimento.cod_procedimento == null ? 0 : procedimento.cod_procedimento;

            return retorno;
        }

        public int RetornaDocGeradoSac(int cod_procedimento)
        {
            int retorno = 0;

            try
            {
                SacProcedimento procedimento = db.SacProcedimento.Where(p => p.COD_PROCEDIMENTO == cod_procedimento).FirstOrDefault();
                if (procedimento != null)
                {
                    retorno = procedimento.COD_SAC;
                }
            }catch(Exception e)
            {
                retorno = 0;
            }

            // pode ser que o SAC esteja vinculado a garantia!
            if (retorno == 0)
            {
                GarantiaProcedimento garantia = db.GarantiaProcedimento.Where(p => p.COD_PROCEDIMENTO == cod_procedimento).FirstOrDefault();
                if (garantia != null)
                {
                    SacGarantia sacgarantia = db.SacGarantia.Where(a => a.GARANTIAID == garantia.GARANTIAID).FirstOrDefault();

                    if (sacgarantia != null)
                    {
                        retorno = sacgarantia.COD_SAC;
                    }
                }
            }

            return retorno;
        }


        //[HttpPost]
        //[CustomAuthorize(AccessLevel = "procedimentoadmDetails")]
        //public ActionResult Details(ProcedimentoAdm procedimentoadm, string ObsAtendimento = "")
        //{
        //    if (string.IsNullOrEmpty(ObsAtendimento))
        //    {
        //        ModelState.AddModelError("ObsAtendimento", "Observação Obrigatória");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(procedimentoadm).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Alteracao", new { cd_procedimento = procedimentoadm.CD_PROCEDIMENTO });
        //    }
        //    ViewBag.CD_DEPARTAMENTO = new SelectList(db.DEPARTAMENTOes, "CD_DEPARTAMENTO", "DESC_DEPARTAMENTO", procedimentoadm.CD_DEPARTAMENTO);
        //    ViewBag.ID_SITUACAO = new SelectList(db.Situacao, "ID_SITUACAO", "DESCRICAO", procedimentoadm.ID_SITUACAO);
        //    return View(procedimentoadm);
        //}

        //
        // POST: /Procedimento/ProcedimentoAdm/Edit/5
        public ActionResult Interacoes(int cd_procedimento)
        {
            var procedimentoadm = db.pa_troca_departamentos.Where(c => c.CD_PROCEDIMENTO == cd_procedimento).OrderBy(c => c.NUM_SEQ);
            return View(procedimentoadm.ToList());
        }


        [HttpPost]
        [CustomAuthorize(AccessLevel = "procedimentoadmEdit")]
        public ActionResult Edit(ProcedimentoAdm procedimentoadm, string ObsAtendimentoNovo = "")
        {
            ViewBag.CD_DEPARTAMENTO_DEBITO = new SelectList(db.DEPARTAMENTOes, "CD_DEPARTAMENTO", "DESC_DEPARTAMENTO", procedimentoadm.CD_DEPARTAMENTO_DEBITO);


            if (string.IsNullOrEmpty(ObsAtendimentoNovo))
            {
                ModelState.AddModelError("ObsAtendimento", "Observação Obrigatória");
                ViewBag.CD_DEPARTAMENTO = new SelectList(db.DEPARTAMENTOes, "CD_DEPARTAMENTO", "DESC_DEPARTAMENTO", procedimentoadm.CD_DEPARTAMENTO);
                ViewBag.ID_SITUACAO = new SelectList(db.Situacao, "ID_SITUACAO", "DESCRICAO", procedimentoadm.ID_SITUACAO);


                return View(procedimentoadm);
            }



            if (ModelState.IsValid)
            {
                int cd_usuario = ((Usuario)Session["oUsuario"]).CD_USUARIO;

                procedimentoadm.CD_USUARIO_ALTERACAO = cd_usuario;
                procedimentoadm.OBSATENDIMENTO = ObsAtendimentoNovo;
                db.Entry(procedimentoadm).State = EntityState.Modified;

                //if ((procedimentoadm.ID_SITUACAO == 2) || (procedimentoadm.ID_SITUACAO == 3) || (procedimentoadm.ID_SITUACAO == 4))
                //{
                //    string subPath = Server.MapPath("~/Arquivos/uploads/" + procedimentoadm.CD_PROCEDIMENTO.ToString());
                //    string pPath = Server.MapPath("~/Arquivos/uploads/");
                //    string startPath = subPath;
                //    string zipPath = Server.MapPath("~/Arquivos/uploads/" + procedimentoadm.CD_PROCEDIMENTO.ToString() + "/" + procedimentoadm.CD_PROCEDIMENTO.ToString() + ".zip");

                //    bool isExists = System.IO.Directory.Exists(subPath);

                //    if (!isExists)
                //    {
                //        System.IO.Directory.CreateDirectory(subPath);
                //    }


                //    ZipFile zip = new ZipFile();

                //    foreach (var arquivos in db.ProcedimentoAdmArq.Where(a => a.CD_PROCEDIMENTO == procedimentoadm.CD_PROCEDIMENTO))
                //    {
                //        string Caminho = arquivos.CAMINHO;
                //        string sourceFile = System.IO.Path.Combine(pPath, Caminho);
                //        string destFile = System.IO.Path.Combine(subPath, Caminho);

                //        ProcedimentoAdmArq padarq = db.ProcedimentoAdmArq.Find(arquivos.ID_ARQ);
                //        if (padarq != null)
                //        {
                //            db.ProcedimentoAdmArq.Remove(padarq);
                //        }


                //        //if (System.IO.File.Exists(sourceFile))
                //        // {
                //        //    System.IO.File.Copy(sourceFile, destFile, true);
                //        // }

                //        if (System.IO.File.Exists(sourceFile))
                //        {
                //            zip.AddFile(sourceFile);
                //            // System.IO.File.Delete(sourceFile);
                //        }

                //    }
                //    zip.Save(zipPath);
                //    Int32? intCD_PROCEDIMENTOARQ = db.ProcedimentoAdmArq.Max(s => (Int32?)s.ID_ARQ);
                //    if (intCD_PROCEDIMENTOARQ != null)
                //    {
                //        intCD_PROCEDIMENTOARQ++;
                //    }
                //    else
                //    {
                //        intCD_PROCEDIMENTOARQ = 1;
                //    }

                //    var novo_arquivo = new ProcedimentoAdmArq { ID_ARQ = Convert.ToInt32(intCD_PROCEDIMENTOARQ), CAMINHO = "/" + procedimentoadm.CD_PROCEDIMENTO.ToString() + "/" + procedimentoadm.CD_PROCEDIMENTO.ToString() + ".zip", CD_PROCEDIMENTO = procedimentoadm.CD_PROCEDIMENTO };
                //    ProcedimentoAdmArq novo_arquivo_add = db.ProcedimentoAdmArq.Add(novo_arquivo);

                //}

                db.SaveChanges();






                SendEmail email = new SendEmail();
                email.EnviarEmail(procedimentoadm.CD_PROCEDIMENTO, "Edit");

                return RedirectToAction("Alteracao", new { cd_procedimento = procedimentoadm.CD_PROCEDIMENTO });
            }

            ViewBag.CD_DEPARTAMENTO = new SelectList(db.DEPARTAMENTOes, "CD_DEPARTAMENTO", "DESC_DEPARTAMENTO", procedimentoadm.CD_DEPARTAMENTO);
            ViewBag.ID_SITUACAO = new SelectList(db.Situacao, "ID_SITUACAO", "DESCRICAO", procedimentoadm.ID_SITUACAO);
            return View(procedimentoadm);


        }

        //
        // GET: /Procedimento/ProcedimentoAdm/Delete/5

        [CustomAuthorize(AccessLevel = "procedimentoadmDelete")]
        public ActionResult Delete(int id = 0)
        {
            ProcedimentoAdm procedimentoadm = db.ProcedimentoAdm.Find(id);
            if (procedimentoadm == null)
            {
                return HttpNotFound();
            }
            return View(procedimentoadm);
        }

        //
        // POST: /Procedimento/ProcedimentoAdm/Delete/5

        [HttpPost, ActionName("Delete")]
        [CustomAuthorize(AccessLevel = "procedimentoadmDelete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ProcedimentoAdm procedimentoadm = db.ProcedimentoAdm.Find(id);
            db.ProcedimentoAdm.Remove(procedimentoadm);
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

        [CustomAuthorize(AccessLevel = "procedimentoadmfind")]
        public ActionResult find()
        {
            return View();
        }


        [CustomAuthorize(AccessLevel = "procedimentoadmExportXls")]
        public void ExportXls(String strPesquisa)
        {

            if (String.IsNullOrEmpty(strPesquisa))
            {
                strPesquisa = String.Empty;
            }

            ViewData["termo"] = strPesquisa;

            var procedimentoadm = db.ProcedimentoAdm.ToList();
            //Codigo Gerou na Index desta Controller (Final da Página)

            var grid = new System.Web.UI.WebControls.GridView();
            grid.DataSource = from _data in procedimentoadm select _data;
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=procedimentoadm.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }

        [CustomAuthorize(AccessLevel = "procedimentoadmUpload")]
        public ActionResult Upload()
        {
            return View();

        }

        [CustomAuthorize(AccessLevel = "procedimentoadmSave")]
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

                                        ProcedimentoAdm procedimentoadm = new ProcedimentoAdm();
                                        int id;

                                        // colocar as colunas aqui para importacao
                                        //tentar customizar no .tt
                                        // na index desta controller ao final do arquivo, gerou um codigo padrao para colocar aqui



                                        try
                                        {

                                            db.ProcedimentoAdm.Add(procedimentoadm);
                                        }
                                        catch (Exception erro)
                                        {
                                            throw new Exception(erro.ToString());
                                            //return RedirectToAction("ErroAoSalvar");
                                        }


                                        procedimentoadm = null;

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



        [CustomAuthorize(AccessLevel = "procedimentoadmRemove")]
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