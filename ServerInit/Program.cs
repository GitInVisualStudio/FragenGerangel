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
            List<List<string>> questions = GetQuestions();
            string query = "INSERT INTO `question` (`question`, `category`, `correct_answer`, `wrong_answer_1`, `wrong_answer_2`, `wrong_answer_3`) VALUES ";
            foreach (List<string> question in questions)
            {
                query += "(";
                foreach (string part in question)
                    query += "\"" + part + "\",";
                query.Remove(query.Length - 1);
                query += ")";
            }

            DBConnection conn = new DBConnection("localhost", "root", "");
            conn.ExecuteNonQuery(Regex.Replace(ReadResourceFile("db_init.sql"), "\r\n", "\n"));
            conn.Database = "FragenGerangel";
            conn.Query(query);
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
