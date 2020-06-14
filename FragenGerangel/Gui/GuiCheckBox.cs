using FragenGerangel.Utils;
using FragenGerangel.Utils.Math;
using FragenGerangel.Utils.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Gui
{
    /// <summary>
    /// checkbox mit zwei zuständen ob sie ausgewählt wurde oder nicht
    /// </summary>
    public class GuiCheckBox : GuiComponent
    {
        private bool enabled;
        private event EventHandler<bool> OnChange;
        private int token;
        private Animation animation;

        public bool Enabled
        {
            get
            {
                return enabled;
            }

            set
            {
                enabled = value;
            }
        }

        /// <summary>
        /// erstellen der events und animationen
        /// </summary>
        public GuiCheckBox()
        {
            token = Selected ? 10004 : 10007;
            OnClick += GuiCheckBox_OnClick;
            animation = new Animation();
            animation.Fire();
            Size = new Vector(20, 20);
        }

        /// <summary>
        /// animation der box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GuiCheckBox_OnClick(object sender, Utils.Math.Vector e)
        {
            animation.Reverse();
        }

        /// <summary>
        /// Zeichnet die box
        /// </summary>
        public override void OnRender()
        {
            StateManager.SetColor(Color.Black);
            StateManager.DrawRect(Location, Size);

            if (!animation.Incremental && animation.Delta < 0.1f)
            {
                Enabled = !Enabled;
                OnChange?.Invoke(this, Enabled);
                token = Enabled ? 10004 : 10007;
                animation.Reverse();
            }
            StateManager.Push();
            StateManager.Translate(Location + Size / 2);
            StateManager.Scale(animation.Delta);
            //rotation des zeichens
            if(animation.Incremental)
                StateManager.Rotate(animation.Delta * 360);
            else
                StateManager.Rotate(-animation.Delta * 360);
            StateManager.DrawCenteredString(((char)token).ToString(), 0, 2);
            StateManager.Pop();
        }
    }
}
