using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils
{
    public class AnimationManager
    {
        private static List<Animation> animations = new List<Animation>();

        public static void Update()
        {
            for(int i = animations.Count - 1; i >= 0; i--)
            {
                animations[i].Update();
                if (animations[i].Finished)
                    animations.RemoveAt(i);
            }
        }

        public static void AddAnimation(Animation animation)
        {
            if(!animations.Contains(animation))
                animations.Add(animation);
        }
    }
}
