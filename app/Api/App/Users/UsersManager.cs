using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.App.Auth;
using Api.App.Db;
using Api.App.Exceptions;
using Diagnostics;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Api.App.Users
{
    public class UsersManager : IUserManager
    {
        private readonly MongoDatabase db;
        private readonly ApplicationUserManager userManager;
        private readonly ILogger logger;

        public UsersManager(IAppDbProvider appDbProvider, ApplicationUserManager userManager, ILogger logger)
        {
            db = appDbProvider.GetDatabase();
            this.userManager = userManager;
            this.logger = logger;
        }

        public async Task<UserHome> GetUserHome(string userName)
        {
            var users = db.GetCollection<User>(User.CollectionName);
            var user = users.FindOne(Query.EQ("UserName", userName));

            if (user == null)
            {
                var login = await userManager.FindByNameAsync(userName);
                AssertUserIsRegistered(login);

                user = new User
                       {
                           UserName = login.UserName,
                           UserId = login.Id,
                           UserImages = CreateDefaultUserImages()
                       };
                var res = users.Insert(user);
                CheckResult(res);
            }

            return new UserHome
                   {
                       UserName = user.UserName,
                       Thumbnail = GetAvatarThumbnail(user)
                   };
        }

        private IEnumerable<UserImage> CreateDefaultUserImages()
        {
            throw new System.NotImplementedException();
        }

        private UserImage GetAvatarThumbnail(User user)
        {
            if (user.UserImages != null)
            {
                var thumbnail = user.UserImages.FirstOrDefault(i => i.ImageType == UserImageType.Thumbnail);
                if (thumbnail == null)
                {
                    //TODO: Hence images might come from external logins, i.e. Facebook, Google, etc., not all image types could be set
                    //thumbnail = CreateDefaultUserThumbnail();
                }
            }
            throw new NotImplementedException();
        }

        private void CheckResult(WriteConcernResult res)
        {
            if (!res.Ok)
            {
                //TODO: Create an extension method for logging WriteConcernResult responses
                var msg = string.Format("Insert failed! Collection: {0}, errorCode: {1}, errorMessage {2}", User.CollectionName, res.Code, res.ErrorMessage);
                logger.Error(msg);
            }
        }

        private void AssertUserIsRegistered(ApplicationUser login)
        {
            if (login == null)
            {
                throw new NotFoundException();
            }
        }
    }
}