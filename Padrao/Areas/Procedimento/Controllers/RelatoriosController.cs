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
using Services.Functions;
using System.Web.Script.Serialization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.Data.SqlClient;
using iTextSharp.text.pdf.draw;
using b2yweb_mvc4.Extends;
using System.Threading;
using System.Globalization;

namespace b2yweb_mvc4.Areas.Procedimento.Controllers
{
    [AuthFilter]
    public class RelatoriosController : Controller
    {
        private b2yweb_entities db = null;
        readonly Funcoes _Funcoes = new Funcoes();
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
        /// Necessário para não dar o erro do JSON quando tem muitos registros
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="contentEncoding"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
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



        [CustomAuthorize(AccessLevel = "Relatorios")]
        public ActionResult ReadProcessos([DataSourceRequest] DataSourceRequest request)
        {
            return Json(db.wProcedimento.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }



        [CustomAuthorize(AccessLevel = "Relatorios")]
        [AuthFilter]
        public ActionResult ReadProcedimentoDepartamento(int COD_PROCEDIMENTO, [DataSourceRequest] DataSourceRequest request)
        {
            var res = (from a in db.wpa_troca_departamentos
                       where 1 == 1
                       && a.CD_PROCEDIMENTO == COD_PROCEDIMENTO
                       orderby a.NUM_SEQ descending
                       select a);

            return Json(res.ToDataSourceResult(request));
        }

        //



        [AuthFilter]
        [CustomAuthorize(AccessLevel = "Relatorios")]
        public ActionResult TempoLiberacaoResult(int nr_procedimento = 0)
        {
            var ProcedimentoAdm = (
             from a in db.ProcedimentoAdm.ToList() select a);

            var trocas = (from a in db.wpa_troca_departamentos select a.CD_DEPARTAMENTO_NOVA).ToList();


            ViewData["Customers"] = ProcedimentoAdm;
            ViewData["Orders"] = (from a in db.wpa_troca_departamentos
                                  where 1 == 2
                                  orderby a.CD_DEPARTAMENTO_NOVA descending
                                  select a);



            return View();
        }


        // GET: /Procedimento/Relatorios/
        [CustomAuthorize(AccessLevel = "Relatorios")]
        [AuthFilter]
        public ActionResult ProcedimentoDepartamento()
        {
            return View();
        }

        [CustomAuthorize(AccessLevel = "Relatorios")]
        [HttpPost]
        public ActionResult ProcedimentoDepartamentoResult(int nr_procedimento = 0
            , string dt_inicial = ""
            , string dt_final = ""
            , int CD_CADASTRO = 0
            , int CD_TIPO = 0
            , int CD_DEPARTAMENTO = 0
            , int CD_USUARIO = 0
            , int ID_SITUACAO = 0
            , string Destino = "")
        {
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

            var ProcedimentoAdm =
                  (from a in db.wProcedimento.Where
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
                          (c.CD_USUARIO <= _cd_usuario_fim)).ToList()
                   select a).OrderByDescending(a => a.CD_PROCEDIMENTO).ToList();


            ViewData["Customers"] = ProcedimentoAdm;
            ViewData["Orders"] = (from a in db.wpa_troca_departamentos
                                  where 1 == 2
                                  orderby a.CD_DEPARTAMENTO_NOVA descending
                                  select a);



            return View(ProcedimentoAdm);



        }



        // GET: /Procedimento/Relatorios/
        [CustomAuthorize(AccessLevel = "Relatorios")]
        [AuthFilter]
        public ActionResult TempoLiberacao()
        {
            return View();
        }

        [CustomAuthorize(AccessLevel = "Relatorios")]
        [HttpPost]
        public ActionResult TempoLiberacaoResult(int nr_procedimento = 0
            , string dt_inicial = ""
            , string dt_final = ""
            , int CD_CADASTRO = 0
            , int CD_TIPO = 0
            , int CD_DEPARTAMENTO = 0
            , int CD_USUARIO = 0
            , int ID_SITUACAO = 0
            , string Destino = "")
        {
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
                          (c.CD_USUARIO <= _cd_usuario_fim))

                  select a).OrderByDescending(a => a.CD_PROCEDIMENTO).ToList();

            var trocas = (from a in db.pa_troca_departamentos select a.CD_DEPARTAMENTO_NOVA).ToList();


            ViewData["Customers"] = ProcedimentoAdm;
            ViewData["Orders"] = (from a in db.wpa_troca_departamentos
                                  where 1 == 2
                                  orderby a.CD_DEPARTAMENTO_NOVA descending
                                  select a);



