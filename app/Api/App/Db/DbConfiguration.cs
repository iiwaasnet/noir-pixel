namespace Api.App.Db
{
    public class DbConfiguration
    {
        public string Server { get; set; }
        public IdentityDbConfiguration Identity { get; set; }
    }
}