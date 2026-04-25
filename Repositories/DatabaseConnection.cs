using MySqlConnector;

namespace WindowsFormsApp1.Repositories
{
    public static class DatabaseConnection
    {
        public static string ConnectionString { get; set; } =
           "server=localhost;database=auto_production;user=root;password=1111;port=3306;SslMode=Preferred;AllowPublicKeyRetrieval=True;CharacterSet=utf8mb4;ConvertZeroDateTime=True;AllowZeroDateTime=True;";
    }
}