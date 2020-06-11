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
    }
}
