using System;
using System.Collections.Generic;
using System.Linq;
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

            bundles.Add(new ScriptBundle("~/bundles/vendor-native")
                            .Include("~/app/vendor/stacktrace.js")
                            .Include("~/app/vendor/moment.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/vendor-ng")
                            .Include("~/app/vendor/angular-local-storage.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/app")
                            .Include(ConfigAssets().ToArray())
                            .Include(UtilsAssets().ToArray())
                            .Include(LoggingAssets().ToArray())
                            .Include("~/app/src/App/np.js")
                            .IncludeDirectory("~/app/src/interceptors", "*.js", true)
                            .Include("~/app/src/App/Routes.js")
                            .IncludeDirectory("~/app/src/localization", "*.js", true)
                            .IncludeDirectory("~/app/src/layout", "*.js", true)
                            .IncludeDirectory("~/app/src/home", "*.js", true)
                            .IncludeDirectory("~/app/src/list", "*.js", true)
                );

#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }

        private static IEnumerable<string> ConfigAssets()
        {
            yield return "~/app/src/config/config.module.js";
            yield return "~/app/src/config/config.js";
        }

        private static IEnumerable<string> LoggingAssets()
        {
            yield return "~/app/src/logging/logging.module.js";
            yield return "~/app/src/logging/trace.factory.js";
            yield return "~/app/src/logging/application-logging.factory.js";
            yield return "~/app/src/logging/exception-logging.factory.js";
            yield return "~/app/src/logging/exception-handler.js";
        }

        private static IEnumerable<String> UtilsAssets()
        {
            yield return "~/app/src/utils/utils.module.js";
            yield return "~/app/src/utils/moment.factory.js";
            yield return "~/app/src/utils/url.service.js";
        }
    }
}