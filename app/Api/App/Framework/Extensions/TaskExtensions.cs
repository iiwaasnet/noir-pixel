using System.Threading.Tasks;
using Api.App.Exceptions;

namespace Api.App.Framework.Extensions
{
    public static class TaskExtensions
    {
        public static async Task EnsureNotNullAsync<T>(this Task<T> task)
            where T : class
        {
            var result = await task;

            if (result == null)
            {
                throw new NotFoundException();
            }
        }
    }
}