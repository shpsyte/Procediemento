using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Services.Functions;

namespace b2yweb_mvc4.Areas.Procedimento.Controllers
{
    [AuthFilter]
	public class ProcedimentoController : Controller
    {
        //
        // GET: /Procedimento/Procedimento/

        public ActionResult Index()
        {
            return View();
        }

    }
}
