using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using EmergenceWorld.Scripts.Core.Interfaces;

namespace EmergenceWorld.Scripts.Core.Entities
{
    public abstract class Entity : IUpdateable
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public Vector3 Velocity { get; set; }

        public Entity(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            Velocity = new Vector3();
        }

        public virtual void Update(KeyboardState keyboardState, MouseState mouseState, float delta)
        {
            Position += Velocity * delta;
        }
    }
}