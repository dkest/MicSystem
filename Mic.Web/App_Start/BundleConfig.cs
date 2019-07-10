using System.Web;
using System.Web.Optimization;

namespace Mic.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = false;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/js/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/js/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/minejs").Include(
                      "~/Content/js/service.js"));

            bundles.Add(new ScriptBundle("~/bundles/layui").Include(
                      "~/Content/layui/layui.all.js"));

            bundles.Add(new StyleBundle("~/Content/mycss").Include(
                      "~/Content/css/mycss.css"));
            bundles.Add(new StyleBundle("~/Content/layui").Include(
                      "~/Content/layui/css/layui.css"));
        }
    }
}
