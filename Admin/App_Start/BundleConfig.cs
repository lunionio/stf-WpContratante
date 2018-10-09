using System.Web;
using System.Web.Optimization;

namespace Admin
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            /*
                                <script src="~/Scripts/jquery.3.2.1.min.js"></script>
                                <script src="~/Scripts/light-bootstrap-dashboard.js"></script>
                                <script src=""></script>
                                <script src=""></script>
                                <script src="~/Scripts/Core/popper.min.js"></script>
                                <script src="~/Scripts/Core/bootstrap.min.js"></script>
                                <script type="text/javascript" src="~/Scripts/jquery.loading.js"></script>
                                <script src=""></script>
                                <script src=""></script>*@
*/

            bundles.Add(new ScriptBundle("~/bundles/script").Include(
                  "~/Scripts/jquery.3.2.1.min.js",
                  "~/Scripts/light-bootstrap-dashboard.js",
                  "~/Scripts/popper.min.js",
                  "~/Scripts/bootstrap.min.js",
                  "~/Scripts/jquery.loading.js",
                  "~/Scripts/perfect-scrollbar.jquery.min.js",
                  "~/Scripts/jquery.datatables.js",
                  "~/Scripts/bootstrap-notify.js",
                  "~/Scripts/demo.js",
                  "~/Scripts/Plugins/jquery.mask.js",
                  "~/Scripts/Plugins/bootstrap-selectpicker.js"));




            bundles.Add(new StyleBundle("~/bundles/css").Include(
                        "~/Content/bootstrap.min.css",
                        "~/Content/light-bootstrap-dashboard.css",
                        "~/Content/pe-icon-7-stroke.css",
                        "~/Content/Custom.css",
                        "~/Content/jquery.loading.css"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
