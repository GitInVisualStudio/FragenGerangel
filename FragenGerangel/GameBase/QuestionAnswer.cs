using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.GameBase
{
    public class QuestionAnswer
    {
        private int onlineID;
        private Question question;
        private int answerPlayer;
        private int answerRemotePlayer;

        public int OnlineID { get => onlineID; set => onlineID = value; }
        public Question Question { get => question; set => question = value; }

        /// <summary>
        /// Index der Spielerantwort. Gleich -1 wenn noch unbeantwortet
        /// </summary>
        public int AnswerPlayer { get => answerPlayer; set => answerPlayer = value; }

        /// <summary>
        /// Index der Antwort des Gegenspielers. Gleich -1 wenn noch unbeantwortet
        /// </summary>
        public int AnswerRemotePlayer { get => answerRemotePlayer; set => answerRemotePlayer = value; }

        public QuestionAnswer(int onlineID, Question question)
        {
            this.onlineID = onlineID;
            this.question = question;
        }
    }
}
