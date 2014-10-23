using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;

namespace Api
{
    public static class DependencyInjection
    {
        private static readonly IContainer container;

        static DependencyInjection()
        {
            var builder = new ContainerBuilder();
            var thisAssembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyModules(thisAssembly);
            builder.RegisterControllers(thisAssembly);
            builder.RegisterApiControllers(thisAssembly);
            container = builder.Build();
        }

        public static IContainer GetContainer()
        {
            return container;
        }
    }
}