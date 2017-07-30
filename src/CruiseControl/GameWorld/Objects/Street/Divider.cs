using CruiseControl.Content;
using GameDevCommon.Rendering;
using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using static Core;

namespace CruiseControl.GameWorld.Objects.Street
{
    internal class Divider : GameObject
    {
        private readonly int _length;

        public Divider(IObjectContainer parent, Vector3 position, int rotationY, int length)
            : base(parent)
        {
            _position = position;
            _length = length;
            _rotation = new Vector3(0, MathHelper.PiOver2 * rotationY, 0);
        }

        public override void LoadContent()
        {
            Texture = GetComponent<Resources>().LoadTexture("Textures/Roads/divider.png");
            base.LoadContent();
        }

        protected override void CreateGeometry()
        {
            var upper = CuboidComposer.Create(_length, 0.1f, 0.02f, new GeometryTextureRectangle(new Rectangle(0, 0, 32 * _length, 4), Texture));
            VertexTransformer.Rotate(upper, new Vector3(-0.4f, 0f, 0f));
            VertexTransformer.Offset(upper, new Vector3(0, 0.45f, 0));
            var lower = CuboidComposer.Create(_length, 0.1f, 0.02f, new GeometryTextureRectangle(new Rectangle(0, 4, 32 * _length, 4), Texture));
            VertexTransformer.Rotate(lower, new Vector3(0.4f, 0f, 0f));
            VertexTransformer.Offset(lower, new Vector3(0, 0.4f, 0));
            Geometry.AddVertices(upper);
            Geometry.AddVertices(lower);

            var pole1 = CuboidComposer.Create(0.1f, 0.5f, 0.1f, new GeometryTextureRectangle(new Rectangle(0, 8, 5, 17), Texture));
            VertexTransformer.Offset(pole1, new Vector3(-0.4f, -0.15f, 0.25f));
            var cover1 = RectangleComposer.Create(0.1f, 0.3f, new GeometryTextureRectangle(new Rectangle(27, 8, 5, 11), Texture));
            VertexTransformer.Offset(cover1, new Vector3(-0.4f, 0.101f, 0.1f));
            var pole2 = CuboidComposer.Create(0.1f, 0.5f, 0.1f, new GeometryTextureRectangle(new Rectangle(0, 8, 5, 17), Texture));
            VertexTransformer.Offset(pole2, new Vector3(0.4f, -0.15f, 0.25f));
            var cover2 = RectangleComposer.Create(0.1f, 0.3f, new GeometryTextureRectangle(new Rectangle(27, 8, 5, 11), Texture));
            VertexTransformer.Offset(cover2, new Vector3(0.4f, 0.101f, 0.1f));

            var vertices = pole1.Concat(cover1).Concat(pole2).Concat(cover2).ToArray();

            float startX = _length / 2f + 0.5f;
            for (int i = 0; i < _length; i += 5)
            {
                var clone = (VertexPositionNormalTexture[])vertices.Clone();
                VertexTransformer.Offset(clone, new Vector3(i - startX, 0.4f, 0f));
                Geometry.AddVertices(clone);
            }
            
            base.CreateGeometry();
        }
    }
}
