using CruiseControl.GameWorld.Objects.Vehicles;
using Microsoft.Xna.Framework;
using System.IO;

namespace CruiseControl.Scenes.InGame
{
    class NetworkPlayer
    {
        private string _cid;
        private Vehicle _vehicle;

        public NetworkPlayer(string cid, Vehicle vehicle)
        {
            _cid = cid;
            _vehicle = vehicle;
        }

        public void Update(byte[] message)
        {
            using (var stream = new MemoryStream(message))
            {
                using (var br = new BinaryReader(stream))
                {
                    var posX = br.ReadSingle();
                    var posY = br.ReadSingle();
                    var posZ = br.ReadSingle();
                    var rotX = br.ReadSingle();
                    var rotY = br.ReadSingle();
                    var rotZ = br.ReadSingle();
                    var speed = br.ReadSingle();
                    var turning = br.ReadSingle();

                    _vehicle.UpdateNetwork(new Vector3(posX, posY, posZ), new Vector3(rotX, rotY, rotZ), speed, turning);
                }
            }
        }
    }
}
