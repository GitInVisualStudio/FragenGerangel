using FragenGerangel.GameBase;
using FragenGerangel.Utils.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils
{
    public class Globals
    {
        private static Player player;

        public static Player Player
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

        private static APIManager aPIManager;
        public static APIManager APIManager { get => aPIManager; set => aPIManager = value; }

    }
}
