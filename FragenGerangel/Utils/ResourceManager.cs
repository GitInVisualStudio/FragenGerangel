using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Manager = System.Resources.ResourceManager;

namespace FragenGerangel.Utils
{
    public class ResourceManager
    {
        /// <summary>
        /// Bestimmt ein Bild für einen angegebenen Resourcenpfad.
        /// </summary>
        /// <returns>Gibt null zurück, falls Datei nicht existiert</returns>
        public static Bitmap GetImage(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MyNote.Resources." + path;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                if (stream == null)
                    return null;
                else
                    return new Bitmap(stream);
        }

        /// <summary>
        /// Bestimmt für einen Typ und einen Namen alle Bilder, die für diesen Typ unter diesem Namen in den Resourcen eingebettet sind
        /// </summary>
        public static Bitmap[] GetImages<T>(T t, string name)
        {
            List<Bitmap> images = new List<Bitmap>();
            string pathName = t.GetType().Name;
            Bitmap bmp = GetImage($"{pathName}.{name}.00.png");
            for (int i = 1; bmp != null; i++)
            {
                images.Add(bmp);
                bmp = GetImage($"{pathName}.{name}.{String.Format("{0:00}", i)}.png");
            }
            return images.ToArray();
        }
    }
}
