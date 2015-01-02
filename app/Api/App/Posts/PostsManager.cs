using System;
using Api.App.Db;

namespace Api.App.Posts
{
    public class PostsManager : IPostsManager
    {
        private readonly IDbProvider db;

        public PostsManager(IAppDbProvider db)
        {
            this.db = db;
        }

        public WallPosts GetUserWallPosts(string userName, int skip, int limit)
        {
            throw new NotImplementedException();
        }
    }
}