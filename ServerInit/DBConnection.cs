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

        /// <summary>
        /// Die verwendete Datenbank
        /// </summary>
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

        /// <summary>
        /// Ein MySql Datenbankverbindungshelper
        /// </summary>
        public DBConnection(string host, string user, string password, string database="")
        {
            InitConnection(host, user, password, database);
            this.host = host;
            this.user = user;
            this.password = password;
        }

        /// <summary>
        /// Initialisiert eine Verbindung mit den gegebenen Daten
        /// </summary>
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

        /// <summary>
        /// Sendet eine SQL-Anfrage
        /// </summary>
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

        /// <summary>
        /// Escaped einen string, damit spezielle Charaktere ermöglicht werden. Von Stackoverflow übernommen
        /// </summary>
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
        /// Dekonstruktor der die offene Verbindung schließt
        /// </summary>
        ~DBConnection()
        {
            connection.Close();
        }
    }
}