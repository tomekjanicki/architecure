namespace Architecture.Web.Code
{
    public static class Const
    {
        public const string BundlesString = "~/bundles";
        public const string BundlesApplicationString = BundlesString + "/application";
        public const string BundlesApplicationViewString = BundlesApplicationString + "/view";
        public const string BundlesApplicationViewModelString = BundlesApplicationString + "/viewmodel";

        public const string BundlesJqueryString = BundlesString + "/jquery";
        public const string BundlesKnockoutString = BundlesString + "/knockout";
        public const string BundlesBlockUiString = BundlesString + "/blockUI";
        public const string BundlesKnockstrapString = BundlesString + "/knockstrap";
        public const string BundlesQunitString = BundlesString + "/qunit";
        public const string BundlesBootstapString = BundlesString + "/bootstrap";
        public const string BundlesModernizrString = BundlesString + "/modernizr";
        public const string BundlesLinqString = BundlesString + "/linq";
        public const string BundlesNumeralString = BundlesString + "/numeral";
        public const string BundlesMomentString = BundlesString + "/moment";
        
        public const string ContentString = "~/content";
        public const string ContentCssString = ContentString + "/css";
        public const string ContentQunitString = ContentString + "/qunit";

        public const string DefaultApiNameString = "DefaultApi";
        public const string DefaultApiString = @"api";
        public const string DefaultApiRouteTemplateString = DefaultApiString + @"/{controller}/{action}/{id}";

        public const string DefaultRouteTemplateString = @"{controller}/{action}/{id}";
        public const string DefaultNameString = "Default";
    }
}