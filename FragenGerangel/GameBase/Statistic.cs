using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.GameBase
{
    [JsonObject(MemberSerialization.Fields)]
    public class Statistic
    {
        private Player player;
        private int elo;
        private Dictionary<string, float> categoryPercentages;
        private int wins;
        private int losses;
        private int draws;
        private int perfectGames;

        /// <summary>
        /// Der Spieler, auf den die Statistik sich bezieht
        /// </summary>
        public Player Player
        {
            get
            {
                return player;
            }

            set
            {
                player = value;
            }
        }

        public int ELO
        {
            get
            {
                return elo;
            }

            set
            {
                elo = value;
            }
        }

        /// <summary>
        /// Prozentzahl der richtig beantworteten Fragen pro Kategorie
        /// </summary>
        public Dictionary<string, float> CategoryPercentages
        {
            get
            {
                return categoryPercentages;
            }

            set
            {
                categoryPercentages = value;
            }
        }

        /// <summary>
        /// Anzahl gewonnener Spiele
        /// </summary>
        public int Wins
        {
            get
            {
                return wins;
            }

            set
            {
                wins = value;
            }
        }

        /// <summary>
        /// Anzahl verlorener Spiele
        /// </summary>
        public int Losses
        {
            get
            {
                return losses;
            }

            set
            {
                losses = value;
            }
        }

        /// <summary>
        /// Anzahl unentschiedener Spiele
        /// </summary>
        public int Draws
        {
            get
            {
                return draws;
            }

            set
            {
                draws = value;
            }
        }

        /// <summary>
        /// Anzahl Spiele, bei denen jede Frage richtig beantwortet wurde
        /// </summary>
        public int PerfectGames
        {
            get
            {
                return perfectGames;
            }

            set
            {
                perfectGames = value;
            }
        }
    }
}
