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
        private Player playerRemote;
        private Round[] rounds;

        public Player PlayerRemote
        {
            get
            {
                return playerRemote;
            }

            set
            {
                playerRemote = value;
            }
        }

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

        public Game(Player remote)
        {
            this.playerRemote = remote;
            this.rounds = new Round[6];
        }
    }
}
