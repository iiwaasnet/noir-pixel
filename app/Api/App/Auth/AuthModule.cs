using Api.App.Auth.ExternalUserInfo;
using Api.App.Auth.ExternalUserInfo.Facebook;
using Api.App.Auth.ExternalUserInfo.GPlus;
using Api.App.Auth.ExternalUserInfo.Twitter;
using Api.App.Db;
using AspNet.Identity.MongoDB;
using Autofac;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Twitter;
using Owin.Security.Providers.GooglePlus;

namespace Api.App.Auth
{
    public class AuthModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthOptions>()
                   .AsSelf()
                   .SingleInstance();
            builder.RegisterType<ApplicationUserManager>()
                   .AsSelf()
                   .SingleInstance();
            builder.Register(c => new UserStore<ApplicationUser>(
                                      c.Resolve<ApplicationIdentityContext>()))
                   .As<IUserStore<ApplicationUser>>()
                   .SingleInstance();
            builder.Register(c => ApplicationIdentityContext.Create(c.Resolve<IIdentityDbProvider>()))
                   .As<ApplicationIdentityContext>()
                   .SingleInstance();
            builder.RegisterType<ApplicationRoleManager>()
                   .AsSelf()
                   .SingleInstance();
            builder.Register(c => new RoleStore<IdentityRole>(c.Resolve<ApplicationIdentityContext>()))
                   .As<IRoleStore<IdentityRole, string>>()
                   .SingleInstance();

            builder.RegisterType<GooglePlusAccountProvider>()
                   .As<ISocialAccountProvider>()
                   .SingleInstance();
            builder.RegisterType<FacebookAccountProvider>()
                   .As<ISocialAccountProvider>()
                   .SingleInstance();
            builder.RegisterType<TwitterAccountProvider>()
                   .As<ISocialAccountProvider>()
                   .SingleInstance();

            builder.RegisterType<ExternalAccountsManager>()
                   .As<IExternalAccountsManager>()
                   .SingleInstance();
            builder.Register(c => c.Resolve<AuthOptions>().GooglePlusAuthOptions)
                   .As<GooglePlusAuthenticationOptions>()
                   .SingleInstance();
            builder.Register(c => c.Resolve<AuthOptions>().FacebookAuthOptions)
                   .As<FacebookAuthenticationOptions>()
                   .SingleInstance();
            builder.Register(c => c.Resolve<AuthOptions>().TwitterAuthOptions)
                   .As<TwitterAuthenticationOptions>()
                   .SingleInstance();
        }
    }
}