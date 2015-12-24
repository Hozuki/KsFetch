using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using KsFetch.Contracts.V1;
using System.Diagnostics;

namespace KsFetch
{

    public sealed class KsFetchDatabaseClient
    {

        public KsFetchDatabaseClient()
        {
        }

        public async void Open(string fileName)
        {
            if (!IsActive)
            {
                SQLiteConnection.CreateFile(fileName);
                _connection = new SQLiteConnection($"Data Source={fileName}");
                await _connection.OpenAsync();
                _fileName = fileName;
            }
        }

        public void Close()
        {
            if (IsActive)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
                _fileName = null;
            }
        }

        public void BeginTransaction()
        {
            if (IsActive && _transaction == null)
            {
                _transaction = _connection.BeginTransaction();
            }
        }

        public void Commit()
        {
            if (IsActive && _transaction != null)
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public void CreateTable()
        {
            if (IsActive)
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = $@"
DROP TABLE IF EXISTS {TABLE_NAME};
CREATE TABLE {TABLE_NAME} (
    id INTEGER PRIMARY KEY,
    name TEXT NOT NULL,
    lowest_price REAL NOT NULL,
    lowest_person INTEGER NOT NULL,
    hottest_price REAL NOT NULL,
    hottest_person INTEGER NOT NULL,
    most_money_price REAL NOT NULL,
    most_money_person INTEGER NOT NULL,
    total_pledged REAL NOT NULL,
    total_backers INTEGER NOT NULL,
    target_money REAL NOT NULL,
    money_unit TEXT NOT NULL,
    money_usd_rate REAL NOT NULL,
    succeeded INTEGER NOT NULL,
    state TEXT NOT NULL
);";
                    command.ExecuteNonQuery();
                }
            }
        }

        public async void InsertRecord(Project project)
        {
            if (IsActive)
            {
                var connection = _transaction != null ? _transaction.Connection : _connection;

                var id = project.ID;

                double lowestPrice = -1;
                uint lowestPerson = 0;
                double hottestPrice = -1;
                uint hottestPerson = 0;
                double mostMoneyPrice = -1;
                uint mostMoneyPerson = 0;
                double totalPledged;
                uint totalBackers;
                double targetMoney;
                string moneyUnit;
                double moneyUsdRate;
                int succeeded;
                string state;
                // Kickstarter's first Reward item is always a "no reward" item, we should filter it out.
                var availableRewardGroups = from reward in project.Rewards
                                            where reward.Description != null
                                            group reward by reward.Minimum into g
                                            select g;
                if (availableRewardGroups.Count() > 0)
                //if (project.Rewards.Length > 1)
                {
                    var groupsLowestPrice = availableRewardGroups.OrderBy(group => group.Key);
                    lowestPrice = groupsLowestPrice.First().First().Minimum;
                    lowestPerson = (uint)groupsLowestPrice.First().Sum(group => group.BackersCount);
                    var groupsHottest = availableRewardGroups.OrderByDescending(group => group.Sum(reward => reward.BackersCount));
                    hottestPrice = groupsHottest.First().First().Minimum;
                    hottestPerson = (uint)groupsHottest.First().Sum(reward => reward.BackersCount);
                    var groupsMostMoney = availableRewardGroups.OrderByDescending(group => group.Sum(reward => reward.BackersCount * reward.Minimum));
                    mostMoneyPrice = groupsHottest.First().First().Minimum;
                    mostMoneyPerson = (uint)groupsHottest.First().Sum(reward => reward.BackersCount);
                }
                totalPledged = project.Pledged;
                totalBackers = project.BackersCount;
                targetMoney = project.Goal;
                moneyUnit = project.CurrencyCode;
                moneyUsdRate = Convert.ToDouble(project.StaticUsdRate);
                succeeded = project.Pledged >= project.Goal ? 1 : 0;
                state = project.State;

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $@"
INSERT INTO {TABLE_NAME} (
    id, name, lowest_price, lowest_person, hottest_price, hottest_person, most_money_price,
    most_money_person, total_pledged, total_backers, target_money, money_unit, money_usd_rate, succeeded, state
) VALUES (
    @id, @name, @lowest_price, @lowest_person, @hottest_price, @hottest_person, @most_money_price,
    @most_money_person, @total_pledged, @total_backers, @target_money, @money_unit, @money_usd_rate, @succeeded, @state
)";
                    command.Parameters.AddWithValue("id", project.ID);
                    command.Parameters.AddWithValue("name", project.Name);
                    command.Parameters.AddWithValue("lowest_price", lowestPrice);
                    command.Parameters.AddWithValue("lowest_person", lowestPerson);
                    command.Parameters.AddWithValue("hottest_price", hottestPrice);
                    command.Parameters.AddWithValue("hottest_person", hottestPerson);
                    command.Parameters.AddWithValue("most_money_price", mostMoneyPrice);
                    command.Parameters.AddWithValue("most_money_person", mostMoneyPerson);
                    command.Parameters.AddWithValue("total_pledged", totalPledged);
                    command.Parameters.AddWithValue("total_backers", totalBackers);
                    command.Parameters.AddWithValue("target_money", targetMoney);
                    command.Parameters.AddWithValue("money_unit", moneyUnit);
                    command.Parameters.AddWithValue("money_usd_rate", moneyUsdRate);
                    command.Parameters.AddWithValue("succeeded", succeeded);
                    command.Parameters.AddWithValue("state", state);
                    try
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (SQLiteException ex)
                    {
                        Debug.Print(ex.Message);
                    }
                }
            }
        }

        public bool IsActive
        {
            get
            {
                return _connection != null && _connection.State == ConnectionState.Open;
            }
        }

        public string FileName => _fileName;

        private string _fileName = null;

        private SQLiteConnection _connection = null;
        private SQLiteTransaction _transaction = null;

        private static readonly string TABLE_NAME = "ks_main";

    }

}
