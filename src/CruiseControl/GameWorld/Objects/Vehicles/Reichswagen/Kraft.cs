using System;
using CruiseControl.GameWorld.Objects.Vehicles.Wheels;
using CruiseControl.Scenes.Configurator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering.Texture;
using GameDevCommon.Rendering;

namespace CruiseControl.GameWorld.Objects.Vehicles.Reichswagen
{
    class Kraft : Vehicle
    {
        private static Dictionary<string, VertexPositionNormalTexture[]> _vertexCache = new Dictionary<string, VertexPositionNormalTexture[]>();
        private static ConfigOption[] _availableConfigOptionCache;

        public override Vector3 ThirdPersonCameraOffset => new Vector3(0, 2, -3f);

        public Kraft(IObjectContainer parent, Vector3 position)
            : this(parent, position, null)
        { }

        public Kraft(IObjectContainer parent, Vector3 position, ConfigOption[] options)
            : base(parent, position, Vector3.Zero, options)
        {
            var tireSize = GetTireSize();
            AddWheel(new ReichswagenWheel(_container, this, new Vector3(0.7f, 0, 1.3f), WheelType.FrontLeft, tireSize));
            AddWheel(new ReichswagenWheel(_container, this, new Vector3(-0.7f, 0, 1.3f), WheelType.FrontRight, tireSize));
            AddWheel(new ReichswagenWheel(_container, this, new Vector3(0.7f, 0, -1.3f), WheelType.RearLeft, tireSize));
            AddWheel(new ReichswagenWheel(_container, this, new Vector3(-0.7f, 0, -1.3f), WheelType.RearRight, tireSize));
        }

        private float GetTireSize()
        {
            return 0.3f;
        }

