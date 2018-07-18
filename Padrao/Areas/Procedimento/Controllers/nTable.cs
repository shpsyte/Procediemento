using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace b2yweb_mvc4.Areas.Procedimento.Controllers
{
    public class nTable : PdfPTable
    {

        private int _colunas;
        private float _tamanhoTotal;
        private Color _corBordaCelula;
        private int _tamanhoBordaCelula;
        private int _alinhamentoHorizontal;
        private int _alinhamentoVertical;
        private float[] _tamanhoColunas;


        public PdfPTable GetNewTable(int colunas, float tamanho, float[] widthColunas)
        {
            var recTable = new PdfPTable(colunas);
            recTable.TotalWidth = tamanho;
            recTable.LockedWidth = true;
            recTable.DefaultCell.BorderColor = _corBordaCelula;
            recTable.DefaultCell.Border = _tamanhoBordaCelula;
            recTable.DefaultCell.HorizontalAlignment = _alinhamentoHorizontal;
            recTable.DefaultCell.VerticalAlignment = _alinhamentoVertical;
            recTable.SetWidths(widthColunas);
            return recTable;
        }


        public nTable(int colunas, float tamanhototal, Color corBordaCelula, int tamanhoBordaCelula, int alinhamentoHorizontal, int alinhamentoVertical, float[] tamanhoColunas)
        {
            this._colunas = colunas;
            this._tamanhoTotal = tamanhototal;
            this._corBordaCelula = corBordaCelula;
            this._tamanhoBordaCelula = tamanhoBordaCelula;
            this._alinhamentoHorizontal = alinhamentoHorizontal;
            this._alinhamentoVertical = alinhamentoVertical;
            this._tamanhoColunas = tamanhoColunas;
        }

        public nTable(int colunas) 
        {
            this._colunas = colunas;
            this._tamanhoTotal = 50;
            this._corBordaCelula = Color.GRAY;
            this._tamanhoBordaCelula = PdfPCell.BOTTOM_BORDER;
            this._alinhamentoHorizontal = PdfPCell.ALIGN_CENTER;
            this._alinhamentoVertical = PdfPCell.ALIGN_CENTER;
            this._tamanhoColunas = new float[] { 40f, 30f, 30f };
        }

        public nTable()
        {
            this._colunas = 3;
            this._tamanhoTotal = 50;
            this._corBordaCelula = Color.GRAY;
            this._tamanhoBordaCelula = PdfPCell.BOTTOM_BORDER;
            this._alinhamentoHorizontal = PdfPCell.ALIGN_CENTER;
            this._alinhamentoVertical = PdfPCell.ALIGN_CENTER;
            this._tamanhoColunas = new float[] { 40f, 30f, 30f };
        }



    }
}