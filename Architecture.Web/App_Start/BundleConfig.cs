using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using Architecture.Web.Code;

namespace Architecture.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            SetupScriptBundles(bundles);
            SetupStyleBundles(bundles);
        }

        private static void SetupScriptBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(Const.BundlesJqueryString).Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle(Const.BundlesKnockoutString).Include("~/Scripts/knockout-{version}.js"));
            bundles.Add(new ScriptBundle(Const.BundlesKnockstrapString).Include("~/Scripts/knockstrap.js"));
            bundles.Add(new ScriptBundle(Const.BundlesLinqString).Include("~/Scripts/linq.js"));
            bundles.Add(new ScriptBundle(Const.BundlesNumeralString).Include("~/Scripts/numeral/numeral.js"));
            bundles.Add(new ScriptBundle(Const.BundlesMomentString).Include("~/Scripts/moment.js"));
            bundles.Add(new ScriptBundle(Const.BundlesQunitString).Include("~/Scripts/qunit-{version}.js"));
            bundles.Add(new ScriptBundle(Const.BundlesBootstapString).Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle(Const.BundlesModernizrString).Include("~/Scripts/modernizr-*"));
            AutoSetupScriptBundles(bundles);            
        }

        private static void SetupStyleBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle(Const.ContentCssString).Include("~/Content/bootstrap.css", "~/Content/site.css", "~/Content/font-awesome.css"));
            bundles.Add(new StyleBundle(Const.ContentQunitString).Include("~/Content/qunit-{version}.css"));
        }

        private static void AutoSetupScriptBundles(BundleCollection bundles)
        {
            var l = GetTuples(@"~/Scripts/application").ToList();
            l.ForEach(tuple => bundles.Add(new ScriptBundle(tuple.Item1).Include(tuple.Item2)));
        }

        private static IEnumerable<Tuple<string, string>> GetTuples(string virtualPath)
        {
            var list = GetPaths(virtualPath);
            return list.Select(s => new Tuple<string, string>(
                string.Format(@"{0}{1}", Const.BundlesApplicationString, s.Replace(".js", "").Replace(".", "_")), 
                string.Format(@"{0}{1}", virtualPath, s)));
        }

        private static IEnumerable<string> GetPaths(string virtualPath)
        {
            var path = HttpContext.Current.Server.MapPath(virtualPath);
            var di = new DirectoryInfo(path);
            var files = di.GetFiles("*.ts", SearchOption.AllDirectories);
            return files.Where(info => info.DirectoryName != null).Select(info => string.Format(@"{0}\{1}", info.DirectoryName.Substring(path.Length), info.Name).Replace(@"\", @"/").Replace(".ts", ".js"));
        }
    }
}
