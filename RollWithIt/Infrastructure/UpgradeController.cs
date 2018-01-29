using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace RollWithIt.Infrastructure
{
    [Route("api/[controller]")]
    public class UpgradeController : Controller
    {
        [HttpPut]
        public void Upgrade()
        {
            using (SqlConnection connection = SqlConnectionFactory.GetSqlConnection())
            {
                using (SqlCommand createUpgradeLogTableCommand = connection.CreateCommand())
                {
                    connection.Open();
                    foreach (Upgrade upgrade in Upgrades.GetOrderedUpgrades())
                    {
                        createUpgradeLogTableCommand.CommandText = upgrade.Sql;
                        createUpgradeLogTableCommand.ExecuteNonQuery(); 
                        Upgrades.LogUpgrade(connection, upgrade);
                    }
                }
            }
        }
    }
}
