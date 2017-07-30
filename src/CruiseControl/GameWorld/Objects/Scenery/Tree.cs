using CruiseControl.Content;
using GameDevCommon.Rendering;
using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static Core;

namespace CruiseControl.GameWorld.Objects.Scenery
{
    class Tree : GameObject
    {
        private static VertexPositionNormalTexture[] _vertexCache;
        private static object _vertexLock = new object();

        public Tree(IObjectContainer parent, Vector3 position)
            : base(parent)
        {
            _position = position;
        }
        
        public override void LoadContent()
        {
            var texture = GetComponent<Resources>().LoadTexture("Textures/Environment/tree.png");
            Texture = texture;

            var floor = _container.GetFloor(_position);
            var y = floor.GetHeightForPosition(new Vector2(_position.X, _position.Z));
            _position.Y = y;

            base.LoadContent();
        }

        protected override void CreateGeometry()
        {
            lock (_vertexLock)
            {
                if (_vertexCache == null)
                {
                    var vertexCache = new List<VertexPositionNormalTexture>();
                    // stump
                    {
                        var vertices = TubeComposer.Create(1f, 10, 10, new GeometryTexturePoleWrapper(new Rectangle(0, 0, 64, 16), Texture.Bounds, 10));
                        VertexTransformer.Rotate(vertices, new Vector3(0, 0, MathHelper.PiOver2));
                        vertexCache.AddRange(vertices);
                    }
                    _vertexCache = vertexCache.ToArray();
                }

                Geometry.AddVertices(_vertexCache);
            }

            base.CreateGeometry();
        }
    }
}
