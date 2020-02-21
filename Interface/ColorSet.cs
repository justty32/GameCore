using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using GameCore.Base;

namespace GameCore.Interface
{
    class ColorManager
    {
        public static Color ToRGBColor(GColor color)
        {
            color.Align();
            return Color.HSVToRGB(
                (float)color.Hue/360.0f
                , (float)color.Sat/100.0f
                , (float)color.Val/100.0f
                );
        }
        public static GColor FromRGB(byte r, byte g, byte b)
        {
            Color color = new Color32(r, g, b, 255);
            float h, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            GColor gcolor = new GColor();
            gcolor.Hue = (int)(h * 360);
            gcolor.Sat = (int)(s * 100);
            gcolor.Val = (int)(v * 100);
            return gcolor;
        }
        public static int ToInt(GColor gcolor)
        {
            if (gcolor == null)
                return 0;
            var c = (Color32)(ToRGBColor(gcolor));
            byte[] bs = new byte[4];
            bs[0] = c.r;
            bs[1] = c.g;
            bs[2] = c.b;
            bs[3] = c.a;
            return BitConverter.ToInt32(bs, 0);
        }
        public static GColor FromInt(int color)
        {
            byte[] bs = BitConverter.GetBytes(color);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bs);
            Color32 color32 = new Color32(bs[0], bs[1], bs[2], bs[3]);
            return FromRGB(bs[0], bs[1], bs[2]);
        }
    }
}
