using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace b2yweb_mvc4.Extends
{
    public class PDFHeaderNDVersaoNova : PdfPageEventHelper
    {

        Int32 Procedimento  = 0;
        bool Canhoto = false;
        Int32 NotaFiscal = 0;
        
        public PDFHeaderNDVersaoNova(int procedimento, int notafiscal, bool canhoto)
        {
            this.Procedimento = procedimento;
            this.Canhoto = canhoto;
            this.NotaFiscal = notafiscal;
        }

        // write on top of document
        public override void OnOpenDocument(PdfWriter writer, Document doc)
        {
            Font verdana = FontFactory.GetFont("Verdana", 12, Font.BOLD, Color.BLACK);
            Font font = FontFactory.GetFont("Arial", 9, Font.NORMAL, Color.BLUE);
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Font times = new Font(bfTimes, 8, Font.NORMAL, Color.BLACK);

            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/Images/Logo_P.gif"));
            img.ScalePercent(53);

            iTextSharp.text.Image imgudmais = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/Images/UdMais.png"));
            imgudmais.ScalePercent(40);

            //tbl 
            PdfPTable HeaderTbl = new PdfPTable(2) ;
            HeaderTbl.TotalWidth = doc.PageSize.Width - 10;
            HeaderTbl.LockedWidth = true;
            HeaderTbl.DefaultCell.BorderWidth = 1;
            HeaderTbl.DefaultCell.Border = 1;
            HeaderTbl.DefaultCell.BorderColor = Color.GRAY;
            HeaderTbl.DefaultCell.BorderWidthLeft = 1;
            HeaderTbl.DefaultCell.BorderWidthRight = 1;
            HeaderTbl.DefaultCell.BorderWidthBottom = 1;
            HeaderTbl.DefaultCell.BorderWidthTop = 1;
            HeaderTbl.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
            HeaderTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            HeaderTbl.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
            HeaderTbl.SetWidths(new float[] { 1f, 1f });




            Paragraph _TITULO = new Paragraph();
            _TITULO.Alignment = Element.ALIGN_CENTER;
            _TITULO.Add(new Chunk(Procedimento.ToString(), verdana));

            PdfPCell celula = new PdfPCell();

            //logo
            HeaderTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            

            HeaderTbl.AddCell(new PdfPCell(img) { MinimumHeight = 50, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER, PaddingTop = 5 });
            HeaderTbl.AddCell(new PdfPCell(new Phrase(new Chunk("NOTA DE DÉBITO" + Environment.NewLine + "Nº: " + NotaFiscal.ToString(), verdana))) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER, PaddingTop = 5 }) ;
            //HeaderTbl.AddCell(new PdfPCell(imgudmais) {  HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER, PaddingTop = 5 });

            //celula = new PdfPCell(new Phrase(new Chunk("N.:" + NotaFiscal.ToString(), verdana))) { Rowspan = 1, Colspan = 2};
            HeaderTbl.AddCell(celula);


            //this is for the position of the footer ... im my case is "+80"
            doc.Add(HeaderTbl);



        }

        // write on start of each page
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
        }

        //write on close of document
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }


        // write on end of each page
        public override void OnEndPage(PdfWriter writer, Document doc)
        {
            

        }

    }
}