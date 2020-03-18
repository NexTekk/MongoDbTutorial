namespace Tutorial.Data
{
    public class DbSettings : IDbSettings
    {
        public string DbServer { get; set; }
        public string DbName { get; set; }
    }

    public interface IDbSettings
    {
        string DbName { get; }
        string DbServer { get; }
    }
}
