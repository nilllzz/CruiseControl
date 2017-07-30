using GameDevCommon.Input;
using GameDevCommon.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static Core;

namespace CruiseControl.Scenes.InGame
{
    internal class OverviewCamera : Camera
    {
        public OverviewCamera()
            : base(Controller.GraphicsDevice)
        {
            Yaw = 0;
            Pitch = -1.5f;
            FOV = 90;
            FarPlane = 10000;

            Position = new Vector3(0, 2, 0);

            CreateProjection();
            CreateView();

            FOVChanged += CreateProjection;
        }

        public override void Update()
        {
            var kboard = GetComponent<KeyboardHandler>();
            var rot = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0f);

            const float speed = 1f;
            const float turnSpeed = 0.05f;

            if (kboard.KeyDown(Keys.W))
                Position += Vector3.Transform(Vector3.Forward * speed, rot);
            if (kboard.KeyDown(Keys.A))
                Position += Vector3.Transform(Vector3.Left * speed, rot);
            if (kboard.KeyDown(Keys.D))
                Position += Vector3.Transform(Vector3.Right * speed, rot);
            if (kboard.KeyDown(Keys.S))
                Position += Vector3.Transform(Vector3.Backward * speed, rot);

            if (kboard.KeyDown(Keys.Up))
                Pitch += turnSpeed;
            if (kboard.KeyDown(Keys.Down))
                Pitch -= turnSpeed;
            if (kboard.KeyDown(Keys.Left))
                Yaw += turnSpeed;
            if (kboard.KeyDown(Keys.Right))
                Yaw -= turnSpeed;

            CreateView();
        }
    }
}
