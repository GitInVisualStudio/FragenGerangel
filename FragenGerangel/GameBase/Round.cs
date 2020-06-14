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
        private int onlineID;
        private QuestionAnswer[] questions;
        private string[] possibleCategories;

        public string Category
        {
            get
            {
                if (questions == null || questions.First().Question == null)
                    return null;
                else
                    return questions.First().Question.Category;
            }
        }

        public QuestionAnswer[] Questions
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

        public string[] PossibleCategories { get => possibleCategories; set => possibleCategories = value; }
        public int OnlineID { get => onlineID; set => onlineID = value; }

        public Round(int onlineID, string category1, string category2, string category3)
        {
            this.onlineID = onlineID;
            this.possibleCategories = new string[] { category1, category2, category3 };
        }

        public override bool Equals(object obj)
        {

            if (obj is Round)
            {
                Round r2 = (Round)obj;
                if (r2 == null)
                    return false;
                if (Category != r2.Category)
                    return false;
                if(questions != null)
                    for (int i = 0; i < questions.Length; i++)
                    {
                        QuestionAnswer q1 = questions[i];
                        QuestionAnswer q2 = r2.questions[i];
                        if (q1.OnlineID != q2.OnlineID)
                            return false;
                    }
                if (questions == null && r2.questions != null)
                    return false;
                return true;
            }
            return base.Equals(obj);
        }
    }
}