        public override void LoadContent()
        {
            var colorConfig = Options.FirstOrDefault(o => o.Category == ConfigOption.CATEGORY_BODY_COLORS);
            var color = VehicleColorProvider.GetColor(colorConfig.Name);
            SetTexture("Textures/Vehicles/Reichswagen/rw_kraft.png", color);
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

                    // lower front grill
                    {
                        // front
                        {
                            var vertices = RectangleComposer.Create(1.2f, 0.3f,
                                new GeometryTextureRectangle(new Rectangle(0, 16, 48, 16), Texture));
                            VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, 0f, 0f));
                            VertexTransformer.Offset(vertices, new Vector3(0f, 0.3f, 2.1f));
                            vertexCache.AddRange(vertices);
                        }
                        // left
                        {
                            var vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(0.6f, 0.45f, 2.1f),
                                new Vector3(0.8f, 0.45f, 1.9f),
                                new Vector3(0.6f, 0.15f, 2.1f),
                                new Vector3(0.8f, 0.15f, 1.9f),
                            }, new GeometryTextureRectangle(new Rectangle(48, 16, 16, 16), Texture));
                            vertexCache.AddRange(vertices);
                        }
                        // right
                        {
                            var vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(-0.6f, 0.45f, 2.1f),
                                new Vector3(-0.8f, 0.45f, 1.9f),
                                new Vector3(-0.6f, 0.15f, 2.1f),
                                new Vector3(-0.8f, 0.15f, 1.9f),
                            }, new GeometryTextureRectangle(new Rectangle(48, 16, 16, 16), Texture));
                            vertexCache.AddRange(vertices);
                        }
                        // top front
                        {
                            var vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(-0.6f, 0.5f, 2.05f),
                                new Vector3(0.6f, 0.5f, 2.05f),
                                new Vector3(-0.6f, 0.45f, 2.1f),
                                new Vector3(0.6f, 0.45f, 2.1f),
                            }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                            vertexCache.AddRange(vertices);
                        }
                        // top left
                        {
                            var vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(0.6f, 0.5f, 2.05f),
                                new Vector3(0.8f, 0.5f, 1.9f),
                                new Vector3(0.6f, 0.45f, 2.1f),
                                new Vector3(0.8f, 0.45f, 1.9f),
                            }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                            vertexCache.AddRange(vertices);
                        }
                        // top right
                        {
                            var vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(-0.6f, 0.5f, 2.05f),
                                new Vector3(-0.8f, 0.5f, 1.9f),
                                new Vector3(-0.6f, 0.45f, 2.1f),
                                new Vector3(-0.8f, 0.45f, 1.9f),
                            }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                            vertexCache.AddRange(vertices);
                        }
                    }
                    // upper front grill
                    {
                        var vertices = RectangleComposer.Create(new[]
                        {
                            new Vector3(-0.5f, 0.55f, 2f),
                            new Vector3(0.5f, 0.55f, 2f),
                            new Vector3(-0.45f, 0.5f, 2.05f),
                            new Vector3(0.45f, 0.5f, 2.05f),
                        }, new GeometryTextureRectangle(new Rectangle(0, 32, 32, 5), Texture));
                        vertexCache.AddRange(vertices);
                    }
                    // front logo
                    {
                        var vertices = CylinderComposer.Create(0.0675f, 0.025f, 10,
                            new GeometryTextureRectangle(new Rectangle(0, 0, 1, 1), Texture),
                            new GeometryTextureRectangle(new Rectangle(0, 0, 16, 16), Texture));
                        VertexTransformer.Rotate(vertices, new Vector3(0f, MathHelper.PiOver2, 0f));
                        VertexTransformer.Rotate(vertices, new Vector3(-MathHelper.PiOver4, 0f, 0f));
                        VertexTransformer.Offset(vertices, new Vector3(0f, 0.525f, 2.025f));
                        vertexCache.AddRange(vertices);
                    }
                    // hood
                    {
                        // main
                        {
                            var vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(-0.7f, 0.75f, 1.2f),
                                new Vector3(0.7f, 0.75f, 1.2f),
                                new Vector3(-0.5f, 0.55f, 2f),
                                new Vector3(0.5f, 0.55f, 2f),
                            }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                            vertexCache.AddRange(vertices);
                        }
                        // side blades
                        {
                            // left
                            {
                                var vertices = RectangleComposer.Create(new[]
                                {
                                    new Vector3(0.7f, 0.75f, 1.2f),
                                    new Vector3(0.8f, 0.7f, 1.2f),
                                    new Vector3(0.5f, 0.55f, 2f),
                                    new Vector3(0.8f, 0.6f, 1.8f),
                                }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                                vertexCache.AddRange(vertices);
                            }
                            // right
                            {
                                var vertices = RectangleComposer.Create(new[]
                                {
                                    new Vector3(-0.7f, 0.75f, 1.2f),
                                    new Vector3(-0.8f, 0.7f, 1.2f),
                                    new Vector3(-0.5f, 0.55f, 2f),
                                    new Vector3(-0.8f, 0.6f, 1.8f),
                                }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                                vertexCache.AddRange(vertices);
                            }
                        }
                    }
                    // front lights
                    {
                        // left
                        {
                            var vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(0.5f, 0.55f, 2f),
                                new Vector3(0.6f, 0.566666667f, 1.93333333f),
                                new Vector3(0.45f, 0.5f, 2.05f),
                                new Vector3(0.6f, 0.5f, 2.05f),
                            }, new GeometryTextureRectangle(new Rectangle(32, 32, 13, 10), Texture));
                            vertexCache.AddRange(vertices);
                            vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(0.6f, 0.566666667f, 1.93333333f),
                                new Vector3(0.8f, 0.6f, 1.8f),
                                new Vector3(0.6f, 0.5f, 2.05f),
                                new Vector3(0.8f, 0.5f, 1.9f),
                            }, new GeometryTextureRectangle(new Rectangle(45, 32, 16, 10), Texture));
                            vertexCache.AddRange(vertices);
                        }
                        //right
                        {
                            var vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(-0.5f, 0.55f, 2f),
                                new Vector3(-0.6f, 0.566666667f, 1.93333333f),
                                new Vector3(-0.45f, 0.5f, 2.05f),
                                new Vector3(-0.6f, 0.5f, 2.05f),
                            }, new GeometryTextureRectangle(new Rectangle(32, 32, 13, 10), Texture));
                            vertexCache.AddRange(vertices);
                            vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(-0.6f, 0.566666667f, 1.93333333f),
                                new Vector3(-0.8f, 0.6f, 1.8f),
                                new Vector3(-0.6f, 0.5f, 2.05f),
                                new Vector3(-0.8f, 0.5f, 1.9f),
                            }, new GeometryTextureRectangle(new Rectangle(45, 32, 16, 10), Texture));
                            vertexCache.AddRange(vertices);
                        }
                    }
                    // front wheel arches
                    {
                        // left
                        {
                            var vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(0.8f, 0.5f, 1.9f),
                                new Vector3(0.8f, 0.53f, 1.725f),
                                new Vector3(0.8f, 0.15f, 1.9f),
                                new Vector3(0.8f, 0.15f, 1.875f),
                            }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                            vertexCache.AddRange(vertices);
                            vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(0.8f, 0.7f, 1.2f),
                                new Vector3(0.8f, 0.6f, 1.8f),
                                new Vector3(0.8f, 0.6f, 1.2f),
                                new Vector3(0.8f, 0.5f, 1.9f),
                            }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                            vertexCache.AddRange(vertices);
                            vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(0.8f, 0.7f, 1.15f),
                                new Vector3(0.8f, 0.7f, 1.2f),
                                new Vector3(0.8f, 0.15f, 1.15f),
                                new Vector3(0.8f, 0.6f, 1.25f),
                            }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                            vertexCache.AddRange(vertices);
                        }
                        // right
                        {
                            var vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(-0.8f, 0.5f, 1.9f),
                                new Vector3(-0.8f, 0.53f, 1.725f),
                                new Vector3(-0.8f, 0.15f, 1.9f),
                                new Vector3(-0.8f, 0.15f, 1.875f),
                            }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                            vertexCache.AddRange(vertices);
                            vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(-0.8f, 0.7f, 1.2f),
                                new Vector3(-0.8f, 0.6f, 1.8f),
                                new Vector3(-0.8f, 0.6f, 1.2f),
                                new Vector3(-0.8f, 0.5f, 1.9f),
                            }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                            vertexCache.AddRange(vertices);
                            vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(-0.8f, 0.7f, 1.15f),
                                new Vector3(-0.8f, 0.7f, 1.2f),
                                new Vector3(-0.8f, 0.15f, 1.15f),
                                new Vector3(-0.8f, 0.6f, 1.25f),
                            }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                            vertexCache.AddRange(vertices);
                        }
                    }
                    // windshield
                    {
                        var vertices = RectangleComposer.Create(new[]
                        {
                            new Vector3(-0.55f, 1.2f, 0.6f),
                            new Vector3(0.55f, 1.2f, 0.6f),
                            new Vector3(-0.7f, 0.75f, 1.2f),
                            new Vector3(0.7f, 0.75f, 1.2f),
                        }, new GeometryTextureRectangle(new Rectangle(0, 48, 48, 32), Texture));
                        vertexCache.AddRange(vertices);
                    }
                    // a pillars
                    {
                        // left
                        {
                            var vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(0.55f, 1.2f, 0.6f),
                                new Vector3(0.6f, 1.2f, 0.6f),
                                new Vector3(0.7f, 0.75f, 1.2f),
                                new Vector3(0.8f, 0.7f, 1.2f),
                            }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                            vertexCache.AddRange(vertices);
                        }
                        // right
                        {
                            var vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(-0.55f, 1.2f, 0.6f),
                                new Vector3(-0.6f, 1.2f, 0.6f),
                                new Vector3(-0.7f, 0.75f, 1.2f),
                                new Vector3(-0.8f, 0.7f, 1.2f),
                            }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                            vertexCache.AddRange(vertices);
                        }
                    }
                    // side front windows
                    {
                        // left
                        {
                            var vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(0.6f, 1.2f, 0.6f),
                                new Vector3(0.6f, 1.15f, 0f),
                                new Vector3(0.8f, 0.7f, 1.2f),
                                new Vector3(0.8f, 0.7f, 0f),
                            }, new GeometryTextureRectangle(new Rectangle(48, 48, 48, 32), Texture));
                            vertexCache.AddRange(vertices);
                        }
                        // right
                        {
                            var vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(-0.6f, 1.2f, 0.6f),
                                new Vector3(-0.6f, 1.15f, 0f),
                                new Vector3(-0.8f, 0.7f, 1.2f),
                                new Vector3(-0.8f, 0.7f, 0f),
                            }, new GeometryTextureRectangle(new Rectangle(48, 48, 48, 32), Texture));
                            vertexCache.AddRange(vertices);
                        }
                    }
                    // doors
                    {
                        // left
                        {
                            var vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(0.8f, 0.7f, 1.15f),
                                new Vector3(0.8f, 0.7f, 0f),
                                new Vector3(0.8f, 0.15f, 1.15f),
                                new Vector3(0.8f, 0.15f, 0f),
                            }, new GeometryTextureRectangle(new Rectangle(0, 80, 48, 32), Texture));
                            vertexCache.AddRange(vertices);
                            vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(0.8f, 0.7f, 0f),
                                new Vector3(0.8f, 0.7f, -1f),
                                new Vector3(0.8f, 0.15f, 0f),
                                new Vector3(0.8f, 0.15f, -0.6f),
                            }, new GeometryTextureRectangle(new Rectangle(16, 80, 32, 32), Texture));
                            vertexCache.AddRange(vertices);
                        }
                        // right
                        {
                            var vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(-0.8f, 0.7f, 1.15f),
                                new Vector3(-0.8f, 0.7f, 0f),
                                new Vector3(-0.8f, 0.15f, 1.15f),
                                new Vector3(-0.8f, 0.15f, 0f),
                            }, new GeometryTextureRectangle(new Rectangle(0, 80, 48, 32), Texture));
                            vertexCache.AddRange(vertices);
                            vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(-0.8f, 0.7f, 0f),
                                new Vector3(-0.8f, 0.7f, -1f),
                                new Vector3(-0.8f, 0.15f, 0f),
                                new Vector3(-0.8f, 0.15f, -0.6f),
                            }, new GeometryTextureRectangle(new Rectangle(16, 80, 32, 32), Texture));
                            vertexCache.AddRange(vertices);
                        }
                    }
                    // rear side windows
                    {
                        // left
                        {
                            var vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(0.6f, 1.15f, 0f),
                                new Vector3(0.6f, 1.1f, -1f),
                                new Vector3(0.8f, 0.7f, 0f),
                                new Vector3(0.8f, 0.7f, -1f),
                            }, new GeometryTextureRectangle(new Rectangle(96, 48, 48, 32), Texture));
                            vertexCache.AddRange(vertices);
                        }
                        // right
                        {
                            var vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(-0.6f, 1.15f, 0f),
                                new Vector3(-0.6f, 1.1f, -1f),
                                new Vector3(-0.8f, 0.7f, 0f),
                                new Vector3(-0.8f, 0.7f, -1f),
                            }, new GeometryTextureRectangle(new Rectangle(96, 48, 48, 32), Texture));
                            vertexCache.AddRange(vertices);
                        }
                    }
                    // roof
                    {
                        var vertices = RectangleComposer.Create(new[]
                        {
                            new Vector3(0.6f, 1.15f, 0f),
                            new Vector3(-0.6f, 1.15f, 0f),
                            new Vector3(0.6f, 1.2f, 0.6f),
                            new Vector3(-0.6f, 1.2f, 0.6f),
                        }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                        vertexCache.AddRange(vertices);
                        vertices = RectangleComposer.Create(new[]
                        {
                            new Vector3(0.6f, 1.1f, -1f),
                            new Vector3(-0.6f, 1.1f, -1f),
                            new Vector3(0.6f, 1.15f, 0f),
                            new Vector3(-0.6f, 1.15f, 0f),
                        }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                        vertexCache.AddRange(vertices);
                        vertices = RectangleComposer.Create(new[]
                        {
                            new Vector3(0.6f, 1.1f, -1f),
                            new Vector3(-0.6f, 1.1f, -1f),
                            new Vector3(0.6f, 1.1f, -1.2f),
                            new Vector3(-0.6f, 1.1f, -1.2f),
                        }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                        vertexCache.AddRange(vertices);
                    }
                    // hatch
                    {
                        // upper
                        {
                            var vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(0.6f, 1.1f, -1.2f),
                                new Vector3(-0.6f, 1.1f, -1.2f),
                                new Vector3(0.8f, 0.7f, -1.5f),
                                new Vector3(-0.8f, 0.7f, -1.5f),
                            }, new GeometryTextureRectangle(new Rectangle(64, 0, 48, 32), Texture));
                            vertexCache.AddRange(vertices);
                        }
                        // lower
                        {
                            var vertices = RectangleComposer.Create(new[]
                            {
                                new Vector3(0.8f, 0.7f, -1.5f),
                                new Vector3(-0.8f, 0.7f, -1.5f),
                                new Vector3(0.7f, 0.15f, -1.55f),
                                new Vector3(-0.7f, 0.15f, -1.55f),
                            }, new GeometryTextureRectangle(new Rectangle(112, 0, 48, 32), Texture));
                            vertexCache.AddRange(vertices);
                        }
                        // logo
                        {
                            var vertices = CylinderComposer.Create(0.0675f, 0.025f, 10,
                                new GeometryTextureRectangle(new Rectangle(0, 0, 1, 1), Texture),
                                new GeometryTextureRectangle(new Rectangle(0, 0, 16, 16), Texture));
                            VertexTransformer.Rotate(vertices, new Vector3(0f, -MathHelper.PiOver2, 0f));
                            VertexTransformer.Offset(vertices, new Vector3(0f, 0.525f, -1.52f));
                            vertexCache.AddRange(vertices);
                        }
                    }
                    // c pillars
                    {
                        // upper
                        {
                            // left
                            {
                                var vertices = RectangleComposer.Create(new[]
                                {
                                    new Vector3(0.6f, 1.1f, -1.2f),
                                    new Vector3(0.6f, 1.1f, -1f),
                                    new Vector3(0.8f, 0.7f, -1.5f),
                                    new Vector3(0.8f, 0.7f, -1f),
                                }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                                vertexCache.AddRange(vertices);
                            }
                            // right
                            {
                                var vertices = RectangleComposer.Create(new[]
                                {
                                    new Vector3(-0.6f, 1.1f, -1.2f),
                                    new Vector3(-0.6f, 1.1f, -1f),
                                    new Vector3(-0.8f, 0.7f, -1.5f),
                                    new Vector3(-0.8f, 0.7f, -1f),
                                }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                                vertexCache.AddRange(vertices);
                            }
                        }
                        // lower
                        {
                            // left
                            {
                                var vertices = RectangleComposer.Create(new[]
                                {
                                    new Vector3(0.8f, 0.7f, -1.5f),
                                    new Vector3(0.8f, 0.7f, -1.3f),
                                    new Vector3(0.7f, 0.15f, -1.55f),
                                    new Vector3(0.7f, 0.15f, -1.5f),
                                }, new GeometryTextureRectangle(new Rectangle(160, 0, 4, 32), Texture));
                                vertexCache.AddRange(vertices);
                                vertices = RectangleComposer.Create(new[]
                                {
                                    new Vector3(0.8f, 0.7f, -1.5f),
                                    new Vector3(0.8f, 0.7f, -1f),
                                    new Vector3(0.775f, 0.575f, -1.5f),
                                    new Vector3(0.8f, 0.575f, -0.9f),
                                }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                                vertexCache.AddRange(vertices);
                            }
                            // right
                            {
                                var vertices = RectangleComposer.Create(new[]
                                {
                                    new Vector3(-0.8f, 0.7f, -1.5f),
                                    new Vector3(-0.8f, 0.7f, -1.3f),
                                    new Vector3(-0.7f, 0.15f, -1.55f),
                                    new Vector3(-0.7f, 0.15f, -1.5f),
                                }, new GeometryTextureRectangle(new Rectangle(160, 0, 4, 32), Texture));
                                vertexCache.AddRange(vertices);
                                vertices = RectangleComposer.Create(new[]
                                {
                                    new Vector3(-0.8f, 0.7f, -1.5f),
                                    new Vector3(-0.8f, 0.7f, -1f),
                                    new Vector3(-0.775f, 0.575f, -1.5f),
                                    new Vector3(-0.8f, 0.575f, -0.9f),
                                }, new GeometryTextureRectangle(new Rectangle(16, 0, 1, 1), Texture));
                                vertexCache.AddRange(vertices);
                            }
                        }
                    }
                    // floor
                    {
                        var vertices = RectangleComposer.Create(new[]
                        {
                            new Vector3(0.8f, 0.53f, 1.725f),
                            new Vector3(-0.8f, 0.53f, 1.725f),
                            new Vector3(0.8f, 0.15f, 1.875f),
                            new Vector3(-0.8f, 0.15f, 1.875f),
                        }, new GeometryTextureRectangle(new Rectangle(48, 96, 64, 16), Texture));
                        vertexCache.AddRange(vertices);
                        vertices = RectangleComposer.Create(new[]
                        {
                            new Vector3(0.8f, 0.53f, 1.725f),
                            new Vector3(-0.8f, 0.53f, 1.725f),
                            new Vector3(0.8f, 0.6f, 1.2f),
                            new Vector3(-0.8f, 0.6f, 1.2f),
                        }, new GeometryTextureRectangle(new Rectangle(48, 96, 64, 16), Texture));
                        vertexCache.AddRange(vertices);
                        vertices = RectangleComposer.Create(new[]
                        {
                            new Vector3(0.8f, 0.6f, 1.2f),
                            new Vector3(-0.8f, 0.6f, 1.2f),
                            new Vector3(0.8f, 0.15f, 1.15f),
                            new Vector3(-0.8f, 0.15f, 1.15f),
                        }, new GeometryTextureRectangle(new Rectangle(48, 96, 64, 16), Texture));
                        vertexCache.AddRange(vertices);
                        vertices = RectangleComposer.Create(new[]
                        {
                            new Vector3(0.8f, 0.15f, 1.15f),
                            new Vector3(-0.8f, 0.15f, 1.15f),
                            new Vector3(0.8f, 0.15f, -0.6f),
                            new Vector3(-0.8f, 0.15f, -0.6f),
                        }, new GeometryTextureRectangle(new Rectangle(48, 80, 48, 48), Texture));
                        vertexCache.AddRange(vertices);
                        vertices = RectangleComposer.Create(new[]
                        {
                            new Vector3(0.8f, 0.15f, -0.6f),
                            new Vector3(-0.8f, 0.15f, -0.6f),
                            new Vector3(0.8f, 0.575f, -0.909f),
                            new Vector3(-0.8f, 0.575f, -0.909f),
                        }, new GeometryTextureRectangle(new Rectangle(48, 96, 64, 16), Texture));
                        vertexCache.AddRange(vertices);
                        vertices = RectangleComposer.Create(new[]
                        {
                            new Vector3(0.8f, 0.575f, -0.909f),
                            new Vector3(-0.8f, 0.575f, -0.909f),
                            new Vector3(0.775f, 0.575f, -1.35f),
                            new Vector3(-0.775f, 0.575f, -1.35f),
                        }, new GeometryTextureRectangle(new Rectangle(48, 96, 64, 16), Texture));
                        vertexCache.AddRange(vertices);
                        vertices = RectangleComposer.Create(new[]
                        {
                            new Vector3(0.775f, 0.575f, -1.35f),
                            new Vector3(-0.775f, 0.575f, -1.35f),
                            new Vector3(0.7f, 0.15f, -1.5f),
                            new Vector3(-0.7f, 0.15f, -1.5f),
                        }, new GeometryTextureRectangle(new Rectangle(48, 96, 64, 16), Texture));
                        vertexCache.AddRange(vertices);
                    }

                    var vertexCacheArr = vertexCache.ToArray();
                    if (true)
                    {
                        VertexTransformer.Offset(vertexCacheArr, new Vector3(0, 0.05f, -0.2f));
                    }
                    _vertexCache.Add(id, vertexCacheArr);
                }

                Geometry.AddVertices(_vertexCache[id]);
            }

            base.CreateGeometry();
        }

        #endregion

        #region configurator

        public override string ModelName => "Kraft";
        public override string Description => "This hatchback invades countries by storm.";
        public override string Manufacturer => "Reichswagen";
        public override int ModelYear => 2013;

        protected override void PrepareConfigOptions()
        {
            if (_availableConfigOptionCache == null)
            {
                var configs = new List<ConfigOption>();

                configs.AddRange(GenerateConfigs(ConfigOption.CATEGORY_BODY_COLORS, new[]
                {
                    "Platinum Gray", "Pure White", "Night Blue", "Tornado Red", "Black", "Silk Blue", "Tungsten Silver"
                }, true));

                _availableConfigOptionCache = configs.ToArray();
            }

            AvailableConfigOptions = _availableConfigOptionCache;
        }

        public override PreviewPosition[] PreviewPositions
            => new[]
            {
                new PreviewPosition
                {
                    Name = "Overview Front",
                    Position = new Vector3(2.5f, 1.5f, 3f),
                    Yaw = MathHelper.PiOver4,
                    Pitch = -0.2f,
                },
                new PreviewPosition
                {
                    Name = "Overview Side",
                    Position = new Vector3(3, 1.5f, 0.5f),
                    Yaw = MathHelper.PiOver2,
                    Pitch = -0.2f,
                },
                new PreviewPosition
                {
                    Name = "Overview Rear",
                    Position = new Vector3(2.5f, 1.5f, -3f),
                    Yaw = MathHelper.Pi * (3 / 4f),
                    Pitch = -0.2f,
                },
                new PreviewPosition
                {
                    Name = "Rear",
                    Position = new Vector3(0f, 1.5f, -4f),
                    Yaw = MathHelper.Pi,
                    Pitch = -0.1f,
                },
                new PreviewPosition
                {
                    Name = "Top",
                    Position = new Vector3(0f, 4f, -0.6f),
                    Yaw = MathHelper.Pi,
                    Pitch = -1.4f,
                },
                new PreviewPosition
                {
                    Name = "Tires",
                    Position = new Vector3(-1.2f, 0.3f, 2f),
                    Yaw = -MathHelper.PiOver4,
                    Pitch = 0.1f,
                },
                new PreviewPosition
                {
                    Name = "Front",
                    Position = new Vector3(0f, 1.5f, 3.5f),
                    Yaw = 0f,
                    Pitch = -0.3f,
                },
            };

        #endregion
    }
}
