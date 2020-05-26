using FragenGerangel.Utils;
using FragenGerangel.Utils.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Gui
{
    public class GuiScreen : GuiPanel
    {
        private bool opend;
        //private Animation<float> animation = Animation<float>.GetDefaultAnimation();

        public bool Opend
        {
            get
            {
                return opend;
            }

            set
            {
                opend = value;
            }
        }

        public GuiScreen() : base()
        {
        }


        public void Open()
        {
            //animation.StartAnimation();
        }

        public void Close()
        {
            //animation.InvertAnimation();
        }
    }
}
