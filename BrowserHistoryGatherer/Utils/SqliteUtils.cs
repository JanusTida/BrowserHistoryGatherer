using System.Data;
using System.Data.SQLite;

namespace BrowserHistoryGatherer.Utils
{
    public static class SqliteUtils
    {
        public static DataTable QueryDataTable(string dbPath, SQLiteCommand command)
        {
            SQLiteDataAdapter sqlDataAdapter;
            DataTable dataTable = new DataTable();
            var connectionStringBuilder = new SQLiteConnectionStringBuilder();
            connectionStringBuilder.DataSource = dbPath;
            connectionStringBuilder.ReadOnly = true;

            using (SQLiteConnection sqlConnection = new SQLiteConnection(connectionStringBuilder.ConnectionString))
            {
                sqlConnection.Open();
                command.Connection = sqlConnection;

                sqlDataAdapter = new SQLiteDataAdapter(command);
                sqlDataAdapter.Fill(dataTable);

                return dataTable;
            } 
        }
    }
}