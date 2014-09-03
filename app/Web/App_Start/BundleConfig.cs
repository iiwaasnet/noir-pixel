using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var angular = new Bundle("~/bundles/angular")
                .Include("~/app/vendor/angular.min.js")
                .Include("~/app/vendor/angular-route.min.js");
            angular.Transforms.Clear();
            bundles.Add(angular);

            var vendorNative = new ScriptBundle("~/bundles/vendor-native")
                .Include("~/app/vendor/stacktrace.js");
            bundles.Add(vendorNative);

            var vendorNg = new ScriptBundle("~/bundles/vendor-ng")
                .Include("~/app/vendor/angular-local-storage.js");
            bundles.Add(vendorNg);
            //var vendorMin = new ScriptBundle("~/bundles/vendor-min")
            //    .Include("~/app/vendor/*.min.js");
            //vendorMin.Transforms.Clear();
            //bundles.Add(vendorMin);

            bundles.Add(new ScriptBundle("~/bundles/app")
                            .IncludeDirectory("~/app/src/Utils", "*.js", true)
                            .IncludeDirectory("~/app/src/Logging", "*.js", true)
                            .Include("~/app/src/npApp.js")
                            .Include("~/app/src/HttpProviderInterceptors.js")
                            .Include("~/app/src/Routes.js")
                            .IncludeDirectory("~/app/src/Localization", "*.js", true)
                            .IncludeDirectory("~/app/src/Layout", "*.js", true)
                            .IncludeDirectory("~/app/src/Home", "*.js", true)
                            .IncludeDirectory("~/app/src/List", "*.js", true)
                );

#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}