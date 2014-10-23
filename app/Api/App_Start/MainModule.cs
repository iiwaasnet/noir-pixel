using System.Reflection;
using Api.Models;
using AspNet.Identity.MongoDB;
using Autofac;
using Autofac.Integration.WebApi;
using Diagnostics;
using Microsoft.AspNet.Identity;
using Module = Autofac.Module;

namespace Api
{
    public class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<Logger>()
                   .As<ILogger>()
                   .SingleInstance();

            builder.RegisterType<ApplicationUserManager>()
                   .AsSelf()
                   .SingleInstance();
            builder.Register(c => new UserStore<ApplicationUser>(
                                      c.Resolve<ApplicationIdentityContext>()
                                      ))
                   .As<IUserStore<ApplicationUser>>().SingleInstance();
            builder.Register(c => ApplicationIdentityContext.Create())
                   .As<ApplicationIdentityContext>()
                   .SingleInstance();
            builder.RegisterType<ApplicationRoleManager>().AsSelf().SingleInstance();
            builder.Register(c => new RoleStore<IdentityRole>(c.Resolve<ApplicationIdentityContext>()))
                   .As<IRoleStore<IdentityRole, string>>()
                   .SingleInstance();
        }
    }
}