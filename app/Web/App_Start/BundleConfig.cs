﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #if DEBUG
            var angular = new Bundle("~/bundles/angular")
                .Include("~/app/vendor/angular.js");
#else
            var angular = new Bundle("~/bundles/angular")
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

#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }

        private static IEnumerable<string> AuthAssets()
        {
            yield return "~/app/src/auth/auth.module.js";
            yield return "~/app/src/auth/auth.service.js";
            yield return "~/app/src/auth/signIn.controller.js";
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
            yield return "~/app/src/interceptors/http-interceptor.js";
        }

        private static IEnumerable<string> MainAppAssets()
        {
            yield return "~/app/src/app/np.js";
            yield return "~/app/src/app/np.routes.js";
        }

        private static IEnumerable<string> LocalizationAssets()
        {
            yield return "~/app/src/localization/i18n.module.js";
            yield return "~/app/src/localization/strings.service.js";
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