using CruiseControl.GameWorld.Objects.Vehicles;
using GameDevCommon.Rendering;
using Microsoft.Xna.Framework;
using static Core;

namespace CruiseControl.Scenes.InGame
{
    class FollowCamera : Camera
    {
        public Vehicle FollowVehicle { get; set; }

        public FollowCamera()
            : base(Controller.GraphicsDevice)
        {
            CreateProjection();
            FOVChanged += CreateProjection;
            Pitch = -0.2f;

            Update();
        }
        
        public override void Update()
        {
            if (FollowVehicle != null)
            {
                Pitch = -(0.17f * FollowVehicle.ThirdPersonCameraOffset.Y);
                Yaw = FollowVehicle.Rotation.Y + MathHelper.Pi;
                Position = FollowVehicle.Position + 
                    Vector3.Transform(FollowVehicle.ThirdPersonCameraOffset, Matrix.CreateRotationY(FollowVehicle.Rotation.Y));

                CreateView();
            }
        }
    }
}
