using System.Linq;
using System.Web.Mvc;
using JsonConfigurationProvider;

namespace Web.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly IConfigProvider configProvider;

        public ConfigurationController(IConfigProvider configProvider)
        {
            this.configProvider = configProvider;
        }

        public ActionResult All()
        {
            var configurations = configProvider.GetAllUntypedConfigurations();

            return new CustomJsonResult {Data = configurations.ToArray()};
        }
    }
}