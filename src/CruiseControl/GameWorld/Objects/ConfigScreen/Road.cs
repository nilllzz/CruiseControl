using CruiseControl.Content;
using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering.Texture;
using Microsoft.Xna.Framework;
using static Core;

namespace CruiseControl.GameWorld.Objects.ConfigScreen
{
    class Road : GameObject
    {
        private readonly float _length;

        public Road(IObjectContainer parent, Vector3 position, float rotation, float length)
            : base(parent)
        {
            _position = position;
            _rotation.Y = rotation;
            _length = length;
        }

        public override void Update()
        {
            _position.Z -= 0.05f;

            if (_position.Z <= -1f)
                _position.Z = 0f;

            base.CreateWorld();
        }

        public override void LoadContent()
        {
            Texture = GetComponent<Resources>().LoadTexture("Textures/Environment/ConfigScreen/road.png");

            base.LoadContent();
        }
        
        protected override void CreateGeometry()
        {
            var vertices = RectangleComposer.Create(4f, _length, new GeometryTextureRectangle(0, 0, 1f, _length));
            Geometry.AddVertices(vertices);

            base.CreateGeometry();
        }
    }
}
