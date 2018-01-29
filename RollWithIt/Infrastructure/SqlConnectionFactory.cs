using System.Data.SqlClient;

namespace RollWithIt.Infrastructure
{
    public static class SqlConnectionFactory
    {
        public static SqlConnection GetSqlConnection()
        {
            return new SqlConnection("Server=tcp:thomasryder.database.windows.net,1433;Initial Catalog=rollwithitdb;Persist Security Info=False;User ID=rydert;Password=12qw!\"QW;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
}
