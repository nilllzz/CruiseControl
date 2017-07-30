using CruiseControl.Content;
using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering.Texture;
using Microsoft.Xna.Framework;
using System;
using static Core;

namespace CruiseControl.GameWorld.Objects.Scenery
{
    internal class Field : Floor
    {
        private static Random _random = new Random();

        private readonly int _sizeX, _sizeZ;
        private readonly float[] _heights;

        public Field(IObjectContainer parent, int sizeX, int sizeZ, Vector3 position)
            : this(parent, sizeX, sizeZ, position, 0f, 0f, 0f, 0f) { }

        public Field(IObjectContainer parent, int sizeX, int sizeZ, Vector3 position, float yTopLeft, float yTopRight, float yBottomLeft, float yBottomRight)
            : base(parent)
        {
            _sizeX = sizeX;
            _sizeZ = sizeZ;
            _position = position;
            _heights = new[] { yTopLeft, yTopRight, yBottomLeft, yBottomRight };

            BoundingRectangle = new Rectangle((int)(_position.X - _sizeX / 2f), (int)(_position.Z - _sizeZ / 2f), _sizeX, _sizeZ);
        }

        public override void LoadContent()
        {
            Texture = GetComponent<Resources>().LoadTexture("Textures/Environment/grass.png");
            base.LoadContent();
        }

        protected override void CreateGeometry()
        {
            Geometry.AddVertices(RectangleComposer.Create(new[]
            {
               new Vector3(-_sizeX / 2f, _heights[0], -_sizeZ / 2f),
               new Vector3(_sizeX / 2f, _heights[1], -_sizeZ / 2f),
               new Vector3(-_sizeX / 2f, _heights[2], _sizeZ / 2f),
               new Vector3(_sizeX / 2f, _heights[3], _sizeZ / 2f)
            }, new GeometryTextureRectangle(0, 0, _sizeX, _sizeZ)));
        }

        public override float GetHeightForPosition(Vector2 position)
        {
            var p1 = new Vector3(-_sizeX / 2f, _heights[0], -_sizeZ / 2f) + _position;
            var p2 = new Vector3(_sizeX / 2f, _heights[1], -_sizeZ / 2f) + _position;
            var p3 = new Vector3(-_sizeX / 2f, _heights[2], _sizeZ / 2f) + _position;
            var p4 = new Vector3(_sizeX / 2f, _heights[3], _sizeZ / 2f) + _position;

            var topLeft = new Vector2(-_sizeX / 2f, -_sizeZ / 2f) + new Vector2(_position.X, _position.Z);
            var bottomRight = new Vector2(_sizeX / 2f, _sizeZ / 2f) + new Vector2(_position.X, _position.Z);
            if (Vector2.Distance(position, topLeft) > Vector2.Distance(position, bottomRight))
            {
                // use triangle 1
                return GetHeightAtTriangle(p1, p2, p3, position);
            }
            else
            {
                // use triangle 2
                return GetHeightAtTriangle(p2, p3, p4, position);
            }
        }

        private static float GetHeightAtTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector2 pos)
        {
            float det = (p2.Z - p3.Z) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Z - p3.Z);
            float l1 = ((p2.Z - p3.Z) * (pos.X - p3.X) + (p3.X - p2.X) * (pos.Y - p3.Z)) / det;
            float l2 = ((p3.Z - p1.Z) * (pos.X - p3.X) + (p1.X - p3.X) * (pos.Y - p3.Z)) / det;
            float l3 = 1.0f - l1 - l2;
            return l1 * p1.Y + l2 * p2.Y + l3 * p3.Y;
        }
    }
}
