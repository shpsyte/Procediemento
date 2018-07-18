using System.Web;
using System.Web.Optimization;

namespace b2yweb_mvc4
{
    public class BootStrapConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegistraBootStrap(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/Scripts/Bootstrap").Include(
            
             "~/Scripts/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/Bootstrap").Include(
                "~/Content/bootstrap.min.css"));

        
        }
    }
}