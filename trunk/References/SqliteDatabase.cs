using System;
using System.Data.Common;
using System.Data.SQLite;
#pragma warning disable 0162 //disable "Unreachable code detected"
#pragma warning disable 0618 //disable "This method is obsolete"

namespace ExtraMegaBlob.References
{
    public class SqliteDatabase
    {
        public SqliteDatabase(string dbfilename)
        {
            db_connect(dbfilename);
        }
        ~SqliteDatabase()
        {
            db_disconnect();
        }

        protected DbProviderFactory _dbfact;
        protected DbConnection _dbcon = null;
        protected DbConnectionStringBuilder _dbconstring;
        private void db_connect(string dbfilename)
        {
            //if (System.IO.File.Exists(dbfilename)) System.IO.File.Delete(dbfilename);
            makeProvider();
            _dbfact = DbProviderFactories.GetFactory("System.Data.SQLite");
            _dbcon = _dbfact.CreateConnection();
            _dbcon.ConnectionString = "Data Source=" + dbfilename + ";Pooling=true;FailIfMissing=false";
            _dbconstring = _dbfact.CreateConnectionStringBuilder();
            _dbconstring.ConnectionString = "Data Source=" + dbfilename + ";Pooling=true;FailIfMissing=false";
            _dbcon.Open();
        }
        private void makeProvider()
        {
            try
            {
                System.Data.DataSet set = (System.Data.DataSet)System.Configuration.ConfigurationSettings.GetConfig("system.data");
                set.Tables[0].Rows.Add("SQLite Data Provider"
                , ".Net Framework Data Provider for SQLite"
                , "System.Data.SQLite"
                , "System.Data.SQLite.SQLiteFactory, System.Data.SQLite");
                //var dataSet = ConfigurationManager.GetSection("system.data") as System.Data.DataSet;
                //dataSet.Tables[0].Rows.Add("SQLite Data Provider"
                //, ".Net Framework Data Provider for SQLite"
                //, "System.Data.SQLite"
                //, "System.Data.SQLite.SQLiteFactory, System.Data.SQLite");
            }
            catch (System.Data.ConstraintException) { }

        }
        private void db_disconnect()
        {
            _dbcon.Close();
        }
        private void db_query(string SQLite)
        {
            using (DbCommand cmd = _dbcon.CreateCommand())
            {
                if (_dbfact.GetType().Name.IndexOf("SQLite", StringComparison.OrdinalIgnoreCase) == -1)
                    cmd.CommandText = SQLite;
                else
                    cmd.CommandText = SQLite;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                }
            }
        }
        public SQLiteDataReader ReadMyData(string sql)
        {
            SQLiteDataReader sqReader = null;
            SQLiteCommand sqCommand = new SQLiteCommand();
            sqCommand.Connection = (SQLiteConnection)_dbcon;
            sqCommand.CommandText = sql;
            try
            {
                sqReader = sqCommand.ExecuteReader();

            }
            catch (Exception)
            {
            }
            return sqReader;
        }

        public void putSomething()
        {
            return;
            using (SQLiteTransaction mytransaction = ((SQLiteConnection)_dbcon).BeginTransaction())
            {
                using (SQLiteCommand mycommand = new SQLiteCommand((SQLiteConnection)_dbcon))
                {
                    mycommand.CommandText = "INSERT INTO [MyTable] ([MyId]) VALUES(@something)";//was using ? symbol
                    SQLiteParameter myparam = new SQLiteParameter();
                    mycommand.Parameters.Add(myparam);
                    for (int n = 0; n < 100000; n++)
                    {
                        myparam.Value = "something!";
                        mycommand.ExecuteNonQuery(); //efficient in loops
                    }
                }
                mytransaction.Commit();
            }
        }

        private string getSomething()
        {
            return "";
            string something = "";
            try
            {
                SQLiteDataReader sqReader = null;
                SQLiteCommand sqCommand = new SQLiteCommand();
                sqCommand.Connection = (SQLiteConnection)_dbcon;
                sqCommand.CommandText = "select `something` from `somewhere` where `something` = @something";
                SQLiteParameter param = new SQLiteParameter("@something", something);
                sqCommand.Parameters.Add(param);
                try
                {
                    sqReader = sqCommand.ExecuteReader();
                    while (sqReader.Read())
                    {
                        something = sqReader.GetString(sqReader.GetOrdinal("something"));
                    }
                }
                catch (Exception)
                {
                }
                sqReader.Close();
            }
            catch (Exception)
            {
            }
            return something;
        }
    }
}