using System.Linq;
using System.Web.Mvc;
using Services.Functions;
using Data.Context;

namespace b2yweb_mvc4.Controllers
{
    
    public class HomeController : Controller
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

        //
        // GET: /Home/
        [AuthFilter]
        public ActionResult Index()
        {
            var grafico = db.Grafico1.ToList();
            ViewData["TOTALPROCEDIMENTOS"] = (from a in db.Grafico1 select a.QTDETOTAL).Sum();
            ViewData["TOTALAPROVADOS"] = (from a in db.Grafico1 select a.QTDEAPROVADA).Sum();
            ViewData["TOTALREPROVADOS"] = (from a in db.Grafico1 select a.QTDEREPROVADA).Sum();
            ViewData["TOTALATIVAS"] = (from a in db.Grafico1 select a.QTDEATIVAS).Sum();

            var grafico2 = db.Grafico2.ToList();
            
            ViewData["TOTALPROCEDIMENTOS2"] = (from a in db.Grafico2 select a.QTDETOTAL).Sum();

            ViewData["TOTALAPROVADOS2"] = db.Grafico2.Where(a => a.ID_SITUACAO == 2).Sum(a => (int?)a.QTDETOTAL) ?? 0;
            ViewData["TOTALREPROVADOS2"] = db.Grafico2.Where(a => a.ID_SITUACAO == 3).Sum(a => (int?)a.QTDETOTAL) ?? 0;
            ViewData["TOTALATIVAS2"] = db.Grafico2.Where(a => a.ID_SITUACAO == 1).Sum(a => (int?)a.QTDETOTAL) ?? 0;
            ViewData["TOTALCANCELADA"] = db.Grafico2.Where(a => a.ID_SITUACAO == 4).Sum(a => (int?)a.QTDETOTAL) ?? 0;



            return View(grafico.Where(c => c.QTDETOTAL > 0).OrderByDescending(a => a.QTDETOTAL) );
        }


        [HttpPost]
        public ActionResult _Grafico1()
        {
            return Json(db.Grafico1.Where(c => c.QTDETOTAL > 0).OrderByDescending(a => a.QTDETOTAL).ToList());
        }


        [HttpPost]
        public ActionResult _Grafico2()
        {
            return Json(db.Grafico2.OrderByDescending(a => a.QTDETOTAL).ToList());
        }



        [HttpPost]
        public ActionResult _Grafico3()
        {
            return Json(db.Grafico3.OrderByDescending(a => a.HORASPARADAS).ToList());
        }

    }
}
