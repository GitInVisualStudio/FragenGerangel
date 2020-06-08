using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ServerInit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Reading questions...");
            List<List<string>> questions = GetQuestions();
            string query = "INSERT INTO `question` (`question`, `category`, `correct_answer`, `wrong_answer_1`, `wrong_answer_2`, `wrong_answer_3`) VALUES ";
            for(int i = 0; i < questions.Count; i++)
            {
                if (questions[i].Count != 6)
                    continue;
                query += "(";
                foreach (string part in questions[i])
                    query += '"' + Regex.Replace(DBConnection.MySQLEscape(part), "'", "''") + '"' + ',';
                query = query.Remove(query.Length - 1);
                query += "),";
            }
            query = query.Remove(query.Length - 1);
            Console.WriteLine("Establishing MySQL connection...");
            DBConnection conn = new DBConnection("localhost", "root", "");
            Console.WriteLine("Reading DB-Init commands...");
            string nonQuery = Regex.Replace(ReadResourceFile("db_init.sql"), "\r\n", "\n");
            Console.WriteLine("Executing commands...");
            conn.ExecuteNonQuery(nonQuery);
            conn.Database = "FragenGerangel";
            Console.WriteLine("Inserting questions...");
            conn.Query(query);
            Console.WriteLine("Database established. Press any key to exit.");
            Console.ReadKey();
        }

        private static List<List<string>> GetQuestions()
        {
            string questionsRaw = ReadResourceFile("questions.csv");
            questionsRaw = System.Net.WebUtility.HtmlDecode(questionsRaw);
            List<string> questionLines = Regex.Split(questionsRaw, "\r\n").ToList();
            questionLines.RemoveAt(0);
            List<List<string>> questions = new List<List<string>>();
            foreach (string q in questionLines)
            {
                string[] question = Regex.Split(q, ";");
                questions.Add(new List<string>());
                questions.Last().AddRange(question);
            }
            return questions;
        }

        private static string ReadResourceFile(string filename)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            using (var stream = thisAssembly.GetManifestResourceStream("ServerInit." + filename))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
