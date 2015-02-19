using System.Collections.Generic;
using System.Web.Mvc;

namespace Architecture.Web.Code
{
    public static class ScriptHelper
    {
        public static string[] GetViewSpecificScripts(ViewContext context)
        {
            var actionName = context.RouteData.Values["action"].ToString().ToLowerInvariant();
            var controllerName = context.RouteData.Values["controller"].ToString().ToLowerInvariant();
            var s1 = string.Format("{0}/{1}", Const.BundlesApplicationViewModelString, controllerName);
            var s2 = string.Format("{0}/{1}_{2}", Const.BundlesApplicationViewString, controllerName, actionName);
            return new[] { s1, s2 };
        }

        public static string[] 
            GetLayoutCommonScripts(bool forTest)
        {
            var data = new List<string>
            {
                Const.BundlesJqueryString,
                Const.BundlesLinqString,
                Const.BundlesNumeralString,
                Const.BundlesMomentString,
                forTest ? Const.BundlesQunitString : Const.BundlesBootstapString,
                Const.BundlesKnockoutString,
                Const.BundlesBlockUiString,
                Const.BundlesKnockstrapString,
                string.Format("{0}/{1}", Const.BundlesApplicationString, "common"),
                string.Format("{0}/{1}", Const.BundlesApplicationString, "gridview")
            };
            if (forTest)
                data.Add(string.Format("{0}/{1}", Const.BundlesApplicationString, "test/helper"));
            return data.ToArray();
        }
    }
}