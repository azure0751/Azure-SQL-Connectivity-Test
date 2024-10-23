namespace AzureSqlConnectionDemo
{
    public class SqlConnectionModel
    {
        public string ConnectionString { get; set; }
        public string ServerAddress { get; set; } // Server IP/hostname for the ping test
        public string Message { get; set; }
        public string PingTestMessage { get; set; } // Ping test result
    }
}
