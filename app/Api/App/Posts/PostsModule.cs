using Autofac;

namespace Api.App.Posts
{
    public class PostsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PostsManager>()
                   .As<IPostsManager>()
                   .SingleInstance();
        }
    }
}