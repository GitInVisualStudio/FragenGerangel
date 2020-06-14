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
        private bool opend = true;
        public Animation animation = new Animation(10);
        public event EventHandler OnClose;
        private bool start = true;

        public virtual void OnSroll(int direction)
        {

        }

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
            RWidth = 1;
            RHeight = 1;
            animation.OnFinish += Animation_OnFinish;
        }

        private void Animation_OnFinish(object sender, bool e)
        {
            opend = animation.Incremental;
        }

        public virtual void Open()
        {
            animation.Reset();
            animation.Fire();
        }

        public virtual void Close()
        {
            if (animation.Delta < 1 && animation.Incremental)
                animation._OnFinish();
            animation.Reverse();
            OnClose?.Invoke(this, null);
        }
    }
}
