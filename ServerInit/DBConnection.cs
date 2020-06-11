using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ServerInit
{
    public class DBConnection
    {
        private string database;

        private MySqlConnection connection;
        private string host;
        private string user;
        private string password;

        public MySqlConnection Connection
        {
            get { return connection; }
        }
        public string Database
        {
            get { return database; }
            set
            {
                try
                {
                    InitConnection(host, user, password, value);
                    database = value;
                }
                catch
                {
                    return;
                }
            }
        }

        public DBConnection(string host, string user, string password, string database="")
        {
            InitConnection(host, user, password, database);
            this.host = host;
            this.user = user;
            this.password = password;
        }

        private void InitConnection(string host, string user, string password, string database)
        {
            if (connection != null)
                connection.Close();
            string connectionString = string.Format("SERVER={0}; UID={1}; PASSWORD={2};", host, user, password);
            if (database != "")
                connectionString += " DATABASE=" + database;
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }

        public MySqlDataReader Query(string query)
        {
            try
            {
                MySqlCommand c = new MySqlCommand(query, connection);
                return c.ExecuteReader();
            }
            catch
            {
                return null;
            }
        }

        public int ExecuteNonQuery(string text)
        {
            try
            {
                MySqlCommand c = new MySqlCommand(text, connection);
                return c.ExecuteNonQuery();
            }
            catch
            {
                return -1;
            }
        }

        public static string MySQLEscape(string str)
        {
            return Regex.Replace(str, @"[\x00'""\b\n\r\t\cZ\\%_]",
                delegate (Match match)
                {
                    string v = match.Value;
                    switch (v)
                    {
                        case "\x00":            // ASCII NUL (0x00) character
                    return "\\0";
                        case "\b":              // BACKSPACE character
                    return "\\b";
                        case "\n":              // NEWLINE (linefeed) character
                    return "\\n";
                        case "\r":              // CARRIAGE RETURN character
                    return "\\r";
                        case "\t":              // TAB
                    return "\\t";
                        case "\u001A":          // Ctrl-Z
                    return "\\Z";
                        default:
                            return "\\" + v;
                    }
                });
        }

        /// <summary>
        /// Deconstructor that closes the open connection
        /// </summary>
        ~DBConnection()
        {
            connection.Close();
        }
    }
}