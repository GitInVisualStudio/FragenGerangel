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
        public float Delta { get => delta; set => delta = value; }
        public float Speed { get => speed; set => speed = value; }
        public bool Finished { get => finished; set => finished = value; }

        public Animation(float speed = 1.0f)
        {
            Speed = speed;
            Reset();
        }

        public void Reset()
        {
            delta = 0;
            finished = false;
        }

        public virtual void Fire()
        {
            AnimationManager.AddAnimation(this);
        }

        public virtual void Stop()
        {
            _OnFinish();
        }

        private void _OnFinish()
        {
            OnFinish?.Invoke(this, reverse);
            finished = false;
            delta = 1;
        }

        public virtual void Reverse()
        {
            reverse = !reverse;
            Fire();
        }

        public virtual void Update()
        {
            if (reverse)
            {
                delta -= delta * StateManager.delta * speed * 3.0f;
                if (delta < tolerance)
                    _OnFinish();
                return;
            }
            delta += (1 - delta) * StateManager.delta * speed * 3.0f;
            if (delta > 1 - tolerance)
                _OnFinish();
        }
    }
}
