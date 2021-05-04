using System.Web;
using System.Web.Optimization;

namespace LoyaltyProgram
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/stylesApp.css"));
            //"~/Content/DataTables/media/css/dataTables.bootstrap4.min.css"

            //my bundles

            bundles.Add(new ScriptBundle("~/bundles/myScripts").Include(
                      "~/Scripts/app/Incidencias.js",
                      "~/Scripts/app/Reportes.js",
                      "~/Scripts/app/Configuration.js",
                      "~/Scripts/app/Recompensas.js"));//

            bundles.Add(new ScriptBundle("~/bundles/confirm").Include(
                      "~/Scripts/app/jquery-confirm.js"));//

            bundles.Add(new StyleBundle("~/Content/confirm").Include(
                "~/Content/jQuery_Confirm/jquery-confirm.css",
                "~/Content/jQuery_Confirm/jquery-confirm.min.css"));//

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                "~/Scripts/moment.min.js"));//

            bundles.Add(new ScriptBundle("~/bundles/tempusdominus_js").Include(
                "~/Scripts/app/tempusdominus_bootstrap4_min.js"));//

            bundles.Add(new StyleBundle("~/Content/tempusdominus_css").Include(
                "~/Content/tempusdominus_bootstrap4.css"));//

            bundles.Add(new StyleBundle("~/Content/font_awesome_min").Include(
                           "~/Content/font-awesome.min.css"));

            bundles.Add(new StyleBundle("~/Content/dataTable_css").Include(
                "~/Content/datatable_bootstrap4_min.css"));

            bundles.Add(new ScriptBundle("~/bundles/dataTable_js").Include(
                "~/Scripts/DataTables/media/js/jquery.dataTables.min.js",
                "~/Scripts/DataTables/media/js/dataTables.bootstrap4.min.js"));
        }
    }
}
