using AuthService.enums;

namespace AuthService.Model
{
    public class DatabaseConfiguration
    {
        public string ConnectionString { get; set; }
        public ExecutionEnvironment Environment { get; set; }
        public bool EnableSensitiveDataLogging { get; set; }
        public bool PreventCommitInTesting { get; set; } = true;
    }
}
