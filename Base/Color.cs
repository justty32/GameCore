using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Base
{
    class GColor
    {
        public int Hue = 300;
        public int Sat = 50;
        public int Val = 50;
        public void AddHue(int h)
        {
            Hue += h;
            if (Hue > 360)
                Hue = Hue - (Hue / 360) * 360;
            else if (Hue < 0)
                Hue = 360 + (Hue - (Hue / 360) * 360);
        }
        public void AddSat(int s)
        {
            Sat += s;
            if (Sat > 75)
                Sat = 75;
            else if (Sat < 0)
                Sat = 0;
        }
        public void AddVal(int v)
        {
            Val += v;
            if (Val > 100)
                Val = 100;
            else if (Val < 30)
                Val = 30;
        }
        public void SetRandom(int seed)
        {
            System.Random r = new System.Random(seed);
            Hue = 0;
            Sat = 50;
            Val = 50;
            AddHue(r.Next());
            AddSat(r.Next());
            AddVal(r.Next());
        }
        public void SetWhite()
        {
            Sat = 0; Val = 100;
        }
        public void SetBlack()
        {
            Sat = 100; Val = 0;
        }
        public void SetGrey(int light)
        {
            Sat = 0;
            Val = light;
            if (Val > 100)
                Val = 100;
            else if (Val < 0)
                Val = 0;
        }
        public void Copy(GColor gcolor)
        {
            if (gcolor != null) 
            {
                Hue = gcolor.Hue;
                Sat = gcolor.Sat;
                Val = gcolor.Val;
            }
        }
        public void Align()
        {
            if(Hue > 360) { Hue = 360; }
            else if(Hue < 0) { Hue = 0; }
            if(Sat > 100) { Sat = 100; }
            else if(Sat < 0) { Sat = 0; }
            if(Val > 100) { Val = 100; }
            else if(Val < 0) { Val = 0; }
        }
        public GColor() { }
    }
}
