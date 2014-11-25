namespace Api.App.Db
{
    public class DbServerConfiguration
    {
        public string Server { get; set; }
        public DbConfiguration Identity { get; set; }
        public DbConfiguration Application { get; set; }
    }
}