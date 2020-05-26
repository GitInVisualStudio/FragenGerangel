using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils.Render
{
    public class RenderUtils
    {
        /// <summary>
        /// Färbt ein Image in eine bestimmte Farbe
        /// </summary>
        /// <param name="b"></param>
        /// <param name="color"></param>
        /// <param name="copy"></param>
        /// <returns></returns>
        public static Bitmap PaintBitmap(Bitmap b, Color color, bool copy = false)
        {
            Bitmap img = b;
            if (copy)
                img = new Bitmap(b);
            BitmapData bSrc = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat); //Lockt die Bits, damit sie gelesen und beschrieben werden können => kein struct deswegen notwendig
            int bytesPerPixel = Bitmap.GetPixelFormatSize(img.PixelFormat) / 8;
            int byteCount = bSrc.Stride * img.Height; //Insgesamten Bytes der Bitmap
            byte[] pixels = new byte[byteCount];
            IntPtr ptrFirstPixel = bSrc.Scan0;
            Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length); //Kopiert die bytes der Bitmap in einen Array
            int heightInPixels = bSrc.Height;//Höhe
            int widthInBytes = bSrc.Width * bytesPerPixel; //Pixel-Breite in bytes für das durchgehen der For-Schleife
            for (int y = 0; y < heightInPixels; y++)
            {
                int currentLine = y * bSrc.Stride;
                for (int x = 0; x < widthInBytes; x += bytesPerPixel)
                {
                    //Setzt die Farbe des Bitmap wenn ein Alpha-Wert existiert
                    if (pixels[currentLine + x + 3] == 0)
                        continue;
                    pixels[currentLine + x + 2] = color.R;
                    pixels[currentLine + x + 1] = color.G;
                    pixels[currentLine + x + 0] = color.B;
                }
            }
            Marshal.Copy(pixels, 0, ptrFirstPixel, pixels.Length);
            img.UnlockBits(bSrc);
            return img;
        }
    }
}
