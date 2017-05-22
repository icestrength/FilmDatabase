using System.Web;
using System.Web.Optimization;

namespace FilmDatabase
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
                      "~/Scripts/respond.js",
                      "~/Scripts/slick.min.js",
                      "~/Scripts/.main.js"));






            bundles.Add(new StyleBundle("~/Content/css").Include(
                                    "~/Content/bootstrap.css"
                    //  " ~/Content/slick-theme.css",
                     // " ~/Content/slick.css",
                     // " ~/Content/sliderStyle.css"
                     ));
            bundles.Add(new StyleBundle("~/Content/css1").Include(
                                   "~/Content/Site.css"
                     //  " ~/Content/slick-theme.css",
                     // " ~/Content/slick.css",
                     // " ~/Content/sliderStyle.css"
                     ));
            //bundles.Add(new StyleBundle("~/Content/style").Include(
            //     " ~/Content/slick-theme.css",
            //         " ~/Content/slick.css",
            //          " ~/Content/sliderStyle.css"
            //    ));


            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));


        }
    }
}
