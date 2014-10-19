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
            BundleJs(bundles);
            BundleCss(bundles);

#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }

        private static void BundleCss(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css")
                .Include("~/app/less/all.css", new CssRewriteUrlTransform()));
        }

        private static void BundleJs(BundleCollection bundles)
        {
#if DEBUG
            var angular = new Bundle("~/bundles/angular")
                .Include("~/app/vendor/angular.js")
                .Include("~/app/vendor/angular-messages.js");
#else
            var angular = new Bundle("~/bundles/angular")
                .Include("~/app/vendor/angular-messages.min.js")
                .Include("~/app/vendor/angular.min.js");
#endif
            angular.Transforms.Clear();
            bundles.Add(angular);

            bundles.Add(new ScriptBundle("~/bundles/vendor-native")
                            .Include("~/app/vendor/stacktrace.js")
                            .Include("~/app/vendor/moment.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/vendor-ng")
                            .Include("~/app/vendor/angular-local-storage.js")
                );

            var vendorNgMin = new Bundle("~/bundles/vendor-ng-min")
                .Include("~/app/vendor/angular-ui-router.min.js")
                .Include("~/app/vendor/bindonce.min.js");
            vendorNgMin.Transforms.Clear();
            bundles.Add(vendorNgMin);

            bundles.Add(new ScriptBundle("~/bundles/app")
                            .Include(RegisterApp().ToArray())
                            .Include(RegisterRoutes().ToArray())
                            .Include(RunApp().ToArray())
                            .Include(ConfigAssets().ToArray())
                            .Include(UtilsAssets().ToArray())
                            .Include(LoggingAssets().ToArray())
                            .Include(MainAppAssets().ToArray())
                            .Include(InterceptorsAssets().ToArray())
                            .Include(LocalizationAssets().ToArray())
                            .Include(HomeAssets().ToArray())
                            .Include(LayoutAssets().ToArray())
                            .Include(AuthAssets().ToArray())
                );
        }

        private static IEnumerable<string> RunApp()
        {
            yield return "~/app/src/app/np.run.js";
        }

        private static IEnumerable<string> RegisterApp()
        {
            yield return "~/app/src/app/np.module.js";
        }

        private static IEnumerable<string> RegisterRoutes()
        {
            yield return "~/app/src/app/np.routes.js";
        }

        private static IEnumerable<string> AuthAssets()
        {
            yield return "~/app/src/auth/auth.module.js";
            yield return "~/app/src/auth/auth.service.js";
            yield return "~/app/src/auth/tokenStorage.service.js";
            yield return "~/app/src/auth/signIn.controller.js";
            yield return "~/app/src/auth/notAuthorized.controller.js";
        }

        private static IEnumerable<string> LayoutAssets()
        {
            yield return "~/app/src/layout/layout.module.js";
            yield return "~/app/src/layout/header/mainMenu.service.js";
            yield return "~/app/src/layout/header/signMenu.service.js";
            yield return "~/app/src/layout/header/header.controller.js";
        }

        private static IEnumerable<string> HomeAssets()
        {
            yield return "~/app/src/home/home.module.js";
            yield return "~/app/src/home/home.controller.js";
        }

        private static IEnumerable<string> InterceptorsAssets()
        {
            yield return "~/app/src/interceptors/httpInterceptor.js";
        }

        private static IEnumerable<string> MainAppAssets()
        {
            yield return "~/app/src/app/np.js";
        }

        private static IEnumerable<string> LocalizationAssets()
        {
            yield return "~/app/src/localization/i18n.module.js";
            yield return "~/app/src/localization/strings.service.js";
            yield return "~/app/src/localization/np-i18n.filter.js";
            yield return "~/app/src/localization/np-i18n.directive.js";
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