using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Web.Controllers
{
    public static class NameValueCollectionExtensions
    {
        public static IDictionary<string, string> ToDictionary(this NameValueCollection collection)
        {
            return collection.AllKeys.ToDictionary(key => key, collection.Get);
        }
    }
}