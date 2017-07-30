using CruiseControl.GameWorld.Objects.Vehicles.Wheels;
using CruiseControl.Scenes.Configurator;
using GameDevCommon.Rendering;
using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CruiseControl.GameWorld.Objects.Vehicles.Cord
{
    internal class C15 : Vehicle
    {
        private static Dictionary<string, VertexPositionNormalTexture[]> _vertexCache = new Dictionary<string, VertexPositionNormalTexture[]>();
        private static ConfigOption[] _availableConfigOptionCache;
        public override Vector3 ThirdPersonCameraOffset => new Vector3(0, 3, -4);

        public C15(IObjectContainer parent, Vector3 position)
            : this(parent, position, null)
        { }

        public C15(IObjectContainer parent, Vector3 position, ConfigOption[] options)
            : base(parent, position, Vector3.Zero, options)
        {
            var tireSize = GetTireSize();
            AddWheel(new CordWheel(_container, this, new Vector3(0.9f, 0, 2f), WheelType.FrontLeft, tireSize));
            AddWheel(new CordWheel(_container, this, new Vector3(-0.9f, 0, 2f), WheelType.FrontRight, tireSize));
            AddWheel(new CordWheel(_container, this, new Vector3(0.9f, 0, -1.5f), WheelType.RearLeft, tireSize));
            AddWheel(new CordWheel(_container, this, new Vector3(-0.9f, 0, -1.5f), WheelType.RearRight, tireSize));
        }

        private float GetTireSize()
        {
            var size = 0.35f;
            if (HasOption("40cm"))
                size = 0.4f;
            return size;
        }

        public override void LoadContent()
        {
            var colorConfig = Options.FirstOrDefault(o => o.Category == ConfigOption.CATEGORY_BODY_COLORS);
            var color = VehicleColorProvider.GetColor(colorConfig.Name);
            SetTexture("Textures/Vehicles/Cord/cord_c15.png", color);
            base.LoadContent();
        }

        #region model

        protected override void CreateGeometry()
        {
            var id = string.Join("-", Options.Select(o => o.Name));
            lock (_vertexCache)
            {
                if (!_vertexCache.ContainsKey(id))
                {
                    var vertexCache = new List<VertexPositionNormalTexture>();

                    var hasOffroadTrim = HasOption("Offroad Trim");
                    var tireSize = GetTireSize();

                    // front grill, lights
                    {
                        // left light
                        {
                            // front
                            {
                                var texture = !hasOffroadTrim ?
                                    new GeometryTextureRectangle(new Rectangle(29, 0, 7, 16), Texture) :
                                    new GeometryTextureRectangle(new Rectangle(105, 64, 7, 16), Texture);
                                var vertices = RectangleComposer.Create(new[]
                                {
                                    new Vector3(-0.45f, 1.2f, 2.8f),
                                    new Vector3(-0.9f, 1.2f, 2.5f),
                                    new Vector3(-0.45f, 0.4f, 2.8f),
                                    new Vector3(-0.9f, 0.4f, 2.5f),
                                }, texture);
                                vertexCache.AddRange(vertices);
                            }
                            // top
                            {
                                var texture = new GeometryTextureRectangle(new Rectangle(1, 16, 1, 1), Texture);
                                var vertices = RectangleComposer.Create(new[]
                                {
                                    new Vector3(-0.9f, 1.2f, 2.5f),
                                    new Vector3(-0.45f, 1.2f, 2.5f),
                                    new Vector3(-0.45f, 1.2f, 2.8f),
                                    new Vector3(-0.45f, 1.2f, 2.8f),
                                }, texture);
                                vertexCache.AddRange(vertices);
                            }
                        }
                        // right light
                        {
                            // front
                            {
                                var texture = !hasOffroadTrim ?
                                    new GeometryTextureRectangle(new Rectangle(0, 0, 7, 16), Texture) :
                                    new GeometryTextureRectangle(new Rectangle(76, 64, 7, 16), Texture);
                                var vertices = RectangleComposer.Create(new[]
                                {
                                    new Vector3(0.9f, 1.2f, 2.5f),
                                    new Vector3(0.45f, 1.2f, 2.8f),
                                    new Vector3(0.9f, 0.4f, 2.5f),
                                    new Vector3(0.45f, 0.4f, 2.8f),
                                }, texture);
                                vertexCache.AddRange(vertices);
                            }
                            // top
                            {
                                var texture = new GeometryTextureRectangle(new Rectangle(1, 16, 1, 1), Texture);
                                var vertices = RectangleComposer.Create(new[]
                                {
                                    new Vector3(0.9f, 1.2f, 2.5f),
                                    new Vector3(0.45f, 1.2f, 2.5f),
                                    new Vector3(0.45f, 1.2f, 2.8f),
                                    new Vector3(0.45f, 1.2f, 2.8f),
                                }, texture);
                                vertexCache.AddRange(vertices);
                            }
                        }
                        // grille
                        {
                            // front
                            {
                                var texture = !hasOffroadTrim ?
                                    new GeometryTextureRectangle(new Rectangle(7, 0, 22, 16), Texture) :
                                    new GeometryTextureRectangle(new Rectangle(83, 64, 22, 16), Texture);
                                var vertices = RectangleComposer.Create(new[]
                                {
                                    new Vector3(-0.45f, 1.2f, 2.8f),
                                    new Vector3(0.45f, 1.2f, 2.8f),
                                    new Vector3(-0.45f, 0.4f, 2.8f),
                                    new Vector3(0.45f, 0.4f, 2.8f),
                                }, texture);
                                vertexCache.AddRange(vertices);
                            }
                            // top
                            {
                                var texture = new GeometryTextureRectangle(new Rectangle(1, 16, 1, 1), Texture);
                                var vertices = RectangleComposer.Create(new[]
                                {
                                new Vector3(-0.45f, 1.2f, 2.8f),
                                new Vector3(0.45f, 1.2f, 2.8f),
                                new Vector3(-0.45f, 1.2f, 2.5f),
                                new Vector3(0.45f, 1.2f, 2.5f),
                            }, texture);
                                vertexCache.AddRange(vertices);
                            }
                        }
                    }
                    // wheel arches
                    {
                        var solidColor = new GeometryTextureRectangle(new Rectangle(1, 16, 1, 1), Texture);
                        var darkColor = new GeometryTextureRectangle(new Rectangle(0, 16, 1, 1), Texture);
                        var side = new VertexPositionNormalTexture[0] as IEnumerable<VertexPositionNormalTexture>;

                        {
                            var vertices = RectangleComposer.Create(0.1f, 0.8f, solidColor);
                            VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, MathHelper.PiOver2, 0));
                            VertexTransformer.Offset(vertices, new Vector3(0.9f, 0.8f, 2.45f));
                            side = side.Concat(vertices);
                        }
                        {
                            var vertices = RectangleComposer.Create(0.1f, 0.5f, solidColor);
                            VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, MathHelper.PiOver2, 0));
                            VertexTransformer.Offset(vertices, new Vector3(0.9f, 0.95f, 2.35f));
                            side = side.Concat(vertices);
                        }
                        {
                            var vertices = RectangleComposer.Create(0.6f, 0.35f, solidColor);
                            VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, MathHelper.PiOver2, 0));
                            VertexTransformer.Offset(vertices, new Vector3(0.9f, 1.025f, 2.0f));
                            side = side.Concat(vertices);
                        }
                        {
                            var vertices = RectangleComposer.Create(0.1f, 0.8f, solidColor);
                            VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, MathHelper.PiOver2, 0));
                            VertexTransformer.Offset(vertices, new Vector3(0.9f, 0.8f, 1.55f));
                            side = side.Concat(vertices);
                        }
                        {
                            var vertices = RectangleComposer.Create(0.1f, 0.5f, solidColor);
                            VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, MathHelper.PiOver2, 0));
                            VertexTransformer.Offset(vertices, new Vector3(0.9f, 0.95f, 1.65f));
                            side = side.Concat(vertices);
                        }

                        var sideArr = side.ToArray();
                        vertexCache.AddRange(sideArr);
                        VertexTransformer.Offset(sideArr, new Vector3(-1.8f, 0, 0));
                        vertexCache.AddRange(sideArr);
                        VertexTransformer.Offset(sideArr, new Vector3(0f, 0f, -3.5f));
                        vertexCache.AddRange(sideArr);
                        VertexTransformer.Offset(sideArr, new Vector3(1.8f, 0, 0));
                        vertexCache.AddRange(sideArr);

                        var arch = new VertexPositionNormalTexture[0] as IEnumerable<VertexPositionNormalTexture>;
                        {
                            var vertices = CuboidComposer.Create(1.85f, 0.5f, 0.1f, darkColor);
                            VertexTransformer.Rotate(vertices, new Vector3(0.6f, 0f, 0f));
                            VertexTransformer.Offset(vertices, new Vector3(0f, 0.75f, 1.7f));
                            arch = arch.Concat(vertices);
                        }
                        {
                            var vertices = CuboidComposer.Create(1.85f, 0.2f, 0.5f, darkColor);
                            VertexTransformer.Offset(vertices, new Vector3(0f, 0.86f, 2f));
                            arch = arch.Concat(vertices);
                        }
                        {
                            var vertices = CuboidComposer.Create(1.85f, 0.5f, 0.1f, darkColor);
                            VertexTransformer.Rotate(vertices, new Vector3(-0.6f, 0f, 0f));
                            VertexTransformer.Offset(vertices, new Vector3(0f, 0.75f, 2.3f));
                            arch = arch.Concat(vertices);
                        }

                        var archArr = arch.ToArray();
                        vertexCache.AddRange(archArr);
                        VertexTransformer.Offset(archArr, new Vector3(0f, 0, -3.5f));
                        vertexCache.AddRange(archArr);

                        side = null;
                        sideArr = null;
                        arch = null;
                        archArr = null;
                    }
                    // axis
                    {
                        // axis
                        {
                            var vertices = CuboidComposer.Create(1.75f, 0.1f, 0.1f,
                                new GeometryTextureRectangle(new Rectangle(0, 16, 1, 1), Texture));

                            VertexTransformer.Offset(vertices, new Vector3(0, tireSize + 0.05f, 2f));
                            vertexCache.AddRange(vertices);
                            VertexTransformer.Offset(vertices, new Vector3(0, 0f, -3.5f));
                            vertexCache.AddRange(vertices);
                        }
                        // block
                        {
                            var vertices = CuboidComposer.Create(1.2f, 0.4f, 0.5f,
                                new GeometryTextureRectangle(new Rectangle(0, 19, 1, 1), Texture));

                            VertexTransformer.Offset(vertices, new Vector3(0, tireSize + 0.19f, 2f));
                            vertexCache.AddRange(vertices);
                            VertexTransformer.Offset(vertices, new Vector3(0, 0f, -3.5f));
                            vertexCache.AddRange(vertices);
                        }
                    }
                    // bottom
                    {
                        var vertices = RectangleComposer.Create(1.8f, 2.6f,
                                new GeometryTextureRectangle(new Rectangle(0, 16, 1, 1), Texture));

                        VertexTransformer.Offset(vertices, new Vector3(0, 0.6f, 0.25f));
                        vertexCache.AddRange(vertices);
                    }
                    // hood
                    {
                        var texture = new GeometryTextureRectangle(new Rectangle(32, 16, 32, 16), Texture);
                        var vertices = RectangleComposer.Create(1.8f, 1.0f, texture);
                        VertexTransformer.Offset(vertices, new Vector3(0, 1.2f, 2f));
                        vertexCache.AddRange(vertices);
                    }
                    // doors
                    {
                        var texture = !hasOffroadTrim ?
                            new GeometryTextureRectangle(new Rectangle(32, 32, 37, 16), Texture) :
                            new GeometryTextureRectangle(new Rectangle(32, 64, 37, 16), Texture);
                        var vertices = RectangleComposer.Create(2.5f, 0.8f, texture);
                        VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, MathHelper.PiOver2, 0));
                        {
                            VertexTransformer.Offset(vertices, new Vector3(-0.9f, 0.8f, 0.25f));
                            vertexCache.AddRange(vertices);
                        }
                        {
                            VertexTransformer.Offset(vertices, new Vector3(1.8f, 0f, 0f));
                            vertexCache.AddRange(vertices);
                        }
                    }
                    // windshield
                    {
                        var texture = new GeometryTextureRectangle(new Rectangle(48, 0, 32, 16), Texture);
                        var vertices = RectangleComposer.Create(new[] {
                            new Vector3(-0.7f, 0.4f, -0.4f),
                            new Vector3(0.7f, 0.4f, -0.4f),
                            new Vector3(-0.9f, -0.4f, 0f),
                            new Vector3(0.9f, -0.4f, 0f)
                        }, texture);
                        VertexTransformer.Offset(vertices, new Vector3(0, 1.6f, 1.5f));
                        vertexCache.AddRange(vertices);
                    }
                    // a pillars
                    {
                        {
                            var texture = new GeometryTextureRectangle(new Rectangle(48, 0, 1, 1), Texture);
                            var vertices = RectangleComposer.Create(new[] {
                                new Vector3(-0.9f, -0.4f, 0f),
                                new Vector3(-0.9f, -0.4f, -0.4f),
                                new Vector3(-0.7f, 0.4f, -0.4f),
                                new Vector3(-0.7f, 0.4f, -0.4f),
                            }, texture);
                            VertexTransformer.Offset(vertices, new Vector3(0, 1.6f, 1.5f));
                            vertexCache.AddRange(vertices);
                        }
                        {
                            var texture = new GeometryTextureRectangle(new Rectangle(48, 0, 1, 1), Texture);
                            var vertices = RectangleComposer.Create(new[] {
                                new Vector3(0.9f, -0.4f, 0f),
                                new Vector3(0.9f, -0.4f, -0.4f),
                                new Vector3(0.7f, 0.4f, -0.4f),
                                new Vector3(0.7f, 0.4f, -0.4f),
                            }, texture);
                            VertexTransformer.Offset(vertices, new Vector3(0, 1.6f, 1.5f));
                            vertexCache.AddRange(vertices);
                        }
                    }
                    // windows
                    {
                        {
                            var texture = new GeometryTextureRectangle(new Rectangle(32, 48, 24, 16), Texture);
                            var vertices = RectangleComposer.Create(new[] {
                                new Vector3(-0.7f, 0.4f, -1.7f),
                                new Vector3(-0.7f, 0.4f, -0.4f),
                                new Vector3(-0.9f, -0.4f, -1.7f),
                                new Vector3(-0.9f, -0.4f, -0.4f),
                            }, texture);
                            VertexTransformer.Offset(vertices, new Vector3(0, 1.6f, 1.5f));
                            vertexCache.AddRange(vertices);
                        }
                        {
                            var texture = new GeometryTextureRectangle(new Rectangle(32, 48, 24, 16), Texture);
                            var vertices = RectangleComposer.Create(new[] {
                                new Vector3(0.7f, 0.4f, -1.7f),
                                new Vector3(0.7f, 0.4f, -0.4f),
                                new Vector3(0.9f, -0.4f, -1.7f),
                                new Vector3(0.9f, -0.4f, -0.4f),
                            }, texture);
                            VertexTransformer.Offset(vertices, new Vector3(0, 1.6f, 1.5f));
                            vertexCache.AddRange(vertices);
                        }
                    }
                    // rear window
                    {
                        var texture = new GeometryTextureRectangle(new Rectangle(80, 32, 32, 16), Texture);
                        var vertices = RectangleComposer.Create(new[] {
                            new Vector3(-0.7f, 0.4f, -1.7f),
                            new Vector3(0.7f, 0.4f, -1.7f),
                            new Vector3(-0.9f, -0.4f, -1.7f),
                            new Vector3(0.9f, -0.4f, -1.7f)
                        }, texture);
                        VertexTransformer.Offset(vertices, new Vector3(0, 1.6f, 1.5f));
                        vertexCache.AddRange(vertices);
                    }
                    // roof
                    {
                        var texture = new GeometryTextureRectangle(new Rectangle(1, 16, 1, 1), Texture);
                        var vertices = RectangleComposer.Create(new[] {
                            new Vector3(-0.7f, 0.4f, -0.4f),
                            new Vector3(0.7f, 0.4f, -0.4f),
                            new Vector3(-0.7f, 0.4f, -1.7f),
                            new Vector3(0.7f, 0.4f, -1.7f)
                        }, texture);
                        VertexTransformer.Offset(vertices, new Vector3(0, 1.6f, 1.5f));
                        vertexCache.AddRange(vertices);
                    }
                    // front bed
                    {
                        var texture = new GeometryTextureRectangle(new Rectangle(80, 48, 32, 9), Texture);
                        var vertices = RectangleComposer.Create(new[] {
                            new Vector3(-0.9f, -0.4f, -1.7f),
                            new Vector3(0.9f, -0.4f, -1.7f),
                            new Vector3(-0.9f, -0.8f, -1.7f),
                            new Vector3(0.9f, -0.8f, -1.7f)
                        }, texture);
                        VertexTransformer.Offset(vertices, new Vector3(0, 1.6f, 1.5f));
                        vertexCache.AddRange(vertices);
                    }
                    // bed
                    {
                        var texture = new GeometryTextureRectangle(new Rectangle(80, 0, 32, 32), Texture);
                        var vertices = RectangleComposer.Create(1.8f, 2.2f, texture);
                        VertexTransformer.Offset(vertices, new Vector3(0, 0.8f, -1.3f));
                        vertexCache.AddRange(vertices);
                    }
                    // bed liner
                    if (HasOption("Side Bedliner"))
                    {
                        var texture = new GeometryTextureRectangle(new Rectangle(0, 16, 1, 1), Texture);
                        var vertices = CuboidComposer.Create(0.02f, 0.02f, 2.6f, texture);

                        VertexTransformer.Offset(vertices, new Vector3(0.9f, 1.2f, -1.3f));
                        vertexCache.AddRange(vertices);
                        VertexTransformer.Offset(vertices, new Vector3(-1.8f, 0f, 0f));
                        vertexCache.AddRange(vertices);
                    }
                    // rear
                    {
                        var texture = new GeometryTextureCuboidWrapper();
                        texture.AddSide(new[] { CuboidSide.Top, CuboidSide.Bottom },
                            new GeometryTextureRectangle(new Rectangle(0, 16, 1, 1), Texture));
                        if (hasOffroadTrim)
                        {
                            texture.AddSide(new[] { CuboidSide.Right, CuboidSide.Left },
                                new GeometryTextureRectangle(new Rectangle(8, 80, 4, 16), Texture));
                        }
                        else
                        {
                            texture.AddSide(new[] { CuboidSide.Right, CuboidSide.Left },
                                new GeometryTextureRectangle(new Rectangle(0, 80, 4, 16), Texture));
                        }
                        if (HasOption("Premium Hatch"))
                        {
                            texture.AddSide(new[] { CuboidSide.Front },
                                new GeometryTextureRectangle(new Rectangle(0, 64, 32, 16), Texture));
                        }
                        else
                        {
                            texture.AddSide(new[] { CuboidSide.Front },
                                new GeometryTextureRectangle(new Rectangle(16, 80, 32, 16), Texture));
                        }
                        texture.AddSide(new[] { CuboidSide.Back },
                            new GeometryTextureRectangle(new Rectangle(80, 48, 32, 9), Texture));

                        var vertices = CuboidComposer.Create(1.8f, 0.8f, 0.2f, texture);
                        VertexTransformer.Offset(vertices, new Vector3(0, 0.8f, -2.5f));
                        vertexCache.AddRange(vertices);
                    }
                    // rear sides
                    {
                        var texture = !hasOffroadTrim ?
                            new GeometryTextureRectangle(new Rectangle(4, 80, 4, 16), Texture) :
                            new GeometryTextureRectangle(new Rectangle(12, 80, 4, 16), Texture);
                        var vertices = RectangleComposer.Create(0.4f, 0.8f, texture);
                        VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, MathHelper.PiOver2, 0));

                        VertexTransformer.Offset(vertices, new Vector3(0.9f, 0.8f, -2.2f));
                        vertexCache.AddRange(vertices);
                        VertexTransformer.Offset(vertices, new Vector3(-1.8f, 0f, 0f));
                        vertexCache.AddRange(vertices);
                    }
                    // exhaust
                    {
                        var hasSportExhausts = HasOption("Sport Exhausts");
                        IGeometryTextureDefintion texture;
                        if (hasSportExhausts)
                            texture = new GeometryTextureRectangle(new Rectangle(0, 16, 1, 1), Texture);
                        else
                            texture = new GeometryTextureRectangle(new Rectangle(0, 18, 1, 1), Texture);

                        var vertices = TubeComposer.Create(0.05f, 1f, 5, texture);
                        VertexTransformer.Rotate(vertices, new Vector3(0, MathHelper.PiOver2, 0));

                        VertexTransformer.Offset(vertices, new Vector3(0.6f, 0.4f, -2.2f));
                        vertexCache.AddRange(vertices);

                        if (hasSportExhausts)
                        {
                            VertexTransformer.Offset(vertices, new Vector3(-0.1f, 0f, 0f));
                            vertexCache.AddRange(vertices);
                            VertexTransformer.Offset(vertices, new Vector3(-1f, 0f, 0f));
                            vertexCache.AddRange(vertices);
                            VertexTransformer.Offset(vertices, new Vector3(-0.1f, 0f, 0f));
                            vertexCache.AddRange(vertices);
                        }
                    }
                    // racing stripes
                    var hasRacingStripes = HasOption("Racing Stripes");
                    if (hasRacingStripes)
                    {
                        var texture = new GeometryTextureRectangle(new Rectangle(0, 17, 1, 1), Texture);
                        var vertices = RectangleComposer.Create(0.2f, 1.3f, texture);

                        VertexTransformer.Offset(vertices, new Vector3(0.3f, 1.201f, 2.15f));
                        vertexCache.AddRange(vertices);
                        VertexTransformer.Offset(vertices, new Vector3(-0.6f, 0f, 0f));
                        vertexCache.AddRange(vertices);
                        VertexTransformer.Offset(vertices, new Vector3(0f, 0.8f, -1.7f));
                        vertexCache.AddRange(vertices);
                        VertexTransformer.Offset(vertices, new Vector3(0.6f, 0f, 0f));
                        vertexCache.AddRange(vertices);
                    }
                    // hood vents
                    if (HasOption("Hood Vents"))
                    {
                        var texture = new GeometryTextureCuboidWrapper();
                        texture.AddSide(new[] { CuboidSide.Front, CuboidSide.Bottom, CuboidSide.Left, CuboidSide.Right },
                            new GeometryTextureRectangle(new Rectangle(64, 20, 9, 4), Texture));
                        texture.AddSide(new[] { CuboidSide.Back },
                            new GeometryTextureRectangle(new Rectangle(64, 16, 9, 4), Texture));
                        if (hasRacingStripes)
                        {
                            texture.AddSide(new[] { CuboidSide.Top },
                                new GeometryTextureRectangle(new Rectangle(72, 24, 8, 8), Texture));
                        }
                        else
                        {
                            texture.AddSide(new[] { CuboidSide.Top },
                                new GeometryTextureRectangle(new Rectangle(64, 24, 8, 8), Texture));
                        }
                        var vertices = CuboidComposer.Create(0.4f, 0.2f, 0.4f, texture);
                        VertexTransformer.Rotate(vertices, new Vector3(-0.35f, 0f, 0f));
                        VertexTransformer.Offset(vertices, new Vector3(0.4f, 1.2f, 2f));
                        vertexCache.AddRange(vertices);
                        VertexTransformer.Offset(vertices, new Vector3(-0.8f, 0f, 0f));
                        vertexCache.AddRange(vertices);
                    }
                    // cab lights
                    if (HasOption("Cab Lights"))
                    {
                        // lights
                        {
                            var vertices = CylinderComposer.Create(0.1f, 0.05f, 10,
                                new GeometryTextureRectangle(new Rectangle(112, 0, 1, 1), Texture),
                                new GeometryTextureRectangle(new Rectangle(112, 0, 16, 16), Texture),
                                new GeometryTextureRectangle(new Rectangle(112, 0, 1, 1), Texture));
                            VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, -MathHelper.PiOver2, 0f));
                            VertexTransformer.Offset(vertices, new Vector3(0.5f, 2.15f, 0.8f));
                            vertexCache.AddRange(vertices);
                            VertexTransformer.Offset(vertices, new Vector3(-(1 / 3f), 0f, 0f));
                            vertexCache.AddRange(vertices);
                            VertexTransformer.Offset(vertices, new Vector3(-(1 / 3f), 0f, 0f));
                            vertexCache.AddRange(vertices);
                            VertexTransformer.Offset(vertices, new Vector3(-(1 / 3f), 0f, 0f));
                            vertexCache.AddRange(vertices);
                        }
                        // beam
                        {
                            var texture = new GeometryTextureCuboidWrapper();
                            texture.AddSide(new[] { CuboidSide.Front, CuboidSide.Back, CuboidSide.Top, CuboidSide.Bottom },
                                new GeometryTextureRectangle(new Rectangle(112, 16, 16, 4), Texture));
                            texture.AddSide(new[] { CuboidSide.Left, CuboidSide.Right },
                                new GeometryTextureRectangle(new Rectangle(112, 16, 4, 4), Texture));
                            var vertices = CuboidComposer.Create(1.3f, 0.05f, 0.05f, texture);
                            VertexTransformer.Offset(vertices, new Vector3(0f, 2.1f, 0.75f));
                            vertexCache.AddRange(vertices);
                        }
                        // stelts
                        {
                            var vertices = CuboidComposer.Create(0.05f, 0.1f, 0.05f,
                                new GeometryTextureRectangle(new Rectangle(112, 16, 4, 4), Texture));
                            VertexTransformer.Offset(vertices, new Vector3(-0.61f, 2.05f, 0.75f));
                            vertexCache.AddRange(vertices);
                            VertexTransformer.Offset(vertices, new Vector3(1.22f, 0f, 0f));
                            vertexCache.AddRange(vertices);
                        }
                    }
                    // antenna
                    if (HasOption("Antenna"))
                    {
                        var vertices = CuboidComposer.Create(0.015f, 1f, 0.015f,
                            new GeometryTextureRectangle(new Rectangle(0, 16, 1, 1), Texture));
                        VertexTransformer.Offset(vertices, new Vector3(-0.88f, 1.7f, 1.5f));
                        vertexCache.AddRange(vertices);
                    }
                    // spare tire
                    if (HasOption("Spare Tire"))
                    {
                        var topTexture = new GeometryTextureRectangle(new Rectangle(0, 32, 32, 32), Texture);
                        var sideTexture = new GeometryTextureRectangle(new Rectangle(16, 16, 16, 12), Texture);

                        var vertices = CylinderComposer.Create(tireSize, 0.2f, 10, sideTexture, topTexture);
                        VertexTransformer.Rotate(vertices, new Vector3(0, MathHelper.PiOver2, 0f));
                        VertexTransformer.Offset(vertices, new Vector3(0.45f, 0.8f + tireSize, -0.3f));

                        vertexCache.AddRange(vertices);
                    }
                    // side mirrors
                    if (HasOption("Side Mirrors"))
                    {
                        // supports
                        {
                            var vertices = CuboidComposer.Create(0.2f, 0.04f, 0.04f,
                                new GeometryTextureRectangle(new Rectangle(0, 16, 1, 1), Texture));

                            VertexTransformer.Offset(vertices, new Vector3(0.95f, 1.3f, 1.3f));
                            vertexCache.AddRange(vertices);
                            VertexTransformer.Offset(vertices, new Vector3(0f, 0.1f, 0f));
                            vertexCache.AddRange(vertices);
                            VertexTransformer.Offset(vertices, new Vector3(-1.9f, 0f, 0f));
                            vertexCache.AddRange(vertices);
                            VertexTransformer.Offset(vertices, new Vector3(0f, -0.1f, 0f));
                            vertexCache.AddRange(vertices);
                        }
                        // mirrors
                        {
                            var texture = new GeometryTextureCuboidWrapper();
                            texture.AddSide(new[] { CuboidSide.Back, CuboidSide.Bottom, CuboidSide.Left, CuboidSide.Right, CuboidSide.Top },
                                new GeometryTextureRectangle(new Rectangle(0, 16, 1, 1), Texture));
                            texture.AddSide(new[] { CuboidSide.Front },
                                new GeometryTextureRectangle(new Rectangle(112, 20, 16, 12), Texture));
                            var vertices = CuboidComposer.Create(0.2f, 0.15f, 0.05f, texture);

                            VertexTransformer.Offset(vertices, new Vector3(1.1f, 1.36f, 1.3f));
                            vertexCache.AddRange(vertices);
                            VertexTransformer.Offset(vertices, new Vector3(-2.2f, 0f, 0f));
                            vertexCache.AddRange(vertices);
                        }
                    }
                    // grille guard
                    if (HasOption("Grille Guard"))
                    {
                        // connecting bars
                        {
                            var vertices = CuboidComposer.Create(0.1f,
                                 new GeometryTextureRectangle(new Rectangle(112, 32, 4, 4), Texture));

                            VertexTransformer.Offset(vertices, new Vector3(0.35f, 0.5f, 2.85f));
                            vertexCache.AddRange(vertices);
                            VertexTransformer.Offset(vertices, new Vector3(-0.7f, 0f, 0f));
                            vertexCache.AddRange(vertices);
                        }
                        // vertical beams
                        {
                            var texture = new GeometryTextureCuboidWrapper();
                            texture.AddSide(new[] { CuboidSide.Left, CuboidSide.Right, CuboidSide.Front, CuboidSide.Back },
                                new GeometryTextureRectangle(new Rectangle(112, 32, 4, 32), Texture));
                            texture.AddSide(new[] { CuboidSide.Top, CuboidSide.Bottom },
                                new GeometryTextureRectangle(new Rectangle(112, 32, 4, 4), Texture));
                            var vertices = CuboidComposer.Create(0.075f, 0.6f, 0.075f, texture);

                            VertexTransformer.Offset(vertices, new Vector3(0.35f, 0.75f, 2.95f));
                            vertexCache.AddRange(vertices);
                            VertexTransformer.Offset(vertices, new Vector3(-0.7f, 0f, 0f));
                            vertexCache.AddRange(vertices);
                        }
                        // horizontal beams
                        {
                            var texture = new GeometryTextureCuboidWrapper();
                            texture.AddSide(new[] { CuboidSide.Left, CuboidSide.Right, CuboidSide.Front, CuboidSide.Back },
                                new GeometryTextureRectangle(new Rectangle(112, 32, 4, 32), Texture));
                            texture.AddSide(new[] { CuboidSide.Top, CuboidSide.Bottom },
                                new GeometryTextureRectangle(new Rectangle(112, 32, 4, 4), Texture));
                            var vertices = CuboidComposer.Create(0.07f, 0.65f, 0.07f, texture);

                            VertexTransformer.Rotate(vertices, new Vector3(0f, 0f, MathHelper.PiOver2));
                            VertexTransformer.Offset(vertices, new Vector3(0f, 0.7f, 2.95f));
                            vertexCache.AddRange(vertices);
                            VertexTransformer.Offset(vertices, new Vector3(0f, 0.3f, 0f));
                            vertexCache.AddRange(vertices);
                        }
                    }
                    // side steps
                    if (HasOption("Side Steps"))
                    {
                        // steps
                        {
                            var texture = new GeometryTextureCuboidWrapper();
                            texture.AddSide(new[] { CuboidSide.Front, CuboidSide.Back },
                                new GeometryTextureRectangle(new Rectangle(48, 82, 6, 2), Texture));
                            texture.AddSide(new[] { CuboidSide.Top },
                                new GeometryTextureRectangle(new Rectangle(80, 80, 6, 32), Texture));
                            texture.AddSide(new[] { CuboidSide.Bottom },
                                new GeometryTextureRectangle(new Rectangle(0, 16, 1, 1), Texture));
                            texture.AddSide(new[] { CuboidSide.Left, CuboidSide.Right },
                                new GeometryTextureRectangle(new Rectangle(48, 80, 32, 2), Texture));
                            var vertices = CuboidComposer.Create(0.2f, 0.05f, 1f, texture);

                            VertexTransformer.Offset(vertices, new Vector3(1f, 0.3f, 0.5f));
                            vertexCache.AddRange(vertices);
                            VertexTransformer.Offset(vertices, new Vector3(-2f, 0f, 0f));
                            vertexCache.AddRange(vertices);
                        }
                        // supports left
                        {
                            var vertices = CuboidComposer.Create(0.02f, 0.2f, 0.02f,
                                new GeometryTextureRectangle(new Rectangle(0, 16, 1, 1), Texture));

                            VertexTransformer.Rotate(vertices, new Vector3(0f, 0f, 0.5f));
                            VertexTransformer.Offset(vertices, new Vector3(0.95f, 0.38f, 0.95f));
                            vertexCache.AddRange(vertices);
                            VertexTransformer.Offset(vertices, new Vector3(0f, 0f, -0.9f));
                            vertexCache.AddRange(vertices);
                        }
                        // supports right
                        {
                            var vertices = CuboidComposer.Create(0.02f, 0.2f, 0.02f,
                                new GeometryTextureRectangle(new Rectangle(0, 16, 1, 1), Texture));

                            VertexTransformer.Rotate(vertices, new Vector3(0f, 0f, -0.5f));
                            VertexTransformer.Offset(vertices, new Vector3(-0.95f, 0.38f, 0.95f));
                            vertexCache.AddRange(vertices);
                            VertexTransformer.Offset(vertices, new Vector3(0f, 0f, -0.9f));
                            vertexCache.AddRange(vertices);
                        }
                    }

                    var vertexCacheArr = vertexCache.ToArray();
                    if (tireSize == 0.35f)
                    {
                        VertexTransformer.Offset(vertexCacheArr, new Vector3(0, -0.05f, 0));
                    }

                    _vertexCache.Add(id, vertexCacheArr);
                }

                Geometry.AddVertices(_vertexCache[id]);
            }

            base.CreateGeometry();
        }

        #endregion

        #region configurator

        public override string ModelName => "C15 " + Options.First(o => o.Category == ConfigOption.CATEGORY_ENGINES).Name;
        public override string Description => "Built TOUGH. Built STRONG. Won't last for very long.";
        public override string Manufacturer => "Cord";
        public override int ModelYear => 2018;

        public override PreviewPosition[] PreviewPositions
            => new[]
            {
                new PreviewPosition
                {
                    Name = "Overview Front",
                    Position = new Vector3(4, 2, 5),
                    Yaw = MathHelper.PiOver4,
                    Pitch = -0.1f,
                },
                new PreviewPosition
                {
                    Name = "Overview Side",
                    Position = new Vector3(5, 2, 0.5f),
                    Yaw = MathHelper.PiOver2,
                    Pitch = -0.1f,
                },
                new PreviewPosition
                {
                    Name = "Overview Rear",
                    Position = new Vector3(4.5f, 2, -4.5f),
                    Yaw = MathHelper.Pi * (3 / 4f),
                    Pitch = -0.1f,
                },
                new PreviewPosition
                {
                    Name = "Rear",
                    Position = new Vector3(0f, 2, -6f),
                    Yaw = MathHelper.Pi,
                    Pitch = -0.1f,
                },
                new PreviewPosition
                {
                    Name = "Top",
                    Position = new Vector3(0f, 6f, -1f),
                    Yaw = MathHelper.Pi,
                    Pitch = -1.4f,
                },
                new PreviewPosition
                {
                    Name = "Tires",
                    Position = new Vector3(-2f, 0.5f, 3.5f),
                    Yaw = -MathHelper.PiOver4,
                    Pitch = 0.1f,
                },
                new PreviewPosition
                {
                    Name = "Front",
                    Position = new Vector3(0f, 2, 6f),
                    Yaw = 0f,
                    Pitch = -0.1f,
                },
            };

        protected override void PrepareConfigOptions()
        {
            if (_availableConfigOptionCache == null)
            {
                var configs = new List<ConfigOption>();

                configs.AddRange(GenerateConfigs(ConfigOption.CATEGORY_BODY_COLORS, new[]
                {
                    "Shadow Black", "Oxford White", "Ingot Silver", "Race Red",
                    "Blue Jeans", "Magnetic", "Caribou", "Lithium Gray", "Lightning Blue"
                }, true));
                configs.AddRange(GenerateConfigs(ConfigOption.CATEGORY_ENGINES, new[]
                {
                    "2.7L V6","3.5L V6", "5.0L V8"
                }, true));
                configs.AddRange(GenerateConfigs(ConfigOption.CATEGORY_TIRES, new[]
                {
                    "35cm", "40cm"
                }, true));
                configs.AddRange(GenerateConfigs(ConfigOption.CATEGORY_EXTERIOR, new[]
                {
                    "Offroad Trim", "Racing Stripes", "Premium Hatch", "Hood Vents", "Side Bedliner"
                }, false));
                configs.AddRange(GenerateConfigs(ConfigOption.CATEGORY_TECHNOLOGY, new[]
                {
                    "Cab Lights", "Antenna"
                }, false));
                configs.AddRange(GenerateConfigs(ConfigOption.CATEGORY_FUNCTIONALITY, new[]
                {
                    "Side Mirrors", "Spare Tire", "Grille Guard", "Side Steps", "Sport Exhausts"
                }, false));

                _availableConfigOptionCache = configs.ToArray();
            }

            AvailableConfigOptions = _availableConfigOptionCache;
        }

        #endregion
    }
}
