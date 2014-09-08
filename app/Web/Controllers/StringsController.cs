using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Diagnostics;
using JsonConfigurationProvider;
using Web.Components.Configuration;
using Web.Components.Localization;

namespace Web.Controllers
{
    public class StringsController : Controller
    {
        private readonly ILocalizedStrings localizedStrings;
        private readonly ILogger logger;
        private readonly StringsCacheConfiguration config;

        public StringsController(ILocalizedStrings localizedStrings, IConfigProvider configProvider, ILogger logger)
        {
            this.logger = logger;
            this.localizedStrings = localizedStrings;
            config = configProvider.GetConfiguration<StringsCacheConfiguration>();
        }

        [HttpGet]
        public ActionResult All()
        {
            return new CustomJsonResult
                   {
                       Data = GetStringsForAllLocales().ToArray()
                   };
        }

        [HttpGet]
        public ActionResult Localized(string lang)
        {
            object strings;
            try
            {
                strings = localizedStrings.GetLocalizedCollection(lang);
            }
            catch (Exception err)
            {
                logger.Error(err);

                strings = localizedStrings.GetDefaultCollection();
            }

            return new CustomJsonResult {Data = strings};
        }

        [HttpGet]
        public ActionResult Versions()
        {
            return new CustomJsonResult
                   {
                       Data = new
                              {
                                  Versions = localizedStrings.GetSupportedLocales()
                                                             .Select(locale => new {Locale = locale, Version = "1"})
                                                             .ToArray()
                              }
                   };
        }

        [HttpGet]
        public ActionResult Config()
        {
            return new CustomJsonResult
                   {
                       Data = new
                              {
                                  InvalidationTimeout = config.InvalidationTimeout.TotalMilliseconds
                              }
                   };
        }

        private IEnumerable<LocalizedStringCollection> GetStringsForAllLocales()
        {
            var allStrings = new List<LocalizedStringCollection>();

            foreach (var locale in localizedStrings.GetSupportedLocales())
            {
                try
                {
                    allStrings.Add(localizedStrings.GetLocalizedCollection(locale));
                }
                catch (Exception err)
                {
                    logger.Error(err);
                }
            }

            return allStrings;
        }
    }
}