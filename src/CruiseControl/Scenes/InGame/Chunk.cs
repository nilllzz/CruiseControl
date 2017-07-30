using CruiseControl.GameWorld;
using CruiseControl.GameWorld.Objects;
using CruiseControl.GameWorld.Objects.Scenery;
using CruiseControl.Services;
using GameDevCommon.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Core;

namespace CruiseControl.Scenes.InGame
{
    internal class Chunk : IObjectContainer
    {
        public static float METERS_PER_UNIT_EW = 1.4178382f;
        public static float METERS_PER_UNIT_NS = 1.4835104f;

        private IWorldScreen _screen;
        private Vector3 _offset;
        private OptimizableRenderObjectCollection _objects;
        private float[] _elevations;

        public IWorldScreen Screen => _screen;
        public Vector3 Offset => _offset;
        public Vector3 Size => new Vector3(1000);
        public GlobePosition GlobePosition { get; }

        //lat, long (z, x)
        
        public Chunk(IWorldScreen screen, int x, int z)
        {
            _screen = screen;
            _offset = new Vector3(x, 0, z) * Size + _screen.World.Offset;
            GlobePosition = new GlobePosition(52.227627, 10.2411023).OffsetMeters(z * METERS_PER_UNIT_NS * Size.Z, x * METERS_PER_UNIT_EW * Size.X);
        }

        public void LoadContent()
        {
            _objects = new OptimizableRenderObjectCollection();
            Task.Run(() =>
            {
                var texture = GoogleMaps.GetStaticMap(GlobePosition);
                GetElevations();
                var i = 0;
                for (int x = 0; x < 10; x++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        var p = new ChunkPart(this, 100, 100, new Vector3(x * 100 - 450, 0, z * 100 - 450), texture,
                            new Rectangle(x * 100, z * 100, 100, 100),
                            new[]
                            {
                                _elevations[i],
                                _elevations[i + 1],
                                _elevations[i + 2],
                                _elevations[i + 3]
                            });
                        p.LoadContent();
                        i += 4;

                        lock (_objects)
                        {
                            _objects.Add(p);
                        }
                    }
                }
                lock (_objects)
                {
                    var obj = new Tree(this, new Vector3(2.5f, 0f, 0f));
                    obj.LoadContent();
                    _objects.Add(obj);

                    _objects.Optmimize<VertexPositionNormalTexture>();
                }
            });
        }

        private void GetElevations()
        {
            var halfSizeX = METERS_PER_UNIT_EW * 50;
            var halfSizeZ = METERS_PER_UNIT_NS * 50;
            var partSizeX = METERS_PER_UNIT_EW * 100;
            var partSizeZ = METERS_PER_UNIT_NS * 100;
            var positions = new List<GlobePosition>();

            GlobePosition isClose(GlobePosition pos)
                => positions.FirstOrDefault(p => p.Difference(pos) < 0.0001);
            
            for (int x = 0; x < 10; x += 1)
            {
                for (int z = 0; z < 10; z += 1)
                {
                    var offsetX = (x - 5) * partSizeX;
                    var offsetZ = (z - 5) * partSizeZ;

                    var basePos = GlobePosition.OffsetMeters(offsetZ, offsetX);

                    var topleft = basePos.OffsetMeters(-halfSizeZ, -halfSizeX);
                    var topright = basePos.OffsetMeters(-halfSizeZ, halfSizeX);
                    var bottomleft = basePos.OffsetMeters(halfSizeZ, -halfSizeX);
                    var bottomright = basePos.OffsetMeters(halfSizeZ, halfSizeX);
                    
                    var b1 = isClose(topleft);
                    var b2 = isClose(topright);
                    var b3 = isClose(bottomleft);
                    var b4 = isClose(bottomright);
                    
                    if (b1.Equals(default(GlobePosition)))
                        positions.Add(topleft);
                    else
                        positions.Add(b1);

                    if (b2.Equals(default(GlobePosition)))
                        positions.Add(topright);
                    else
                        positions.Add(b2);

                    if (b3.Equals(default(GlobePosition)))
                        positions.Add(bottomleft);
                    else
                        positions.Add(b3);

                    if (b4.Equals(default(GlobePosition)))
                        positions.Add(bottomright);
                    else
                        positions.Add(b4);
                }
            }

            _elevations = GoogleMaps.GetElevation(positions.ToArray()).Select(d => (float)d * 3).ToArray();
        }

        public void AddObject(GameObject obj)
        {
            lock (_objects)
            {
                if (!obj.LoadedContent)
                    obj.LoadContent();
                _objects.Add(obj);
            }
        }

        public void RemoveObject(GameObject obj)
        {
            lock (_objects)
            {
                obj.OnRemove();
                _objects.Remove(obj);
                obj.Dispose();
            }
        }

        public void Update(GameTime gameTime)
        {
            lock (_objects)
            {
                foreach (var o in _objects.OriginalObjects)
                {
                    o.Update();
                }
            }
        }

        public void Render(GameTime gameTime)
        {
            lock (_objects)
            {
                Controller.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                for (int i = 0; i < _objects.OpaqueObjects.Count(); i++)
                    Screen.World.Renderer.Render(_objects.OpaqueObjects.ElementAt(i));

                Controller.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
                for (int i = 0; i < _objects.TransparentObjects.Count(); i++)
                    Screen.World.Renderer.Render(_objects.TransparentObjects.ElementAt(i));
            }
        }

        public Floor GetFloor(Vector3 position)
        {
            var pos2D = new Vector2(position.X, position.Z);

            foreach (var floor in _objects.OriginalObjects.Where(o => o is Floor))
            {
                if (((Floor)floor).BoundingRectangle.Contains(pos2D))
                {
                    return (Floor)floor;
                }
            }

            return null;
        }
    }
}
