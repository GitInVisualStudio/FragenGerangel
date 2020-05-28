using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.GameBase
{
    public struct Answer
    {
        private string answer;
        private Question question;

        public string A
        {
            get
            {
                return answer;
            }

            set
            {
                answer = value;
            }
        }

        public Question Question
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

        public Answer(string answer)
        {
            this.answer = answer;
            this.question = null;
        }
    }
}
