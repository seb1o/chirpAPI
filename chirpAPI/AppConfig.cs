namespace chirpAPI
{
    internal class AppConfig
    {
        public static IConfiguration Configuration { get; private set; }
        static AppConfig()
        {
            if (Configuration == null)
            {
                Configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("jsconfig.json", optional: false, reloadOnChange: true)
                    .Build();
            }
        }

        public static string? GetConnectionString()
        {
            return Configuration.GetConnectionString("Default");
        }
    }
}