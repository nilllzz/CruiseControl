using CruiseControl.Content;
using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering.Texture;
using Microsoft.Xna.Framework;
using System;
using static Core;

namespace CruiseControl.GameWorld.Objects.Street
{
    internal class TwoLaneHighway : StraightRoad
    {
        private static Random _r = new Random();

        public TwoLaneHighway(IObjectContainer parent, int length)
            : base(parent, length)
        {
            BoundingRectangle = new Rectangle(-3, -length / 2, 6, length);
        }

        public override void LoadContent()
        {
            Texture = GetComponent<Resources>().LoadTexture("Textures/Roads/twolanehighway.png");
            base.LoadContent();
        }
        
        protected override void CreateGeometry()
        {
            var vertices = RectangleComposer.Create(6f, Length, new GeometryTextureRectangle(0, 0, 1f, Length));
            Geometry.AddVertices(vertices);
        }
    }
}
