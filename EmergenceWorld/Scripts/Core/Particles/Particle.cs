using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using EmergenceWorld.Scripts.Core.Interfaces;
using System.Drawing;

namespace EmergenceWorld.Scripts.Core.Particles
{
    public enum ParticleType
    {
        Empty,
        Dirt,
        Grass,
        Stone
    }

    public class Particle : IUpdateable
    {
        public Vector3i Position { get; }
        public ParticleType Type { get; }

        public Particle(Vector3i position, ParticleType type) 
        {
            Position = position;
            Type = type;
        }

        public void Update(KeyboardState keyboardState, MouseState mouseState, float delta)
        {

        }

        public Color GetColor()
        {
            switch (Type)
            {
                case ParticleType.Empty:
                    throw new Exception("Error: empty particle cannot have color");
                case ParticleType.Dirt:
                    return Color.SaddleBrown;
                case ParticleType.Grass:
                    return Color.LawnGreen;
                case ParticleType.Stone:
                    return Color.Gray;
                default:
                    return new Color();
            }
        }
    }
}
