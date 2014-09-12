using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Diagnostics;
using Web.Components.Localization;

namespace Web.Controllers
{
    public class StringsController : Controller
    {
        private readonly ILocalizedStrings localizedStrings;
        private readonly ILogger logger;

        public StringsController(ILocalizedStrings localizedStrings, ILogger logger)
        {
            this.logger = logger;
            this.localizedStrings = localizedStrings;
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