using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.GameBase
{
    [JsonObject(MemberSerialization.Fields)]
    public class Question
    {
        private string category;
        private string question;
        private string[] answers; // Die erste Antwort ist immer die richtige

        private string Correct
        {
            get
            {
                return answers[0];
            }
        }

        public string Category
        {
            get
            {
                return category;
            }

            set
            {
                category = value;
            }
        }

        /// <summary>
        /// Die Fragestellung
        /// </summary>
        public string Q
        {
            get
            {
                return question;
            }

            set
            {
                question = value;
            }
        }

        public Question(string question, string category, string correct, string wrong1, string wrong2, string wrong3)
        {
            this.question = question;
            this.category = category;
            this.answers = new string[] { correct, wrong1, wrong2, wrong3 };
        }
    }
}
