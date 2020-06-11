using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils.Render
{
    public class FontUtils
    {
        public static Font DEFAULT_FONT;
        private static PrivateFontCollection pfc;

        public static void Init(FragenGerangel fragenGerangel)
        {
            pfc = new PrivateFontCollection();
            Stream fontStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("FragenGerangel.comfortaa.ttf");
            byte[] fontdata = new byte[fontStream.Length];
            fontStream.Read(fontdata, 0, (int)fontStream.Length);
            fontStream.Close();
            unsafe
            {
                fixed (byte* pFontData = fontdata)
                {
                    pfc.AddMemoryFont((IntPtr)pFontData, fontdata.Length);
                }
            }
            DEFAULT_FONT = new Font(pfc.Families[0].Name, 20, FontStyle.Bold, GraphicsUnit.Pixel);
        }
    }
}
