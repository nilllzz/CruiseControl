using CruiseControl.Content;
using GameDevCommon.Rendering;
using GameDevCommon.Rendering.Composers;
using Microsoft.Xna.Framework;
using static Core;

namespace CruiseControl.GameWorld.Objects.ConfigScreen
{
    class Tree : ConfigScreenProp
    {
        public Tree(IObjectContainer parent, Vector3 position)
            : base(parent)
        {
            _position = position;
            IsOpaque = false;
        }

        public override void LoadContent()
        {
            Texture = GetComponent<Resources>().LoadTexture("Textures/Environment/ConfigScreen/tree.png");

            base.LoadContent();
        }

        public override void Update()
        {
            _rotation.Y = _container.Screen.Camera.Yaw;

            base.Update();
            base.CreateWorld();
        }

        protected override void CreateGeometry()
        {
            var vertices = RectangleComposer.Create(8f, 10f);
            VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, 0f, 0f));
            VertexTransformer.Offset(vertices, new Vector3(0f, 5f, 0f));
            Geometry.AddVertices(vertices);

            base.CreateGeometry();
        }
    }
}
