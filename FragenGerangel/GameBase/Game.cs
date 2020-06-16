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
        private int scorePlayer;
        private int scoreRemotePlayer;

        /// <summary>
        /// Der Gegenspieler
        /// </summary>
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

        /// <summary>
        /// Runden des Spiels. Die Länge ist immer 6, falls die Runde noch nicht initialisiert wurde ist sie hier null.
        /// </summary>
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

        /// <summary>
        /// Die letzte initialisierte Runde. Falls das Spiel noch nicht initialisiert wurde = null
        /// </summary>
        public Round LastRound
        {
            get
            {
                if (rounds[0] == null)
                    return null;
                for (int i = 0; i < rounds.Length; i++)
                    if (rounds[i] == null)
                        return Rounds[i - 1];
                return Rounds[rounds.Length - 1];
            }
        }

        /// <summary>
        /// Der Punktestand des Spielers
        /// </summary>
        public int ScorePlayer
        {
            get
            {
                if (rounds[0] == null)
                    return scorePlayer;
                int score = 0;
                foreach (Round r in rounds)
                    if (r != null && r.Questions != null)
                        foreach (QuestionAnswer q in r.Questions)
                            if (q.AnswerPlayer == 0)
                                score++;
                return score;
            }
            set => scorePlayer = value;
        }

        /// <summary>
        /// Punktestand des Gegenspielers
        /// </summary>
        public int ScoreRemotePlayer
        {
            get
            {
                if (rounds[0] == null)
                    return scoreRemotePlayer;
                int score = 0;
                foreach (Round r in rounds)
                    if (r != null && r.Questions != null)
                        foreach (QuestionAnswer q in r.Questions)
                            if (q.AnswerRemotePlayer == 0)
                                score++;
                return score;
            }
            set => scoreRemotePlayer = value;
        }

        /// <summary>
        /// ID des Spiels
        /// </summary>
        public int OnlineID { get => onlineID; set => onlineID = value; }

        /// <summary>
        /// Wie das Spiel die ELO des Spielers beeinflusst hat. Nur gesetzt, wenn das Spiel vom Spieler beendet wurde
        /// </summary>
        public float? EloChange { get => eloChange; set => eloChange = value; }
        public bool Active 
        {
            get
            {
                if (rounds[0] == null)
                    return active;
                if (rounds.ToList().FindIndex(x => x == null) != -1)
                    return true;
                foreach (Round r in rounds)
                    if (r == null || r.Questions == null || r.Questions.ToList().FindIndex(x => x.AnswerPlayer == -1 || x.AnswerRemotePlayer == -1) != -1)
                        return true;
                return false;
            }
            set => active = value; 
        }

        public Game(Player remote, int onlineID)
        {
            this.remotePlayer = remote;
            this.onlineID = onlineID;
            this.rounds = new Round[6];
        }
    }
}
