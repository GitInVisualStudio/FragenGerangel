using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.GameBase
{
    [JsonObject(MemberSerialization.Fields)]
    public class Player
    {
        private int onlineID;
        private string name;

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

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public Player(int onlineID, string name)
        {
            this.onlineID = onlineID;
            this.name = name;
        }
    }
}