            return View();



        }



        [CustomAuthorize(AccessLevel = "Relatorios")]
        [AuthFilter]
        public ActionResult PrintPro()
        {
            return View();
        }


        [CustomAuthorize(AccessLevel = "Relatorios")]
        [AuthFilter]
        public FileStreamResult PrintProPDF(int nr_procedimento = 0)
        {
            MemoryStream workStream = new MemoryStream();
            Document document = new Document();
            //PdfWriter.GetInstance(document, workStream).CloseStream = false;
            string imagepath = Server.MapPath("~/Images");
            Image gif = Image.GetInstance(imagepath + "/Logo_P.gif");
            PdfWriter writer = PdfWriter.GetInstance(document, workStream);
            writer.CloseStream = false;
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Font times = new Font(bfTimes, 12, Font.NORMAL, Color.RED);
            Font arial8 = FontFactory.GetFont("Arial", 12, Color.BLACK);
            Font verdana = FontFactory.GetFont("Verdana", 16, Font.BOLD, Color.ORANGE);
            Font fverdana = FontFactory.GetFont("Verdana", 12, Color.BLACK);

            Font palatino = FontFactory.GetFont(
             "palatino linotype italique",
              BaseFont.CP1252,
              BaseFont.EMBEDDED,
              10,
              Font.ITALIC,
              Color.GREEN
              );
            Font smallfont = FontFactory.GetFont("Arial", 7);
            Font xarial = FontFactory.GetFont("Arial");
            xarial.SetStyle("Bold");


            document.Open();
            document.Add(gif);
            document.Add(new Paragraph(" "));

            //PdfContentByte cb = writer.DirectContent;
            //cb.Rectangle(10f, 200f, 800f, 600f);
            //cb.Stroke();


            ProcedimentoAdm proc = db.ProcedimentoAdm.Find(nr_procedimento);
            var trocas = (from a in db.wpa_troca_departamentos.Where(a => a.CD_PROCEDIMENTO == nr_procedimento) select a).OrderBy(a => a.NUM_SEQ).ToList();
            string ultima = (from a in db.wpa_troca_departamentos.Where(a => a.CD_PROCEDIMENTO == nr_procedimento).OrderByDescending(a => a.NUM_SEQ)
                             select a.OBS).FirstOrDefault();



            Paragraph _TITULO = new Paragraph();
            _TITULO.Alignment = Element.ALIGN_CENTER;
            _TITULO.Add(new Chunk("HISTÓRICO DE PROCEDIMENTO", verdana));
            Chunk linebreak = new Chunk(new LineSeparator(1f, 100f, Color.LIGHT_GRAY, Element.ALIGN_CENTER, -1));

            document.Add(_TITULO);
            //document.Add(new Paragraph("______________________________________________________________________________"));
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph("Dados do Cliente:", arial8));







            PdfContentByte cb = writer.DirectContent;
            //cb.SetColorStroke(new CMYKColor(1f, 0f, 0f, 0f));
            //cb.SetColorFill(new CMYKColor(0f, 0f, 1f, 0f));
            //cb.MoveTo(70, 200);
            //cb.LineTo(170, 200);
            //cb.LineTo(170, 300);
            //cb.LineTo(70, 300);
            //cb.ClosePathStroke();


            document.Add(linebreak);

            PdfPTable table_cliente = new PdfPTable(2);
            table_cliente.TotalWidth = 520f;
            table_cliente.LockedWidth = true;
            table_cliente.DefaultCell.BorderWidth = 1;
            float[] widths = new float[] { 80f, 200f };
            table_cliente.SetWidths(widths);
            table_cliente.HorizontalAlignment = 0;
            //table_cliente.SpacingBefore = 20f;
            //table_cliente.SpacingAfter = 30f;


            fverdana.Size = 8;
            Paragraph paragrafo = new Paragraph();

            table_cliente.AddCell(new Chunk("Nº do Procedimento Administrativo : ", fverdana).ToString());
            table_cliente.AddCell(new Chunk(proc.CD_PROCEDIMENTO.ToString(), fverdana).ToString());

            document.Add(table_cliente);

            //document.Add(paragrafo);

            paragrafo.Clear();
            paragrafo.Add(new Chunk(string.Format("Cód. Cliente: {0}", proc.CD_CADASTRO), fverdana));
            document.Add(paragrafo);

            paragrafo.Clear();
            paragrafo.Add(new Chunk(string.Format("Razão Social: {0}", proc.Clientes.RAZAO), fverdana));
            document.Add(paragrafo);

            paragrafo.Clear();
            paragrafo.Add(new Chunk(string.Format("Transportador: {0}", proc.TRANSPORTADOR.RAZAO), fverdana));
            document.Add(paragrafo);

            document.Add(new Paragraph(" "));
            paragrafo.Clear();
            paragrafo.Add(new Chunk(string.Format("Última: {0}", ultima), fverdana));
            document.Add(paragrafo);


            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));

            PdfPTable table = new PdfPTable(5);
            table.TotalWidth = 540f;
            table.LockedWidth = true;
            table.DefaultCell.BorderWidth = 1;

            float[] widths_tab = new float[] { 40f, 40f, 40f, 40f, 40f };
            table.SetWidths(widths_tab);
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            table.AddCell("Nome");
            table.AddCell("Departamento");
            table.AddCell("Entrada");
            table.AddCell("Saida");
            table.AddCell("Horas");

            foreach (var x in trocas)
            {
                table.AddCell(x.Usuario.NOME);
                table.AddCell(x.DEPANT.DESC_DEPARTAMENTO);
                table.AddCell(x.DTA_ENTRADA_DEP_NOVA.ToString());
                table.AddCell(x.DTA_SAIDA_DEP_NOVA.ToString());
                table.AddCell(x.HORASCORRIDAS);
                //   table.AddCell(x.OBS);
            }
            document.Add(table);




            /*PdfPTable table_2_colunas = new PdfPTable(2);
            table_2_colunas.TotalWidth = 340f;
            table_2_colunas.LockedWidth = true;
            table_2_colunas.DefaultCell.BorderWidth = 0;
            float[] widths = new float[] { 40f, 20f };
            table_2_colunas.SetWidths(widths);
            table_2_colunas.HorizontalAlignment = 0;
            table_2_colunas.SpacingBefore = 20f;
            table_2_colunas.SpacingAfter = 30f;


            paragrafo.Add(new Chunk("Nº do Procedimento Administrativo :", fverdana));
            table_2_colunas.AddCell(paragrafo);
            table_2_colunas.AddCell(proc.CD_PROCEDIMENTO.ToString());
            document.Add(table_2_colunas);



            PdfPTable table_cliente = new PdfPTable(4);
            paragrafo.Clear();
            table_cliente.TotalWidth = 540f;
            table_cliente.LockedWidth = true;
            table_cliente.DefaultCell.BorderWidth = 1;
            float[] widths_cliente = new float[] { 20f, 40f, 20f, 40f };
            table_cliente.SetWidths(widths_cliente);
            table_cliente.HorizontalAlignment = 0;
            table_cliente.SpacingBefore = 20f;
            table_cliente.SpacingAfter = 30f;
            paragrafo.Add(new Chunk("Nº do Procedimento Administrativo :", fverdana));
            table_cliente.AddCell(paragrafo);
            paragrafo.Clear();
            paragrafo.Add(new Chunk(proc.CD_CADASTRO.ToString(), fverdana));
            table_cliente.AddCell(paragrafo);
            paragrafo.Clear();
            paragrafo.Add(new Chunk("Razão Social", fverdana));
            table_cliente.AddCell(paragrafo);
            paragrafo.Clear();
            paragrafo.Add(new Chunk(proc.Clientes.RAZAO, fverdana));
            table_cliente.AddCell(paragrafo);
            document.Add(table_cliente);
            
            */




            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return new FileStreamResult(workStream, "application/pdf");
        }

        [CustomAuthorize(AccessLevel = "Relatorios")]
        [AuthFilter]
        public ActionResult TMR()
        {
            return View();
        }

        [CustomAuthorize(AccessLevel = "Relatorios")]
        [HttpPost]
        public ActionResult TMRR(
            int nr_procedimento = 0
            , string dt_inicial = ""
            , string dt_final = ""
            , int CD_TIPO = 0
             , int CD_DEPARTAMENTO = 0
            , string UF = ""
            , string Destino = "")
        {
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

            decimal Vlrp = new decimal(0.02);


            var retorno_aux =
              (from a in db.wpa_troca_departamentos
               where a.DTA_TROCA >= _dt_inicial &&
                a.DTA_TROCA <= _dt_final &&
                a.ProcedimentoAdm.CD_TIPO >= _cd_tipo_ini &&
                a.ProcedimentoAdm.CD_TIPO <= _cd_tipo_fim &&
                a.CD_DEPARTAMENTO_NOVA >= _cd_dep_ini &&
                a.CD_DEPARTAMENTO_NOVA <= _cd_dep_fim &&
                a.CD_PROCEDIMENTO >= _nr_procedimento_ini &&
                a.CD_PROCEDIMENTO <= _nr_procedimento_fim &&
                (!string.IsNullOrEmpty(UF) ? UF.Contains(a.ProcedimentoAdm.Clientes.CD_ESTADO.ToUpper()) : 1 == 1)
               select a).ToList();



            var retorno =
                from a in db.wpa_troca_departamentos
                where a.DTA_TROCA >= _dt_inicial &&
                 a.DTA_TROCA <= _dt_final &&
                 a.ProcedimentoAdm.CD_TIPO >= _cd_tipo_ini &&
                 a.ProcedimentoAdm.CD_TIPO <= _cd_tipo_fim &&
                 a.CD_DEPARTAMENTO_NOVA >= _cd_dep_ini &&
                 a.CD_DEPARTAMENTO_NOVA <= _cd_dep_fim &&
                 a.CD_PROCEDIMENTO >= _nr_procedimento_ini &&
                 a.CD_PROCEDIMENTO <= _nr_procedimento_fim &&
                 (!string.IsNullOrEmpty(UF) ? UF.Contains(a.ProcedimentoAdm.Clientes.CD_ESTADO.ToUpper()) : 1 == 1)
                 && a.HORASNUMBER >= Vlrp
                group a by new
                {
                    a.CD_PROCEDIMENTO,
                    a.ProcedimentoAdm.Clientes.FANTASIA,
                    a.ProcedimentoAdm.tp_procedimento.DES_TIPO,
                    a.DEPNOVA.DESC_DEPARTAMENTO
                } into g
                select new wpa_troca_departamentos_unico
                {
                    Procedimento = g.Key.CD_PROCEDIMENTO,
                    Fantasia = g.Key.FANTASIA,
                    Tipo = g.Key.DES_TIPO,
                    Departamento = g.Key.DESC_DEPARTAMENTO,
                    Horas = g.Sum(p => p.HORASNUMBER),
                    HorasString = ""
                };


            var retorno_result = retorno.ToList();

            var retorno_json = (from a in retorno_result
                                select new wpa_troca_departamentos_unico
                                {
                                    Procedimento = a.Procedimento,
                                    Fantasia = a.Fantasia,
                                    Tipo = a.Tipo,
                                    Departamento = a.Departamento,
                                    HorasString = string.Concat(
                                    Math.Truncate(Convert.ToDecimal(a.Horas)).ToString(), ":",
                                    Convert.ToString(Math.Truncate(Math.Round(((Convert.ToDecimal(a.Horas) - Math.Truncate(Convert.ToDecimal(a.Horas))) * 60), 2))).PadLeft(2, '0')
                                    ),
                                    Horas = 0
                                });




            decimal? averageTicks = 0;

            try
            {
                averageTicks = retorno_result.Where(erika => erika.Horas > Vlrp).Select(d => d.Horas).Average();
            }
            catch
            {
                averageTicks = 0;
            }

            string tempo_medio = string.Concat(Math.Truncate(Convert.ToDecimal(averageTicks)).ToString().PadLeft(2, '0'), ":", Convert.ToString(Math.Truncate(Math.Round(((Convert.ToDecimal(averageTicks) - Math.Truncate(Convert.ToDecimal(averageTicks))) * 60), 2))).PadLeft(2, '0'));

            decimal? qtde_fora = retorno_aux.Where(a => a.PERCENTUAL > 100).Count();

            ViewData["tempo_medio_resposta"] = tempo_medio;
            ViewData["qtde_fora"] = qtde_fora;



            return View(retorno_json);


        }




        [CustomAuthorize(AccessLevel = "Relatorios")]
        [AuthFilter]
        public ActionResult TempoMedio()
        {
            return View();
        }

        [CustomAuthorize(AccessLevel = "Relatorios")]
        [HttpPost]
        public ActionResult TempoMedioResult(
            int nr_procedimento = 0
            , string dt_inicial_fecha = ""
            , string dt_final_fecha = ""
            , string dt_inicial = ""
            , string dt_final = ""
            , int CD_TIPO = 0
             , int CD_DEPARTAMENTO = 0
            , string UF = ""
            , string Destino = "",
            [DataSourceRequest] DataSourceRequest request = null)
        {
            var list_regional = (List<int>)Session["oRegional"];
            int cd_usuario = ((Usuario)Session["oUsuario"]).CD_USUARIO;
            var list_departamento = (from a in db.DepartamentoUsuario.Where(a => a.CD_USUARIO == cd_usuario) select a.CD_DEPARTAMENTO).ToList();
            int cd_grupo = ((Usuario)Session["oUsuario"]).CD_GUSUARIO;

            DateTime _dt_inicial;
            DateTime _dt_final;
            DateTime _dt_inicial_fecha;
            DateTime _dt_final_fecha;

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



            if (dt_inicial_fecha.HasValue())
            {
                _dt_inicial_fecha = Convert.ToDateTime(Convert.ToDateTime(dt_inicial_fecha.ToString()).ToString("yyyy-MM-dd 00:00:00"));
            }
            else
            {
                _dt_inicial_fecha = DateTime.Now.AddYears(-1);
            }


            if (dt_final_fecha.HasValue())
            {
                _dt_final_fecha = Convert.ToDateTime(Convert.ToDateTime(dt_final_fecha.ToString()).ToString("yyyy-MM-dd 23:59:59"));
            }
            else
            {
                _dt_final_fecha = DateTime.Now.AddYears(1);
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


            //var ProcedimentoAdm =
            //      (from a in db.wProcedimento.Where
            //          (c =>

            //              (c.DTA_ABERTURA >= _dt_inicial) &&
            //              (c.DTA_ABERTURA <= _dt_final) &&

            //              (c.CD_TIPO >= _cd_tipo_ini) &&
            //              (c.CD_TIPO <= _cd_tipo_fim) &&

            //              (c.CD_DEPARTAMENTO >= _cd_dep_ini) &&
            //              (c.CD_DEPARTAMENTO <= _cd_dep_fim) &&

            //              ( !string.IsNullOrEmpty(UF) ?  UF.Contains(c.Clientes.CD_ESTADO.ToUpper()) : 1 == 1  )

            //              ).ToList()
            //       select a).OrderByDescending(a => a.CD_PROCEDIMENTO).ToList();

            decimal Vlrp = new decimal(0.02);
            var retorno =
                (from a in db.wpa_troca_departamentos
                 join b in db.ProcedimentoAdm on a.CD_PROCEDIMENTO equals b.CD_PROCEDIMENTO
                 where
                     a.DTA_TROCA >= _dt_inicial &&
                     a.DTA_TROCA <= _dt_final &&

                     b.DTA_FECHAMENTO >= _dt_inicial_fecha &&
                     b.DTA_FECHAMENTO <= _dt_final_fecha &&


                     a.DTA_TROCA >= _dt_inicial &&
                     a.DTA_TROCA <= _dt_final &&

                     a.ProcedimentoAdm.CD_TIPO >= _cd_tipo_ini &&
                     a.ProcedimentoAdm.CD_TIPO <= _cd_tipo_fim &&

                     a.CD_DEPARTAMENTO_NOVA >= _cd_dep_ini &&
                     a.CD_DEPARTAMENTO_NOVA <= _cd_dep_fim &&

                     a.CD_PROCEDIMENTO >= _nr_procedimento_ini &&
                     a.CD_PROCEDIMENTO <= _nr_procedimento_fim &&

                     (!string.IsNullOrEmpty(UF) ? UF.Contains(a.ProcedimentoAdm.Clientes.CD_ESTADO.ToUpper()) : 1 == 1)
                 select a).ToList().OrderByDescending(a => a.CD_PROCEDIMENTO).ToList();


            if (retorno.Count() > 0)
            {
                decimal? averageTicks = retorno.Where(erika => erika.HORASNUMBER > Vlrp).Select(d => d.HORASNUMBER).Average();

                decimal? qtde_fora = retorno.Where(a => a.PERCENTUAL > 100).Count();
                //decimal? qtde_dentro = retorno.Where(a => a.PERCENTUAL > a.DEPANT.TEMPO_PADRAO).Count();

                //string tempo_medio = string.Concat(Math.Truncate(Convert.ToDecimal(averageTicks)).ToString(),":", Convert.ToString( Math.Round(   ((Convert.ToDecimal(averageTicks) -  Math.Truncate(Convert.ToDecimal(averageTicks)) ) * 60 ) , 4)   ));
                string tempo_medio = string.Concat(Math.Truncate(Convert.ToDecimal(averageTicks)).ToString().PadLeft(2, '0'), ":", Convert.ToString(Math.Truncate(Math.Round(((Convert.ToDecimal(averageTicks) - Math.Truncate(Convert.ToDecimal(averageTicks))) * 60), 2))).PadLeft(2, '0'));

                ViewData["tempo_medio_resposta"] = tempo_medio;
                ViewData["qtde_fora"] = qtde_fora;
            }
            else
            {
                ViewData["tempo_medio_resposta"] = 0;
                ViewData["qtde_fora"] = 0;
            }


            /*    var serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                var result = new ContentResult();
                result.Content = serializer.Serialize(retorno);
                result.ContentType = "text/json"; */
            // var jsonResult = Json(retorno, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            //return jsonResult;
            //return View(jsonResult);


            /* var serializer = new JavaScriptSerializer();
             var result = new ContentResult();
             serializer.MaxJsonLength = Int32.MaxValue; // Whatever max length you want here
             result.Content = serializer.Serialize(retorno.ToDataSourceResult(request));
             result.ContentType = "application/json";
             return result;
             * */


            return View(retorno);


        }




        [CustomAuthorize(AccessLevel = "Relatorios")]
        [AuthFilter]
        public ActionResult ListagemGeral()
        {
            return View();
        }

        [CustomAuthorize(AccessLevel = "Relatorios")]
        [HttpPost]
        public ActionResult ListagemGeralResult(int nr_procedimento = 0
            , string dt_inicial = ""
            , string dt_final = ""
            , int CD_CADASTRO = 0
            , int CD_TIPO = 0
            , int CD_DEPARTAMENTO = 0
            , int CD_USUARIO = 0
            , int ID_SITUACAO = 0
            , string Destino = ""
            , int cod_representante = 0)
        {
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
            int _cd_representante_ini;
            int _cd_representante_fim;



            if (cod_representante > 0)
            {
                _cd_representante_ini = cod_representante;
                _cd_representante_fim = cod_representante;
            }else
            {
                _cd_representante_ini = 0;
                _cd_representante_fim = int.MaxValue;
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
                _dt_inicial = DateTime.MinValue;
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

            var ProcedimentoAdm =
                  (from a in db.wProcedimento.Where
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
                          (c.CD_USUARIO <= _cd_usuario_fim) &&

                          (c.COD_REPRESENTANTE >= _cd_representante_ini) &&
                          (c.COD_REPRESENTANTE <= _cd_representante_fim) 
                          



                          ).ToList()
                   select a).OrderByDescending(a => a.CD_PROCEDIMENTO).ToList();












            return View(ProcedimentoAdm);



        }


        [CustomAuthorize(AccessLevel = "procedimentoadmExportXls")]
        public void ExportXls(wProcedimento wpro)
        {


            var procedimentoadm = db.wProcedimento.ToList();
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





        [CustomAuthorize(AccessLevel = "Relatorios")]
        [AuthFilter]
        public ActionResult PrintNFDebito()
        {
            return View();
        }


        [CustomAuthorize(AccessLevel = "Relatorios")]
        [AuthFilter]
        public FileStreamResult PrintNFDebitoPDF(int nr_procedimento = 0,
            string motivo = "", DateTime? dta_vencimento = null, decimal vlr_credito = 0, string sacado = "T")
        {

            if (!dta_vencimento.HasValue)
            {
                dta_vencimento = System.DateTime.Now.AddDays(28);
            }

            ProcedimentoAdm proc = db.ProcedimentoAdm.Find(nr_procedimento);
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(iTextSharp.text.PageSize.A5);
            Document document = new Document(rec);
            PdfWriter writer = PdfWriter.GetInstance(document, workStream);
            writer.CloseStream = false;


            if (proc == null)
            {

                writer.PageEvent = new PDFFooter(titulo: "NOTA DE DÉBITO", canhoto: true);
                document.Open();
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph("  NÃO EXISTEM DADSO PARA LISTAR "));

                document.Close();

                byte[] byteInfoS = workStream.ToArray();
                workStream.Write(byteInfoS, 0, byteInfoS.Length);
                workStream.Position = 0;


                return new FileStreamResult(workStream, "application/pdf");
            }


            var transportador = db.TRANSPORTADOR.Where(a => a.CD_CADASTRO == proc.CD_TRANSPORTADOR).FirstOrDefault();
            var cliente = db.Clientes.Where(a => a.CD_CADASTRO == proc.CD_CADASTRO).FirstOrDefault();


            if ("T".Equals(sacado))
            {

                if (transportador == null || proc == null)
                {

                    writer.PageEvent = new PDFFooter(titulo: "NOTA DE DÉBITO", canhoto: true);
                    document.Open();
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph("  NÃO EXISTEM DADSO PARA LISTAR "));

                    document.Close();

                    byte[] byteInfoS = workStream.ToArray();
                    workStream.Write(byteInfoS, 0, byteInfoS.Length);
                    workStream.Position = 0;


                    return new FileStreamResult(workStream, "application/pdf");
                }
            }


            if ("C".Equals(sacado))
            {

                if (cliente == null || proc == null)
                {

                    writer.PageEvent = new PDFFooter(titulo: "NOTA DE DÉBITO", canhoto: true);
                    document.Open();
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph("  NÃO EXISTEM DADSO PARA LISTAR "));

                    document.Close();

                    byte[] byteInfoS = workStream.ToArray();
                    workStream.Write(byteInfoS, 0, byteInfoS.Length);
                    workStream.Position = 0;


                    return new FileStreamResult(workStream, "application/pdf");
                }
            }

            //PdfWriter.GetInstance(document, workStream).CloseStream = false;
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Font times = new Font(bfTimes, 12, Font.NORMAL, Color.RED);
            Font arial8 = FontFactory.GetFont("Arial", 12, Color.BLACK);
            Font verdana = FontFactory.GetFont("Verdana", 16, Font.BOLD, Color.ORANGE);
            Font fverdana = FontFactory.GetFont("Verdana", 12, Color.BLACK);
            Font normal = FontFactory.GetFont("Verdana", 8, Font.BOLD, Color.BLACK);

            Font palatino = FontFactory.GetFont(
             "palatino linotype italique",
              BaseFont.CP1252,
              BaseFont.EMBEDDED,
              10,
              Font.NORMAL, Color.ORANGE
              );

            Font palatino_xs = FontFactory.GetFont(
             "palatino linotype italique",
              BaseFont.CP1252,
              BaseFont.EMBEDDED,
              12,
              Font.BOLD, Color.RED
              );


            Font smallfont = FontFactory.GetFont("Arial", 6, Color.GRAY);
            Font xarial = FontFactory.GetFont("Arial");
            xarial.SetStyle("Bold");

            Font palatino_i = FontFactory.GetFont(
             "palatino linotype italique",
              BaseFont.CP1252,
              BaseFont.EMBEDDED,
              8,
              Font.NORMAL, Color.BLACK
              );


            writer.PageEvent = new PDFHeaderND(nr_procedimento, Convert.ToInt32(proc.NF_FOX), false);

            document.Open();

            //gif.ScalePercent(45);
            //document.Add(gif);
            document.Add(new Paragraph(" "));

            //PdfContentByte cb = writer.DirectContent;
            //cb.Rectangle(10f, 200f, 800f, 600f);
            //cb.Stroke();

            float[] largura_colunas = new float[] { 1f, 2f };
            Chunk linebreak = new Chunk(new LineSeparator(1f, 100f, Color.LIGHT_GRAY, Element.ALIGN_CENTER, -1));
            PdfPCell celula = new PdfPCell();


            Paragraph _TituloParagrafo = new Paragraph();
            _TituloParagrafo.Alignment = Element.ALIGN_LEFT;



            PdfPTable header = new PdfPTable(2);
            header.TotalWidth = document.PageSize.Width - 10;
            header.LockedWidth = true;
            header.DefaultCell.BorderWidth = 1;
            header.HorizontalAlignment = Element.ALIGN_LEFT;
            float[] largura_col = new float[] { 15f, 100f };
            header.SetWidths(largura_col);

            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");


            Paragraph _TITULO = new Paragraph();
            _TITULO.Alignment = Element.ALIGN_CENTER;
            _TITULO.Add(new Chunk("Pinhais " + System.DateTime.Now.ToLongDateString(), normal));
            document.Add(_TITULO);

            document.Add(new Paragraph(" "));




            //linha do numero
            celula = new PdfPCell(new Paragraph(new Chunk("SACADO:", palatino))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk("T".Equals(sacado) ? string.Concat(transportador.CD_CADASTRO, "-", transportador.RAZAO) : string.Concat(cliente.CD_CADASTRO, "-", cliente.RAZAO), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);


            celula = new PdfPCell(new Paragraph(new Chunk("CNPJ:", palatino))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk("T".Equals(sacado) ? Convert.ToUInt64(transportador.CGC_CPF).ToString(@"00\.000\.000\/0000\-00") : Convert.ToUInt64(cliente.CGC_CPF).ToString(@"00\.000\.000\/0000\-00"), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);



            celula = new PdfPCell(new Paragraph(new Chunk("", palatino))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(string.Concat(transportador.DES_CIDADE, "-", transportador.UF), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk("", palatino))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);


            document.Add(header);

            document.Add(new Paragraph(" "));

            _TITULO = new Paragraph();
            _TITULO.Alignment = Element.ALIGN_CENTER;
            _TITULO.Add(new Chunk("Comunicamos que lançamos o seguinte débito em sua conta na Foxlux Ltda", normal));
            document.Add(_TITULO);



            PdfPTable table = new PdfPTable(3);
            table.TotalWidth = document.PageSize.Width - 10;
            table.LockedWidth = true;
            table.DefaultCell.BorderWidth = 0;
            table.HorizontalAlignment = 1;
            float[] largura_colunas_coleta = new float[] { 300f, 100f, 100f };
            table.SetWidths(largura_colunas_coleta);


            document.Add(new Paragraph(" "));


            celula = new PdfPCell(new Paragraph(new Chunk("Dados do lançamento ", palatino))) { Colspan = 3, Border = 0, HorizontalAlignment = Element.ALIGN_CENTER };
            table.AddCell(celula);


            celula = new PdfPCell(new Paragraph(new Chunk("Histórico", palatino)));
            table.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk("Débito", palatino)));
            table.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk("Crédito", palatino)));
            table.AddCell(celula);


            celula = new PdfPCell(new Paragraph(new Chunk("Cliente: " + proc.Clientes.RAZAO, palatino_i)));
            table.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk(proc.VL_TRANSPORTADORA.HasValue ? proc.VL_TRANSPORTADORA.Value.ToString("c") : "", palatino_i))) { HorizontalAlignment = Element.ALIGN_CENTER };
            table.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk(vlr_credito.ToString("c"), palatino_i)));
            table.AddCell(celula);


            celula = new PdfPCell(new Paragraph(new Chunk("Cnpj: " + Convert.ToUInt64(proc.Clientes.CGC_CPF).ToString(@"00\.000\.000\/0000\-00"), palatino_i)));
            table.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk("", palatino_i))) { HorizontalAlignment = Element.ALIGN_CENTER };
            table.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk("", palatino_i)));
            table.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk("Motivo: " + motivo, palatino_i)));
            table.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk("", palatino_i))) { HorizontalAlignment = Element.ALIGN_CENTER };
            table.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk("", palatino_i)));
            table.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk("Procedimento: " + proc.CD_PROCEDIMENTO.ToString(), palatino_i)));
            table.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk("", palatino_i))) { HorizontalAlignment = Element.ALIGN_CENTER };
            table.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk("", palatino_i)));
            table.AddCell(celula);



            celula = new PdfPCell(new Paragraph(new Chunk("Total: ", palatino_i))) { BackgroundColor = Color.GRAY };
            table.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk(proc.VL_TRANSPORTADORA.HasValue ? proc.VL_TRANSPORTADORA.Value.ToString("c") : "", palatino_i))) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = Color.GRAY };
            table.AddCell(celula);
            celula = new PdfPCell(new Paragraph(new Chunk(vlr_credito.ToString("c"), palatino_i))) { BackgroundColor = Color.GRAY };
            table.AddCell(celula);

            document.Add(table);

            _TITULO = new Paragraph();
            _TITULO.Alignment = Element.ALIGN_RIGHT;
            _TITULO.Add(new Chunk("Total Geral da Nota " + (((proc.VL_TRANSPORTADORA.HasValue ? proc.VL_TRANSPORTADORA.Value : 0) - vlr_credito).ToString("c")), palatino_xs));
            document.Add(_TITULO);


            document.Add(new Paragraph(" "));

            _TITULO = new Paragraph();
            _TITULO.Alignment = Element.ALIGN_CENTER;
            _TITULO.Add(new Chunk("Data de Vencimento .: " + dta_vencimento.Value.ToShortDateString(), normal));
            document.Add(_TITULO);


            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return new FileStreamResult(workStream, "application/pdf");


        }






        [CustomAuthorize(AccessLevel = "Relatorios")]
        [AuthFilter]
        public FileStreamResult PrintNFDebitoPDFNovaVersao(int nr_procedimento = 0,
            string motivo = "", DateTime? dta_vencimento = null, decimal vlr_credito = 0, string sacado = "T")
        {

            if (!dta_vencimento.HasValue)
            {
                dta_vencimento = System.DateTime.Now.AddDays(28);
            }

            ProcedimentoAdm proc = db.ProcedimentoAdm.Find(nr_procedimento);
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(iTextSharp.text.PageSize.LETTER);
            Document document = new Document(rec);
            PdfWriter writer = PdfWriter.GetInstance(document, workStream);
            writer.CloseStream = false;


            if (proc == null)
            {

                writer.PageEvent = new PDFFooter(titulo: "NOTA DE DÉBITO", canhoto: true);
                document.Open();
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph("  NÃO EXISTEM DADSO PARA LISTAR "));

                document.Close();

                byte[] byteInfoS = workStream.ToArray();
                workStream.Write(byteInfoS, 0, byteInfoS.Length);
                workStream.Position = 0;


                return new FileStreamResult(workStream, "application/pdf");
            }


            var transportador = db.TRANSPORTADOR.Where(a => a.CD_CADASTRO == proc.CD_TRANSPORTADOR).FirstOrDefault();
            var cliente = db.Clientes.Where(a => a.CD_CADASTRO == proc.CD_CADASTRO).FirstOrDefault();


            if ("T".Equals(sacado))
            {

                if (transportador == null || proc == null)
                {

                    writer.PageEvent = new PDFFooter(titulo: "NOTA DE DÉBITO", canhoto: true);
                    document.Open();
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph("  NÃO EXISTEM DADSO PARA LISTAR "));

                    document.Close();

                    byte[] byteInfoS = workStream.ToArray();
                    workStream.Write(byteInfoS, 0, byteInfoS.Length);
                    workStream.Position = 0;


                    return new FileStreamResult(workStream, "application/pdf");
                }
            }


            if ("C".Equals(sacado))
            {

                if (cliente == null || proc == null)
                {

                    writer.PageEvent = new PDFFooter(titulo: "NOTA DE DÉBITO", canhoto: true);
                    document.Open();
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph("  NÃO EXISTEM DADSO PARA LISTAR "));

                    document.Close();

                    byte[] byteInfoS = workStream.ToArray();
                    workStream.Write(byteInfoS, 0, byteInfoS.Length);
                    workStream.Position = 0;


                    return new FileStreamResult(workStream, "application/pdf");
                }
            }

            //PdfWriter.GetInstance(document, workStream).CloseStream = false;
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Font times = new Font(bfTimes, 12, Font.NORMAL, Color.RED);
            Font arial8 = FontFactory.GetFont("Arial", 12, Color.BLACK);
            Font verdana = FontFactory.GetFont("Verdana", 16, Font.BOLD, Color.ORANGE);
            Font fverdana = FontFactory.GetFont("Verdana", 12, Color.BLACK);
            Font normal = FontFactory.GetFont("Verdana", 12, Font.BOLD, Color.BLACK);
            Font tituloemnegrito = FontFactory.GetFont("Verdana", 8, Font.BOLD, Color.BLACK);

            

            Font palatino = FontFactory.GetFont(
             "palatino linotype italique",
              BaseFont.CP1252,
              BaseFont.EMBEDDED,
              10,
              Font.NORMAL, Color.ORANGE
              );

            Font palatino_xs = FontFactory.GetFont(
             "palatino linotype italique",
              BaseFont.CP1252,
              BaseFont.EMBEDDED,
              12,
              Font.BOLD, Color.RED
              );

            string valor_total = (((proc.VL_TRANSPORTADORA.HasValue ? proc.VL_TRANSPORTADORA.Value : 0) - vlr_credito).ToString("c"));

            Font smallfont = FontFactory.GetFont("Arial", 6, Color.GRAY);
            Font xarial = FontFactory.GetFont("Arial");
            xarial.SetStyle("Bold");

            Font palatino_i = FontFactory.GetFont(
             "palatino linotype italique",
              BaseFont.CP1252,
              BaseFont.EMBEDDED,
              8,
              Font.BOLD, Color.BLACK
              );

            Font palatino_nome = FontFactory.GetFont(
            "palatino linotype italique",
             BaseFont.CP1252,
             BaseFont.EMBEDDED,
             10,
             Font.BOLD, Color.BLACK
             );


            writer.PageEvent = new PDFHeaderNDVersaoNova(nr_procedimento, Convert.ToInt32(proc.NF_FOX), false);

            document.Open();

            //gif.ScalePercent(45);
            //document.Add(gif);

            //PdfContentByte cb = writer.DirectContent;
            //cb.Rectangle(10f, 200f, 800f, 600f);
            //cb.Stroke();
            PdfPTable recTable = new PdfPTable(1);
            recTable.TotalWidth = document.PageSize.Width - 10;
            recTable.LockedWidth = true;
            
            recTable.DefaultCell.BorderColor = Color.GRAY;
            recTable.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
            recTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            recTable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
            recTable.SetWidths(new float[] { 1f });
            recTable.AddCell(new PdfPCell(new Phrase(new Chunk("Pinhais, " + String.Format("{0:d \\de MMMM \\de yyyy}", System.DateTime.Now), normal)))
             { MinimumHeight = 40, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_CENTER, PaddingTop = 10 });
            document.Add(recTable);


            //            document.Add(new Paragraph(" "));

            recTable = new PdfPTable(2);
            recTable.TotalWidth = document.PageSize.Width - 10;
            recTable.LockedWidth = true;

            recTable.DefaultCell.BorderColor = Color.GRAY;
            recTable.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
            recTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            recTable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
            recTable.SetWidths(new float[] { 20f, 80f });

            recTable.AddCell(new PdfPCell() { Colspan = 2, MinimumHeight = 15 });

            recTable.AddCell(new PdfPCell(new Paragraph(new Chunk("SACADO:", normal))) { HorizontalAlignment = PdfCell.ALIGN_CENTER });
            recTable.AddCell(new PdfPCell(new Paragraph(new Chunk("T".Equals(sacado) ? string.Concat(transportador.CD_CADASTRO, "-", transportador.RAZAO) : string.Concat(cliente.CD_CADASTRO, "-", cliente.RAZAO), palatino_nome))) { HorizontalAlignment = PdfCell.ALIGN_CENTER });

            recTable.AddCell(new PdfPCell(new Paragraph(new Chunk("CNPJ:", normal))) { HorizontalAlignment = PdfCell.ALIGN_CENTER });
            recTable.AddCell(new PdfPCell(new Paragraph(new Chunk("T".Equals(sacado) ? Convert.ToUInt64(transportador.CGC_CPF).ToString(@"00\.000\.000\/0000\-00") : Convert.ToUInt64(cliente.CGC_CPF).ToString(@"00\.000\.000\/0000\-00"), palatino_nome))) { HorizontalAlignment = PdfCell.ALIGN_CENTER });

            recTable.AddCell(new PdfPCell() { Colspan = 2, MinimumHeight = 10 });

            document.Add(recTable);


            recTable = new PdfPTable(1);
            recTable.TotalWidth = document.PageSize.Width - 10;
            recTable.LockedWidth = true;

            recTable.DefaultCell.BorderColor = Color.GRAY;
            recTable.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
            recTable.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            recTable.DefaultCell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            recTable.SetWidths(new float[] { 1f });
            recTable.AddCell(new PdfPCell(new Phrase(new Chunk("Comunicamos que nesta data foi lançado o débito abaixo descrito: ", normal)))
            { MinimumHeight = 40, PaddingTop = 10 });
            document.Add(recTable);



            recTable = new PdfPTable(1);
            recTable.TotalWidth = document.PageSize.Width - 10;
            recTable.LockedWidth = true;

            recTable.DefaultCell.BorderColor = Color.GRAY;
            recTable.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
            recTable.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            recTable.DefaultCell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            recTable.SetWidths(new float[] { 100f });
            //60 40

            recTable.AddCell(new PdfPCell(new Phrase(new Chunk("Histórico: ", new nFont())))
            { MinimumHeight = 15, BackgroundColor = Color.GRAY,  PaddingTop = 5 , HorizontalAlignment = PdfCell.ALIGN_CENTER });

          //  recTable.AddCell(new PdfPCell(new Phrase(new Chunk("Valor: ", new nFont())))
          //  { MinimumHeight = 15, BackgroundColor = Color.GRAY, PaddingTop = 5, HorizontalAlignment = PdfCell.ALIGN_CENTER });


            recTable.AddCell(new PdfPCell(new Phrase(new Chunk("T".Equals(sacado) ? "CLIENTE: " + proc.Clientes.RAZAO : "TRANSPORTADOR:" + proc.TRANSPORTADOR.RAZAO, new nFont("Verdana",8,true,Color.BLACK)))));
            //if ("T".Equals(sacado))
            //{ 
            //    recTable.AddCell(new PdfPCell(new Phrase(new Chunk(proc.VL_TRANSPORTADORA.HasValue ? proc.VL_TRANSPORTADORA.Value.ToString("c") : "", new nFont("Verdana", 8, true, Color.BLACK)))) { HorizontalAlignment = PdfCell.ALIGN_CENTER });
            //}else
            //{
            //    recTable.AddCell(new PdfPCell(new Phrase(new Chunk(vlr_credito.ToString("c"), new nFont("Verdana", 8, true, Color.BLACK)))) { HorizontalAlignment = PdfCell.ALIGN_CENTER });

            //}

            recTable.AddCell(new PdfPCell(new Phrase(new Chunk("CNPJ: " + Convert.ToUInt64(proc.Clientes.CGC_CPF).ToString(@"00\.000\.000\/0000\-00"), new nFont("Verdana", 8, true, Color.BLACK)))));
            //recTable.AddCell(new PdfPCell(new Phrase()));

            recTable.AddCell(new PdfPCell(new Phrase(new Chunk("MOTIVO: " +  motivo, new nFont("Verdana", 8, true, Color.BLACK)))));
            //recTable.AddCell(new PdfPCell(new Phrase()));

            recTable.AddCell(new PdfPCell(new Phrase(new Chunk("NOTA FISCAL DE ORIGEM: " + (proc.NF_FOX.HasValue ? proc.NF_FOX.ToString() : "" ), new nFont("Verdana", 8, true, Color.BLACK)))));
           // recTable.AddCell(new PdfPCell(new Phrase()));

            recTable.AddCell(new PdfPCell(new Phrase(new Chunk("PROCEDIMENTO: " + proc.CD_PROCEDIMENTO.ToString(), new nFont("Verdana", 8, true, Color.BLACK)))));
            //recTable.AddCell(new PdfPCell(new Phrase()));

            //recTable.AddCell(new PdfPCell(new Phrase(new Chunk("Total", new nFont())))
            //{ MinimumHeight = 15, BackgroundColor = Color.GRAY, PaddingTop = 5, HorizontalAlignment = PdfCell.ALIGN_CENTER });

            //recTable.AddCell(new PdfPCell(new Phrase(new Chunk(valor_total, new nFont()))) { MinimumHeight = 15, BackgroundColor = Color.GRAY, PaddingTop = 5, HorizontalAlignment = PdfCell.ALIGN_CENTER });



            document.Add(recTable);


            document.Add(new Paragraph(" "));


            recTable = new nTable().GetNewTable(3, document.PageSize.Width - 10, (new float[] { 40f, 30f, 30f }));

            recTable.AddCell(new PdfPCell(new Phrase()) { Border = 0, BorderWidthLeft = 0, BorderWidthTop = 0, BorderWidthBottom = 0 });
            recTable.AddCell(new PdfPCell(new Phrase(new Chunk("VALOR TOTAL", new nFont("Verdana", 10, true, Color.BLACK)))) { MinimumHeight = 15 });
            recTable.AddCell(new PdfPCell(new Phrase(new Chunk(valor_total, new nFont("Verdana", 10, true, Color.BLACK)))));

            recTable.AddCell(new PdfPCell(new Phrase()) { Border = 0, BorderWidthLeft = 0, BorderWidthTop = 0, BorderWidthBottom = 0 });
            recTable.AddCell(new PdfPCell(new Phrase(new Chunk("VENCIMENTO", new nFont("Verdana", 10, true, Color.BLACK)))) { MinimumHeight = 15 });
            recTable.AddCell(new PdfPCell(new Phrase(new Chunk(dta_vencimento.Value.ToShortDateString(), new nFont("Verdana", 10, true, Color.BLACK)))));

            document.Add(recTable);

            document.Add(new Paragraph(" "));


            recTable = new nTable().GetNewTable(1, document.PageSize.Width - 10, ( new float[] { 1f }));
            
            recTable.AddCell(new PdfPCell(new Phrase(new Chunk("OBS.: Informamos que os boletos estão "+ 
                " registrados no banco, sendo assim, o pagamento deverá "+
                "ser através do mesmo.", new nFont("Verdana", 8, true, Color.BLACK)))) { MinimumHeight = 15 });
            document.Add(recTable);


            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return new FileStreamResult(workStream, "application/pdf");


        }






    }
}
