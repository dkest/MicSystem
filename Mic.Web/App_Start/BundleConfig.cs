﻿using System.Web;
using System.Web.Optimization;

namespace Mic.Web
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/js/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/js/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/minejs").Include(
                      "~/Content/js/service.js"));

            bundles.Add(new ScriptBundle("~/bundles/layui").Include(
                      "~/Content/layui/layui.js"));

            bundles.Add(new StyleBundle("~/Content/mycss").Include(
                      "~/Content/css/mycss.css"));
            bundles.Add(new StyleBundle("~/Content/layui").Include(
                      "~/Content/layui/css/layui.css"));
        }
    }
}
