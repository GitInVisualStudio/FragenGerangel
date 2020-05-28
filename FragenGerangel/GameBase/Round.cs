using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.GameBase
{
    [JsonObject(MemberSerialization.Fields)]
    public class Round
    {
        private string category;
        private Question[] questions;

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

        public Question[] Questions
        {
            get
            {
                return questions;
            }

            set
            {
                questions = value;
            }
        }

        public Round(string category, Question q1, Question q2, Question q3)
        {
            this.category = category;
            this.questions = new Question[] { q1, q2, q3 };
        }
    }
}
