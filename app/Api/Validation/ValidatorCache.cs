using System;
using System.Collections.Concurrent;

namespace Api.Validation
{
    public class ValidatorCache
    {
        private readonly ConcurrentDictionary<Type, object> cache;

        public ValidatorCache()
        {
            cache = new ConcurrentDictionary<Type, object>();
        }

        /// <summary>
        ///     Gets or creates an instance using Activator.CreateInstance
        /// </summary>
        /// <param name="type">The type to instantiate</param>
        /// <returns>The instantiated object</returns>
        public object GetOrCreateInstance(Type type)
        {
            return GetOrCreateInstance(type, Activator.CreateInstance);
        }

        /// <summary>
        ///     Gets or creates an instance using a custom factory
        /// </summary>
        /// <param name="type">The type to instantiate</param>
        /// <param name="factory">The custom factory</param>
        /// <returns>The instantiated object</returns>
        public object GetOrCreateInstance(Type type, Func<Type, object> factory)
        {
            object existingInstance;

            if (!cache.TryGetValue(type, out existingInstance))
            {
                existingInstance = factory(type);
                cache[type] = existingInstance;
            }

            return existingInstance;
        }
    }
}