﻿using System.Web.Optimization;

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

            bundles.Add(new ScriptBundle("~/bundles/vendor-native")
                            .Include("~/app/vendor/stacktrace.js"));

            bundles.Add(new ScriptBundle("~/bundles/vendor-ng")
                            .Include("~/app/vendor/angular-local-storage.js"));

            bundles.Add(new ScriptBundle("~/bundles/app")
                            .Include("~/app/src/Config/Const.js")
                            .Include("~/app/src/Config/Config.js")
                            .IncludeDirectory("~/app/src/Utils", "*.js", true)
                            .IncludeDirectory("~/app/src/Logging", "*.js", true)
                            .Include("~/app/src/npApp.js")
                            .IncludeDirectory("~/app/src/Interceptors", "*.js", true)
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