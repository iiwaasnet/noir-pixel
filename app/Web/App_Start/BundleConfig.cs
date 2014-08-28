using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var angular = new ScriptBundle("~/bundles/angular")
                .Include("~/app/vendor/angular.min.js")
                .Include("~/app/vendor/angular-route.min.js");
            angular.Transforms.Clear();
            bundles.Add(angular);

            var vendor = new ScriptBundle("~/bundles/vendor")
                .Include("~/app/vendor/angular-local-storage.js");
            bundles.Add(vendor);
            //var vendorMin = new ScriptBundle("~/bundles/vendor-min")
            //    .Include("~/app/vendor/*.min.js");
            //vendorMin.Transforms.Clear();
            //bundles.Add(vendorMin);

            bundles.Add(new ScriptBundle("~/bundles/app").IncludeDirectory("~/app/src", "*.js", true));

#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}