using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace b2yweb_mvc4.Extends
{
    public class PDFHeaderND : PdfPageEventHelper
    {

        Int32 Procedimento  = 0;
        bool Canhoto = false;
        Int32 NotaFiscal = 0;
        
        public PDFHeaderND(int procedimento, int notafiscal, bool canhoto)
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

            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/Images/Logo_P.gif"));
            img.ScalePercent(45);

            Paragraph _TITULO = new Paragraph();
            _TITULO.Alignment = Element.ALIGN_CENTER;
            _TITULO.Add(new Chunk(Procedimento.ToString(), verdana));

            //tbl 
            PdfPTable HeaderTbl = new PdfPTable(2);
            HeaderTbl.TotalWidth = doc.PageSize.Width - 10;
            HeaderTbl.LockedWidth = true;
            HeaderTbl.DefaultCell.BorderWidth = 1;
            float[] largura_colunas_coleta = new float[] { 1f, 1f };
            HeaderTbl.SetWidths(largura_colunas_coleta);



            //logo
            HeaderTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell = new PdfPCell(img);
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            HeaderTbl.AddCell(cell);


            //title
            //cell = new PdfPCell(_TITULO);
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell.Border = 0;
            //HeaderTbl.AddCell(cell);


            //Date  print
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Font times = new Font(bfTimes, 8, Font.NORMAL, Color.BLACK);
            Chunk myFooter = new Chunk("NOTA DE DÉBITO - " + NotaFiscal.ToString(), verdana);


            PdfPCell footer = new PdfPCell(new Phrase(myFooter));
            footer.Border = 0;
            footer.HorizontalAlignment = Element.ALIGN_CENTER;
            HeaderTbl.AddCell(footer);

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


            Font font = FontFactory.GetFont("Arial", 9, Font.NORMAL, Color.BLUE);
            //tbl footer
            PdfPTable footerTbl = new PdfPTable(1);
            footerTbl.TotalWidth = doc.PageSize.Width;
            //img footer

            iTextSharp.text.Image foot = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/Images/Logo_P.gif"));
            foot.ScalePercent(45);



            footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell = new PdfPCell(foot);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = 0;
            //footerTbl.AddCell(cell);


            //page number
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Font times = new Font(bfTimes, 8, Font.NORMAL, Color.GRAY);
            Chunk myFooter = new Chunk("Página " + (doc.PageNumber), times);
            PdfPCell footer = new PdfPCell(new Phrase(myFooter));
            footer.Border = Rectangle.NO_BORDER;
            footer.HorizontalAlignment = Element.ALIGN_CENTER;
            footerTbl.AddCell(footer);

            //this is for the position of the footer ... im my case is "+80"
            footerTbl.WriteSelectedRows(0, -1, 0, (doc.BottomMargin + 10), writer.DirectContent);



            if (Canhoto)
            {
                Font smallfont = FontFactory.GetFont("Arial", 6, Color.GRAY);

                PdfPTable finaliza = new PdfPTable(3);
                finaliza.TotalWidth = doc.PageSize.Width - 15; ;
                finaliza.LockedWidth = true;
                finaliza.DefaultCell.BorderWidth = 0;
                finaliza.HorizontalAlignment = 1;

                float[] largura_colunas_finaliza = new float[] { 240f, 100f, 200f };
                finaliza.SetWidths(largura_colunas_finaliza);
                PdfPCell celula = new PdfPCell();

                celula = new PdfPCell(new Paragraph(new Chunk("Local" + Chunk.NEWLINE + Chunk.NEWLINE, smallfont)));
                finaliza.AddCell(celula);

                celula = new PdfPCell(new Paragraph(new Chunk("Data", smallfont)));
                finaliza.AddCell(celula);

                celula = new PdfPCell(new Paragraph(new Chunk("Assinatura", smallfont)));
                finaliza.AddCell(celula);

                finaliza.WriteSelectedRows(0, -1, 9, (doc.BottomMargin + 30), writer.DirectContent);
            }

        }

    }
}