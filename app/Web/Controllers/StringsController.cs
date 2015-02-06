using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Diagnostics;
using Web.Components.Localization;

namespace Web.Controllers
{
    [RoutePrefix("strings")]
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
        [Route("all")]
        public ActionResult All()
        {
            return new CamelCaseJsonResult
                   {
                       Data = GetStringsForAllLocales().ToArray()
                   };
        }

        [HttpGet]
        [Route("localized/{lang}")]
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

            return new CamelCaseJsonResult {Data = strings};
        }

        [HttpGet]
        [Route("versions")]
        public ActionResult Versions()
        {
            return new CamelCaseJsonResult
                   {
                       Data = new
                              {
                                  Versions = localizedStrings
                           .GetSupportedLocales()
                           .Select(locale => new
                                             {
                                                 Locale = locale,
                                                 Version = localizedStrings.GetCurrentVersion()
                                             })
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