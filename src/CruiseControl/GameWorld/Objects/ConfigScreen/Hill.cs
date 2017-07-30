using CruiseControl.Content;
using GameDevCommon.Rendering;
using GameDevCommon.Rendering.Composers;
using Microsoft.Xna.Framework;
using static Core;

namespace CruiseControl.GameWorld.Objects.ConfigScreen
{
    class Hill : ConfigScreenProp
    {
        public Hill(IObjectContainer parent, Vector3 position)
            : base(parent)
        {
            _position = position;
            IsOpaque = false;
        }

        public override void LoadContent()
        {
            Texture = GetComponent<Resources>().LoadTexture("Textures/Environment/ConfigScreen/hill.png");

            base.LoadContent();
        }

        public override void Update()
        {
            _rotation.Y = _container.Screen.Camera.Yaw;

            base.Update();
        }

        protected override void CreateGeometry()
        {
            var vertices = RectangleComposer.Create(4f, 2f);
            VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, 0f, 0f));
            VertexTransformer.Offset(vertices, new Vector3(0f, 1f, 0f));
            Geometry.AddVertices(vertices);

            base.CreateGeometry();
        }
    }
}
