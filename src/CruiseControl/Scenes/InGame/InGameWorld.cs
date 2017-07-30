using CruiseControl.GameWorld;
using CruiseControl.GameWorld.Objects.Vehicles;
using CruiseControl.GameWorld.Objects.Vehicles.Cord;
using CruiseControl.GameWorld.Objects.Vehicles.Reichswagen;
using CruiseControl.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CruiseControl.Scenes.InGame
{
    class InGameWorld : World
    {
        public Vehicle PlayerVehicle { get; private set; }
        public string CurrentStreet { get; private set; } = "";

        public InGameWorld(IWorldScreen screen)
            : base(screen)
        {
            _containers = new List<IObjectContainer>();
        }

        public void LoadContent()
        {
            var r = new Random();
            var x = 0;// r.Next(0, 10);
            var z = 0;// r.Next(0, 10);

            Offset = new Vector3(x * -1000, 0, z * -1000);
            
            var chunk = LoadChunk(x, z);
            PlayerVehicle = new C15(chunk, new Vector3(0));
            PlayerVehicle.ControlSource = VehicleControlSource.Player;

            chunk.AddObject(PlayerVehicle);

            if (_parentScreen.Camera is FollowCamera f)
                f.FollowVehicle = PlayerVehicle;

            //LoadChunk(x + 1, z + 0);
            //LoadChunk(x + 1, z + 1);
            //LoadChunk(x + 0, z + 1);
            //LoadChunk(x - 1, z + 1);
            //LoadChunk(x - 1, z + 0);
            //LoadChunk(x - 1, z - 1);
            //LoadChunk(x + 0, z - 1);
            //LoadChunk(x + 1, z - 1);
        }

        private Chunk LoadChunk(int x, int y)
        {
            var chunk = new Chunk(_parentScreen, x, y);
            chunk.LoadContent();
            lock (_containers)
            {
                _containers.Add(chunk);
            }
            if (_activeContainer == null)
                _activeContainer = chunk;

            return chunk;
        }

        private int _streetUpdateDelay = 60;

        public override void Update(GameTime gameTime)
        {
            _streetUpdateDelay--;
            if (_streetUpdateDelay == 0)
            {
                var chunkPos = (_activeContainer as Chunk).GlobePosition;
                var vPos = PlayerVehicle.Position * new Vector3(Chunk.METERS_PER_UNIT_EW, 1, Chunk.METERS_PER_UNIT_NS);
                var vGlobePos = chunkPos.OffsetMeters(vPos.Z, vPos.X);
                Task.Run(() =>
                {
                    CurrentStreet = GoogleMaps.GetStreet(vGlobePos);
                    Console.WriteLine(chunkPos + "|" + vGlobePos + "|" + CurrentStreet);
                    _streetUpdateDelay = 60;
                });
            }

            base.Update(gameTime);
        }
    }
}
