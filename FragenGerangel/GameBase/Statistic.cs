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
