using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils
{
    /// <summary>
    /// Behandelt alle animationen und verarbeitet diese
    /// </summary>
    public class AnimationManager
    {
        private static List<Animation> animations = new List<Animation>();//liste der aktiven animationen

        /// <summary>
        /// updated alle animationen
        /// </summary>
        public static void Update()
        {
            for(int i = animations.Count - 1; i >= 0; i--)
            {
                animations[i].Update();
                if (animations[i].Finished)//entfernen falls animation zu ende ist
                    animations.RemoveAt(i);
            }
        }

        /// <summary>
        /// fügt die animation hinzu
        /// </summary>
        /// <param name="animation"></param>
        public static void AddAnimation(Animation animation)
        {
            if(!animations.Contains(animation))
                animations.Add(animation);
        }
    }
}
