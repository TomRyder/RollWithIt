using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RollWithIt.Infrastructure
{
    public static class Upgrades
    {
        private static List<Upgrade> _upgrades = new List<Upgrade>
        {
            new Upgrade("FEAE35F9-4424-4519-880C-90F51DD385DF", "createUpgradeLogTable", 0, "CREATE TABLE [upgradeLog]([Id] [uniqueidentifier] NOT NULL,[UpgradeName] [nvarchar](80) NOT NULL,[Order] [int] NOT NULL)")
        };

        public static IOrderedEnumerable<Upgrade> GetFilteredOrderedUpgrades()
        {
            List<string> completedUpgrades = GetCompletedUpgradeNames();
            return _upgrades.Where(upgrade => !completedUpgrades.Contains(upgrade.Id.ToString())).OrderBy(upgrade => upgrade.Order);
        }

        public static IOrderedEnumerable<Upgrade> GetOrderedUpgrades()
        {
            return _upgrades.OrderBy(upgrade => upgrade.Order);
        }

        public static void LogUpgrade(SqlConnection connection, Upgrade upgrade)
        {
            using (SqlCommand logUpgradeCommand = connection.CreateCommand())
            {
                logUpgradeCommand.CommandText = $"INSERT INTO [upgradeLog] VALUES ({upgrade.Id}, {upgrade.Name}, {upgrade.Order})";
                logUpgradeCommand.ExecuteNonQuery();
            }
        }

        private static List<string> GetCompletedUpgradeNames(SqlConnection connection)
        {
            string queryString =
                "SELECT OrderID, CustomerID FROM dbo.Orders;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command =
                    new SqlCommand(queryString, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data.
                while (reader.Read())
                {
                    ReadSingleRow((IDataRecord) reader);
                }

                // Call Close when done reading.
                reader.Close();
            }

            //read the list of completed upgrades
            return new List<string>();
        }
    }
}
