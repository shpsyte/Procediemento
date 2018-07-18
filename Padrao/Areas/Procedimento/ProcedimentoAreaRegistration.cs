using System.Web.Mvc;

namespace b2yweb_mvc4.Areas.Procedimento
{
    public class ProcedimentoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Procedimento";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Procedimento_default",
                "Procedimento/{controller}/{action}/{id}",
                new { controller = "Procedimento", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
