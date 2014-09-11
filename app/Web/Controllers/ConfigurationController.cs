using System.Linq;
using System.Web.Mvc;
using JsonConfigurationProvider;
using Web.Components.Configuration;

namespace Web.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly IConfigProvider configProvider;

        public ConfigurationController(IConfigProvider configProvider)
        {
            this.configProvider = configProvider;
            var config = configProvider.GetConfiguration<StringsConfig>();
        }

        [HttpGet]
        public ActionResult All()
        {
            var configurations = configProvider.GetAllUntypedConfigurations();

            return new CustomJsonResult {Data = configurations.ToArray()};
        }
    }
}