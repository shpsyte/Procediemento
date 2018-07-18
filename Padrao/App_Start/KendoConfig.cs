using System.Web;
using System.Web.Optimization;

namespace b2yweb_mvc4
{
    public class KendoConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegistraKendo(BundleCollection bundles)
        {



           

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                    "~/Scripts/kendo/jquery.min.js",
                    "~/Scripts/kendo/kendo.all.min.js",
                    "~/Scripts/kendo/kendo.aspnetmvc.min.js",
                    "~/Scripts/kendo/kendo.timezones.min.js"));



         bundles.Add(new StyleBundle("~/Content/kendo").Include(
        "~/Content/kendo/kendo.common-bootstrap.min.css",
        "~/Content/kendo/kendo.dataviz.min.css",
        "~/Content/kendo/kendo.bootstrap.min.css"));



            
            // Clear all items from the default ignore list to allow minified CSS and JavaScript files to be included in debug mode
            bundles.IgnoreList.Clear();


            // Add back the default ignore list rules sans the ones which affect minified files and debug mode
            bundles.IgnoreList.Ignore("*.intellisense.js");
            bundles.IgnoreList.Ignore("*-vsdoc.js");
            bundles.IgnoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);


        }
    }
}