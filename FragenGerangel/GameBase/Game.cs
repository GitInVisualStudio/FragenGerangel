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
        private float? eloChange;
        private bool active;

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
                return rounds[rounds.Length - 1];
            }
        }

        public bool Active
        {
            get
            {
                if (rounds[0] == null)
                    return active;
                if (LastRound == null || LastRound.Questions == null || LastRound.Questions.ToList().Find(x => x.AnswerPlayer == -1 || x.AnswerRemotePlayer == -1) == null)
                    return true;
                return false;
            }
            set => active = value;
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

        /// <summary>
        /// How this game influenced the player's elo
        /// </summary>
        public float? EloChange { get => eloChange; set => eloChange = value; } 

        public Game(Player remote, int onlineID)
        {
            this.remotePlayer = remote;
            this.onlineID = onlineID;
            this.rounds = new Round[6];
        }

        public override bool Equals(object obj)
        {
            if(obj is Game)
            {
                Game game = (Game)obj;
                for (int i = 0; i < rounds.Length; i++)
                {
                    if (rounds[i] == null && game.Rounds[i] != null)
                        return false;
                    if (rounds[i] != null && game.Rounds[i] == null)
                        return false;
                    if(rounds[i] != null)
                        if (!rounds[i].Equals(game.Rounds[i]))
                            return false;
                }
                return game.RemotePlayer.Name == RemotePlayer.Name;
            }
            return base.Equals(obj);
        }
    }
}
