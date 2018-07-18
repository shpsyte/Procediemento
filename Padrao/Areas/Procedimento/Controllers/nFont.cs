using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace b2yweb_mvc4.Areas.Procedimento.Controllers
{
    public class nFont : Font
    {

        private string _nomeFonte;
        private int _tamanhoFonte;
        private bool _bold;
        private Color _cor;


        public nFont()
        {
            this._nomeFonte = "Verdana";
            this._tamanhoFonte = 8;
            this._bold = false;
            this._cor = Color.BLACK;
        }
        public nFont(string nomeFonte, int tamanhoFonte, bool bold, Color cor)
        {
            this._nomeFonte = nomeFonte;
            this._tamanhoFonte = tamanhoFonte;
            this._bold = bold;
            this._cor = cor;
        }

        public Font GetNewFont()
        {
            return FontFactory.GetFont(_nomeFonte, _tamanhoFonte, _bold? Font.BOLD : Font.NORMAL, _cor);
        }


    }
}