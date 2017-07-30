using Microsoft.Xna.Framework;
using System;

namespace CruiseControl.Scenes.InGame
{
    static class ColorReplacer
    {
        private static bool Approx(Color c1, Color c2, int maxDiff)
        {
            var diff = Math.Abs(c1.R - c2.R) + Math.Abs(c1.G - c2.G) + Math.Abs(c1.B - c2.B);
            return diff <= maxDiff;
        }

        public static void Replace(Color[] tData)
        {
            var fieldColor = new Color(235, 232, 222);

            var highway1 = new Color(255, 235, 175);
            var highway2 = new Color(247, 228, 163);
            var highway3 = new Color(228, 206, 151);

            var road1 = new Color(254, 254, 254);
            var road2 = new Color(247, 247, 247);
            var road3 = new Color(243, 242, 242);
            var road4 = new Color(239, 239, 239);
            var road5 = new Color(235, 235, 235);

            var ground1 = new Color(223, 223, 222);
            var ground2 = new Color(222, 222, 222);

            var trail1 = new Color(211, 211, 211);
            var trail2 = new Color(207, 222, 187);

            for (int x = 0; x < 1000; x++)
            {
                for (int y = 0; y < 1000; y++)
                {
                    var i = x + y * 1000;

                    if (x % 100 == 0 || y % 100 == 0)
                    {
                        tData[i] = Color.Red;
                        continue;
                    }

                    var c = tData[i];

                    // fields
                    if (c == fieldColor)
                    {
                        var m = (int)Math.Floor(x / 4d) % 2 == 0;
                        tData[i] = m ? new Color(112, 123, 61) : new Color(135, 147, 80);
                    }

                    // gray ground
                    else if (c == ground1 || c == ground2)
                        tData[i] = (i + y) % 2 == 0 ? new Color(112, 112, 60) : new Color(98, 107, 52);

                    // highway
                    else if (c == highway1)
                        tData[i] = new Color(127, 127, 127);
                    else if (Approx(c, highway2, 20))
                        tData[i] = new Color(156, 156, 156);
                    else if (Approx(c, highway3, 20))
                        tData[i] = new Color(160, 160, 160);

                    // road
                    else if (Approx(c, road1, 10))
                        tData[i] = (i + y) % 2 == 0 ? new Color(173, 173, 173) : new Color(170, 170, 170);
                    else if (Approx(c, road2, 10))
                        tData[i] = new Color(164, 164, 146);
                    else if (Approx(c, road3, 10))
                        tData[i] = new Color(151, 151, 106);
                    else if (Approx(c, road4, 8))
                        tData[i] = new Color(142, 142, 79);
                    else if (Approx(c, road5, 8))
                        tData[i] = new Color(130, 130, 40);

                    // trail
                    else if (c == trail1)
                        tData[i] = (i + y) % 2 == 0 ? new Color(164, 144, 6) : new Color(170, 149, 6);
                    else if (c == trail2)
                        tData[i] = new Color(158, 144, 17);

                }
            }
        }
    }
}
