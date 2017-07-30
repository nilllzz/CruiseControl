using CruiseControl.Scenes.InGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using static Core;

namespace CruiseControl.Services
{
    internal static class GoogleMaps
    {
        private const string API_KEY = "AIzaSyAWOlz5RPg6FJ7fyTVwRu11RS2rWKblw-w";

        public static Texture2D GetStaticMap(GlobePosition position)
        {
            var url = $"https://maps.googleapis.com/maps/api/staticmap?center={position.Latitude},{position.Longitude}&zoom=15&size=500x522&scale=2&key={API_KEY}" +
                "&style=feature:all|element:labels|visibility:off";
            var request = WebRequest.CreateHttp(url);
            var response = request.GetResponse();
            var responseStream = response.GetResponseStream();

            var memStream = new MemoryStream();
            responseStream.CopyTo(memStream);

            var staticMap = Texture2D.FromStream(Controller.GraphicsDevice, memStream);

            // cut 22 pixels off the bottom of the texture because of the Google watermark:
            var tData = new Color[staticMap.Width * staticMap.Height];
            staticMap.GetData(tData, 0, tData.Length);

            // replace colors with texture colors:
            ColorReplacer.Replace(tData);

            var texture = new Texture2D(Controller.GraphicsDevice, 1000, 1000);
            texture.SetData(tData, 0, 1000 * 1000);

            staticMap.Dispose();
            responseStream.Dispose();
            response.Dispose();
            return texture;
        }

        public static string GetStreet(GlobePosition position)
        {
            var url = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={position.Latitude},{position.Longitude}&radius=50&type=route&key=AIzaSyCEBDwoSSefsfjLXJf4FNdBK0pfl-y2xAI";
            var request = WebRequest.CreateHttp(url);
            var response = request.GetResponse();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                var str = sr.ReadToEnd();
                if (str.Contains("\"name\""))
                {
                    var i = str.IndexOf("\"name\"");
                    i = str.IndexOf(":", i);
                    i = str.IndexOf("\"", i) + 1;
                    str = str.Remove(0, i);
                    str = str.Remove(str.IndexOf("\""));
                    return str;
                }
                else
                {
                    return "";
                }
            }
        }

        public static double[] GetElevation(GlobePosition[] positions)
        {
            int i = 0;
            var results = new GlobePosition[0] as IEnumerable<GlobePosition>;

            while (i < positions.Length)
            {
                results = results.Concat(GetElevationInternal(positions.Skip(i).Take(100).ToArray()));
                i += 100;
            }

            return results.Select(p => p.Altitude).ToArray();
        }

        private static GlobePosition[] GetElevationInternal(GlobePosition[] positions)
        {
            var url = "https://maps.googleapis.com/maps/api/elevation/json?locations=" + 
                            string.Join("|", positions.Select(p => $"{p.Latitude},{p.Longitude}")) + 
                            $"&key=AIzaSyDSREBUn8MgSkWJHXM0z5C7zp16RtzfbWU";
            
            var request = WebRequest.CreateHttp(url);
            var results = new GlobePosition[positions.Length];
            var index = 0;

            using (var response = request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(responseStream))
                    {
                        // avoid json parsing as it's slow, directly extract the data from the response
                        string json = sr.ReadToEnd();
                        int i = 0;
                        while ((i = json.IndexOf("\"elevation\"", i)) > -1)
                        {
                            var elevStart = json.IndexOf(":", i) + 1;
                            var elevStr = json.Substring(elevStart, (i = json.IndexOf(",", i)) - elevStart);
                            var elev = float.Parse(elevStr);
                            var pos = positions[index];
                            pos.Altitude = elev;
                            results[index] = pos;
                            index++;
                        }
                    }
                }
            }

            return results;
        }
    }
}
