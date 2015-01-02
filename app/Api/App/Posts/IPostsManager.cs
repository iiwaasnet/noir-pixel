namespace Api.App.Posts
{
    public interface IPostsManager
    {
        WallPosts GetUserWallPosts(string userName, int skip, int limit);
    }
}