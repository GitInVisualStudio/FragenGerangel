using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.GameBase
{
    [JsonObject(MemberSerialization.Fields)]
    public class Game
    {
        private Player remotePlayer;
        private Round[] rounds;
        private int onlineID;

        public Player RemotePlayer
        {
            get
            {
                return remotePlayer;
            }

            set
            {
                remotePlayer = value;
            }
        }

        public Round[] Rounds
        {
            get
            {
                return rounds;
            }

            set
            {
                rounds = value;
            }
        }

        public Round LastRound
        {
            get
            {
                if (rounds[0] == null)
                    return null;
                for (int i = 0; i < rounds.Length; i++)
                    if (rounds[i] == null)
                        return rounds[i - 1];
                return rounds.Last();
            }
        }

        public int ScorePlayer
        {
            get
            {
                int score = 0;
                foreach (Round r in rounds)
                    if (r != null && r.Questions != null)
                        foreach (QuestionAnswer q in r.Questions)
                            if (q.AnswerPlayer == 0)
                                score++;
                return score;
            }
        }

        public int ScoreRemotePlayer
        {
            get
            {
                int score = 0;
                foreach (Round r in rounds)
                    if (r != null && r.Questions != null)
                        foreach (QuestionAnswer q in r.Questions)
                            if (q.AnswerRemotePlayer == 0)
                                score++;
                return score;
            }
        }

        public int OnlineID { get => onlineID; set => onlineID = value; }

        public Game(Player remote, int onlineID)
        {
            this.remotePlayer = remote;
            this.onlineID = onlineID;
            this.rounds = new Round[6];
        }
    }
}
