using Diagnostics;
using MongoDB.Driver;

namespace Api.App.Db.Extensions
{
    public static class CommandResultExtensions
    {
        public static CommandResult LogCommandResult(this CommandResult res, ILogger logger)
        {
            if (!res.Ok)
            {
                var msg = string.Format("Insert failed! Command: {0}, errorCode: {1}, errorMessage: {2}",
                                        res.CommandName,
                                        res.Code,
                                        res.ErrorMessage);
                logger.Error(msg, res.Response);
            }

            return res;
        }
    }
}