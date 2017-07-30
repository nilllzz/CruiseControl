using Microsoft.Xna.Framework;

namespace CruiseControl.GameWorld.Objects.Vehicles
{
    internal static class VehicleColorProvider
    {
        public static Color GetColor(string name)
        {
            // C15 2018
            switch (name)
            {
                case "Shadow Black":
                    return new Color(26, 26, 26);
                case "Oxford White":
                    return new Color(249, 249, 249);
                case "Ingot Silver":
                    return new Color(189, 190, 192);
                case "Race Red":
                    return new Color(219, 25, 39);
                case "Blue Jeans":
                    return new Color(23, 70, 104);
                case "Magnetic":
                    return new Color(105, 105, 104);
                case "Caribou":
                    return new Color(100, 86, 70);
                case "Lithium Gray":
                    return new Color(111, 111, 111);
                case "Lightning Blue":
                    return new Color(25, 60, 130);
            }
            // rw kraft 2013
            switch (name)
            {
                case "Night Blue":
                    return new Color(40, 46, 71);
                case "Tornado Red":
                    return new Color(184, 60, 82);
                case "Platinum Gray":
                    return new Color(105, 115, 121);
                case "Black":
                    return new Color(35, 38, 43);
                case "Pure White":
                    return new Color(233, 234, 229);
                case "Silk Blue":
                    return new Color(44, 93, 120);
                case "Tungsten Silver":
                    return new Color(181, 180, 178);
            }

            return new Color(255, 0, 255);
        }
    }
}
