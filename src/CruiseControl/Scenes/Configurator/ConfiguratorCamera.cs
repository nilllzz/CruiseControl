using GameDevCommon.Rendering;
using Microsoft.Xna.Framework;
using static Core;

namespace CruiseControl.Scenes.Configurator
{
    internal class ConfiguratorCamera : Camera
    {
        private PreviewPosition? _previewPosition;

        public ConfiguratorCamera()
            : base(Controller.GraphicsDevice)
        {
            Yaw = 0;
            Pitch = -0.35f;
            FOV = 60;

            Position = new Vector3(0, 1.1f, 2.5f);

            CreateProjection();
            CreateView();

            FOVChanged += CreateProjection;
        }

        public void ApplyPreviewPosition(PreviewPosition position)
        {
            _previewPosition = position;
        }

        public override void Update()
        {
            const float speed = 0.3f;

            if (_previewPosition != null)
            {
                var pp = _previewPosition.Value;
                if (Position != pp.Position)
                {
                    Position = new Vector3
                    {
                        X = MathHelper.Lerp(Position.X, pp.Position.X, speed),
                        Y = MathHelper.Lerp(Position.Y, pp.Position.Y, speed),
                        Z = MathHelper.Lerp(Position.Z, pp.Position.Z, speed),
                    };
                }
                if (Yaw != pp.Yaw)
                {
                    Yaw = MathHelper.Lerp(Yaw, pp.Yaw, speed);
                }
                if (Pitch != pp.Pitch)
                {
                    Pitch = MathHelper.Lerp(Pitch, pp.Pitch, speed);
                }

                CreateView();
            }
        }
    }
}
