using FragenGerangel.Utils.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils
{
    /// <summary>
    /// eine animation welche sich dem wert 1 annähert
    /// </summary>
    public class Animation
    {
        private float delta;
        private float tolerance = 0.01f; //toleranz zum beenden der animation
        private float speed = 1.0f;
        private bool finished;
        private bool reverse;
        public event EventHandler<bool> OnFinish; //wird aufgerufen wenn die animation zu ende ist

        /// <summary>
        /// ob die animation sich von 1 auf 0 annähert oder von 0 auf 1
        /// </summary>
        public bool Incremental
        {
            get
            {
                return !reverse;
            }
            set
            {
                reverse = !value;
            }
        }

        /// <summary>
        /// Ob die animation beendet ist
        /// </summary>
        public bool Finished
        {
            get
            {
                return finished;
            }

            set
            {
                finished = value;
            }
        }

        /// <summary>
        /// geschwindigkeit
        /// </summary>
        public float Speed
        {
            get
            {
                return speed;
            }

            set
            {
                speed = value;
            }
        }

        /// <summary>
        /// wie weit die animation ist 0 = anfagn 1 = ende
        /// </summary>
        public float Delta
        {
            get
            {
                return delta;
            }

            set
            {
                delta = value;
            }
        }

        /// <summary>
        /// geschwindigkeit der animation
        /// </summary>
        /// <param name="speed"></param>
        public Animation(float speed = 1.0f)
        {
            Speed = speed;
            Reset();
        }

        /// <summary>
        /// setzt die animation zurück
        /// </summary>
        public void Reset() 
        {
            Delta = 0;
            Finished = false;
        }

        /// <summary>
        /// startet die animation
        /// </summary>
        public virtual void Fire()
        {
            AnimationManager.AddAnimation(this);
        }
        /// <summary>
        /// stoppt die animation
        /// </summary>
        public virtual void Stop()
        {
            _OnFinish();
            Finished = true;
            Delta = 1;
        }

        /// <summary>
        /// wird aufgerufen wenn die animation zu ende ist
        /// </summary>
        public void _OnFinish()
        {
            OnFinish?.Invoke(this, reverse);
            if(Incremental)
                Delta = 1;
            else
                Delta = 0;
            Finished = true;
        }

        /// <summary>
        /// dreht die richtig der animation um
        /// </summary>
        public virtual void Reverse()
        {
            reverse = !reverse;
            Finished = false;
            Fire();
        }

        /// <summary>
        /// nähert die beiden werte an
        /// </summary>
        public virtual void Update()
        {
            if (reverse)
            {
                Delta -= Delta * StateManager.delta * Speed * 3.0f;
                if (Delta <= tolerance)
                    _OnFinish();
                return;
            }
            Delta += (1 - Delta) * StateManager.delta * Speed * 3.0f;
            if (Delta >= 1 - tolerance)
                _OnFinish();
        }
    }
}
