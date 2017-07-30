using CruiseControl.Scenes.InGame;
using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CruiseControl.GameWorld.Objects
{
    internal class ChunkPart : Floor
    {
        private static Random _random = new Random();
        
        private readonly Rectangle _textureRectangle;
        private readonly int _sizeX, _sizeZ;
        private float[] _heights;

        public ChunkPart(Chunk parent, int sizeX, int sizeZ, Vector3 position, Texture2D texture, Rectangle textureRectangle, float[] elevations)
            : base(parent)
        {
            _sizeX = sizeX;
            _sizeZ = sizeZ;
            _position = position;
            _heights = elevations;
            Texture = texture;
            _textureRectangle = textureRectangle;

            BoundingRectangle = new Rectangle((int)(_position.X - _sizeX / 2f), (int)(_position.Z - _sizeZ / 2f), _sizeX, _sizeZ);
        }
        
        protected override void CreateGeometry()
        {
            Geometry.AddVertices(RectangleComposer.Create(new[]
            {
               new Vector3(-_sizeX / 2f, _heights[0], -_sizeZ / 2f),
               new Vector3(_sizeX / 2f, _heights[1], -_sizeZ / 2f),
               new Vector3(-_sizeX / 2f, _heights[2], _sizeZ / 2f),
               new Vector3(_sizeX / 2f, _heights[3], _sizeZ / 2f)
            }, new GeometryTextureRectangle(_textureRectangle, Texture)));
        }

        public override float GetHeightForPosition(Vector2 position)
        {
            var p1 = new Vector3(-_sizeX / 2f, _heights[0], -_sizeZ / 2f);
            var p2 = new Vector3(_sizeX / 2f, _heights[1], -_sizeZ / 2f);
            var p3 = new Vector3(-_sizeX / 2f, _heights[2], _sizeZ / 2f);
            var p4 = new Vector3(_sizeX / 2f, _heights[3], _sizeZ / 2f);

            var topLeft = new Vector2(-_sizeX / 2f, -_sizeZ / 2f);
            var bottomRight = new Vector2(_sizeX / 2f, _sizeZ / 2f);
            var pos2d = position - new Vector2(_position.X, _position.Z);
            if (Vector2.Distance(pos2d, topLeft) > Vector2.Distance(pos2d, bottomRight))
            {
                // use triangle 1
                return GetHeightAtTriangle(p1, p2, p3, pos2d);
            }
            else
            {
                // use triangle 2
                return GetHeightAtTriangle(p2, p3, p4, pos2d);
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
