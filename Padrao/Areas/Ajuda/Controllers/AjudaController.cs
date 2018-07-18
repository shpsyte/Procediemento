using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Services.Functions;


namespace b2yweb_mvc4.Areas.Ajuda.Controllers
{
    [AuthFilter]
	public class AjudaController : Controller
    {
        //
        // GET: /Ajuda/Ajuda/

        public ActionResult Index()
        {
            return View();
        }

    }
}
