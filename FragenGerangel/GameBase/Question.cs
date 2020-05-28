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
        private Answer[] answers; // first answer is always correct one
        private Answer answerRemotePlayer;
        private Answer answerPlayer;
        private int onlineID;

        private Answer Correct
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

        public Answer AnswerRemotePlayer
        {
            get
            {
                return answerRemotePlayer;
            }

            set
            {
                answerRemotePlayer = value;
            }
        }

        public Answer AnswerPlayer
        {
            get
            {
                return answerPlayer;
            }

            set
            {
                answerPlayer = value;
            }
        }

        public int OnlineID
        {
            get
            {
                return onlineID;
            }

            set
            {
                onlineID = value;
            }
        }

        public Question(string question, string category, Answer correct, Answer wrong1, Answer wrong2, Answer wrong3, Answer answerPlayer, Answer answerRemotePlayer, int onlineID)
        {
            this.question = question;
            correct.Question = this;
            wrong1.Question = this;
            wrong2.Question = this;
            wrong3.Question = this;
            this.answers = new Answer[] { correct, wrong1, wrong2, wrong3 };
            this.answerPlayer = answerPlayer;
            this.answerRemotePlayer = answerRemotePlayer;
            this.onlineID = onlineID;
        }
    }
}
