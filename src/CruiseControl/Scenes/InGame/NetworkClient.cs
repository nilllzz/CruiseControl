using CruiseControl.GameWorld.Objects.Vehicles;
using CruiseControl.GameWorld.Objects.Vehicles.Cord;
using Microsoft.Xna.Framework;
using NetVoltr;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CruiseControl.Scenes.InGame
{
    class NetworkClient
    {
        private InGameWorld _world;
        private Voltr _voltrClient;
        private Channel _voltrChannel;
        private bool _isOpen = false;

        private Dictionary<string, NetworkPlayer> _players = new Dictionary<string, NetworkPlayer>();

        public NetworkClient(InGameWorld world)
        {
            _world = world;
            _voltrClient = new Voltr();
        }

        public async void Open()
        {
            await _voltrClient.Open();
            _voltrChannel = _voltrClient.GetChannel("drive");
            await _voltrChannel.Subscribe();
            _voltrChannel.MessageReceived += MessageReceived;
            _isOpen = true;
            Runner();
        }

        public async void Close()
        {
            await _voltrChannel.Unsubscribe();
            _isOpen = false;
        }

        private void Runner()
        {
            Task.Run(async () =>
            {
                while (_isOpen)
                {
                    using (var stream = new MemoryStream())
                    {
                        using (var bw = new BinaryWriter(stream, Encoding.UTF8, true))
                        {
                            var vehicle = _world.PlayerVehicle;
                            bw.Write(vehicle.Position.X);
                            bw.Write(vehicle.Position.Y);
                            bw.Write(vehicle.Position.Z);
                            bw.Write(vehicle.Rotation.X);
                            bw.Write(vehicle.Rotation.Y);
                            bw.Write(vehicle.Rotation.Z);
                            bw.Write(vehicle.Speed);
                            bw.Write(vehicle.Turning);
                        }

                        var buffer = new byte[stream.Length];
                        stream.Position = 0;
                        stream.Read(buffer, 0, (int)stream.Length);
                        await _voltrChannel.Publish(buffer);
                    }
                    
                    await Task.Delay(5);
                }
            });
        }

        private void MessageReceived(Channel channel, string cid, byte[] message)
        {
            if (cid != _voltrClient.CId)
            {
                if (!_players.TryGetValue(cid, out var player))
                {
                    var vehicle = new C15(_world.ActiveContainer, Vector3.Zero);
                    vehicle.ControlSource = VehicleControlSource.Network;
                    player = new NetworkPlayer(cid, vehicle);
                    _players.Add(cid, player);
                    _world.ActiveContainer.AddObject(vehicle);
                }
                player.Update(message);
            }
        }
    }
}
