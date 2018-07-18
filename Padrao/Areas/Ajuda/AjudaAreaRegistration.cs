using System.Web.Mvc;

namespace b2yweb_mvc4.Areas.Ajuda
{
    public class AjudaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Ajuda";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Ajuda_default",
                "Ajuda/{controller}/{action}/{id}",
                new { controller = "Ajuda", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
