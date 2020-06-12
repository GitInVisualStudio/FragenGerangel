using FragenGerangel.GameBase;
using FragenGerangel.Utils.API;
using FragenGerangel.Utils.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FragenGerangel
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>^^
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FragenGerangel());
            //APIManager manager = new APIManager("kaminund", "12345");
            //manager.StartDuel(new Player("yamimiriam")).Wait();
            //Task<Game[]> games = manager.GetGames();
            //games.Wait();
            //foreach (Game g in games.Result)
            //    Console.WriteLine(g.OnlineID);
        }
    }
}
