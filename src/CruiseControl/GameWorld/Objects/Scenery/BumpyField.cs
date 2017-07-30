using CruiseControl.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using static Core;

namespace CruiseControl.GameWorld.Objects.Scenery
{
    internal class BumpyField : GameObject
    {
        private static Random _random = new Random();

        private readonly int _sizeX, _sizeZ;
        private readonly int _resolution;

        public BumpyField(IObjectContainer parent, int sizeX, int sizeZ, Vector2 position, int resolution = 5)
            : base(parent)
        {
            _sizeX = sizeX;
            _sizeZ = sizeZ;
            _resolution = resolution;
            _position = new Vector3(position.X, 0, position.Y);
        }

        public override void LoadContent()
        {
            Texture = GetComponent<Resources>().LoadTexture("Textures/Environment/grass.png");
            base.LoadContent();
        }

        protected override void CreateGeometry()
        {
            var maxX = _sizeX / 2f;
            var minX = maxX * -1;
            var maxZ = _sizeZ / 2f;
            var minZ = maxZ * -1;

            var vertices = new Dictionary<Vector2, Vector3>();

            bool isImportant(Vector2 v)
                => v.X == minX || v.X == maxX || v.Y == minZ || v.Y == maxZ;

            float randomY() => _random.Next(-10, 50) / 200f;

            for (float x = minX; x < maxX; x += _resolution)
            {
                for (float z = minZ; z < maxZ; z += _resolution)
                {
                    var topleft = new Vector2(x, z);
                    if (!vertices.TryGetValue(topleft, out Vector3 v1))
                    {
                        v1 = new Vector3(x, isImportant(topleft) ? 0 : randomY(), z);
                        vertices.Add(topleft, v1);
                    }
                    var topright = new Vector2(x + _resolution, z);
                    if (!vertices.TryGetValue(topright, out Vector3 v2))
                    {
                        v2 = new Vector3(x + _resolution, isImportant(topright) ? 0 : randomY(), z);
                        vertices.Add(topright, v2);
                    }
                    var bottomleft = new Vector2(x , z + _resolution);
                    if (!vertices.TryGetValue(bottomleft, out Vector3 v3))
                    {
                        v3 = new Vector3(x, isImportant(bottomleft) ? 0 : randomY(), z + _resolution);
                        vertices.Add(bottomleft, v3);
                    }
                    var bottomright = new Vector2(x + _resolution, z + _resolution);
                    if (!vertices.TryGetValue(bottomright, out Vector3 v4))
                    {
                        v4 = new Vector3(x + _resolution, isImportant(bottomright) ? 0 : randomY(), z + _resolution);
                        vertices.Add(bottomright, v4);
                    }

                    Geometry.AddVertices(new[] {
                        new VertexPositionNormalTexture(v1, Vector3.Zero, Vector2.Zero),
                        new VertexPositionNormalTexture(v2, Vector3.Zero, Vector2.UnitX * _resolution),
                        new VertexPositionNormalTexture(v3, Vector3.Zero, Vector2.UnitY * _resolution),
                        new VertexPositionNormalTexture(v2, Vector3.Zero, Vector2.UnitX * _resolution),
                        new VertexPositionNormalTexture(v3, Vector3.Zero, Vector2.UnitY * _resolution),
                        new VertexPositionNormalTexture(v4, Vector3.Zero, Vector2.One * _resolution),
                    });
                }
            }
        }
    }
}
