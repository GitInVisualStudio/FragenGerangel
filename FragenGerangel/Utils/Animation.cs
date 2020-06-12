using FragenGerangel.Utils.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils
{
    public class Animation
    {
        private float delta;
        private float tolerance = 0.01f;
        private float speed = 1.0f;
        private bool finished;
        private bool reverse;
        public event EventHandler<bool> OnFinish;
        public bool Incremental => !reverse;

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

        //public float Delta { get => delta; set => delta = value; }
        //public float Speed { get => speed; set => speed = value; }
        //public bool Finished { get => finished; set => finished = value; }

        public Animation(float speed = 1.0f)
        {
            Speed = speed;
            Reset();
        }

        public void Reset()
        {
            Delta = 0;
            Finished = false;
        }

        public virtual void Fire()
        {
            AnimationManager.AddAnimation(this);
        }

        public virtual void Stop()
        {
            _OnFinish();
            Finished = true;
            Delta = 1;
        }

        private void _OnFinish()
        {
            OnFinish?.Invoke(this, reverse);
            if(Incremental)
                Delta = 1;
            else
                Delta = 0;
            Finished = true;
        }

        public virtual void Reverse()
        {
            reverse = !reverse;
            Finished = false;
            Fire();
        }

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
