using CruiseControl.Content;
using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static Core;

namespace CruiseControl.GameWorld.Objects.Vehicles.Wheels
{
    internal class CordWheel : Wheel
    {
        private static Dictionary<float, VertexPositionNormalTexture[]> _vertexCache = new Dictionary<float, VertexPositionNormalTexture[]>();

        private readonly float _size;

        public CordWheel(IObjectContainer parent, Vehicle owner, Vector3 position, WheelType type, float size)
            : base(parent, owner, position + new Vector3(0, size, 0), type)
        {
            _size = size;
            IsOptimizable = false;
        }
        
        public override void LoadContent()
        {
            Texture = GetComponent<Resources>().LoadTexture("Textures/Vehicles/Wheels/cord_wheel.png");

            base.LoadContent();
        }
        
        protected override void CreateGeometry()
        {
            lock (_vertexCache)
            {
                if (!_vertexCache.ContainsKey(_size))
                {
                    var vertexCache = new List<VertexPositionNormalTexture>();
                    var topTexture = new GeometryTextureRectangle(new Rectangle(0, 0, 32, 32), Texture);
                    var sideTexture = new GeometryTextureRectangle(new Rectangle(0, 32, 16, 12), Texture);
                    _vertexCache.Add(_size, CylinderComposer.Create(_size, 0.2f, 10, sideTexture, topTexture));
                }

                Geometry.AddVertices(_vertexCache[_size]);
            }
            
            base.CreateGeometry();
        }
    }
}
