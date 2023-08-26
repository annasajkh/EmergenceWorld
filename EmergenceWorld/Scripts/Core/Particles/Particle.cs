using EmergenceWorld.Scripts.Core.Interfaces;
using EmergenceWorld.Scripts.Core.WorldGeneration;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;

namespace EmergenceWorld.Scripts.Core.Particles
{
    public enum ParticleType
    {
        Air,
        Dirt,
        Grass,
        Stone,
        Water,
        Sand
    }

    public enum ParticleSide
    {
        TopAndBottom,
        LeftAndRight,
        FrontAndBack
    }

    public class Particle : IUpdateable
    {
        public Vector3i Position { get; }
        public int i { get; }
        public int j { get; }
        public int k { get; }
        public ParticleType Type { get; set; }
        public Chunk Chunk { get; private set; }



        public Particle(Vector3i position, int i, int j, int k, ParticleType type, Chunk chunk) 
        {
            Position = position;
            Type = type;
            Chunk = chunk;

            this.i = i;
            this.j = j;
            this.k = k;
        }

        public bool Move(int iParams, int jParams, int kParams)
        {
            if (Chunk.IsParticle(ParticleType.Air, iParams, jParams, kParams))
            {
                ParticleType particleType = Type;

                Chunk.Particles[i, j, k].Type = ParticleType.Air;
                Chunk.Particles[iParams, jParams, kParams].Type = particleType;

                Chunk.IsParticleMove = true;

                return true;
            }

            return false;
        }

        public void Update(KeyboardState keyboardState, MouseState mouseState, float delta)
        {
            switch (Type)
            {
                case ParticleType.Air:
                    break;

                case ParticleType.Dirt:
                    break;

                case ParticleType.Grass:
                    break;

                case ParticleType.Stone:
                    break;

                case ParticleType.Water:
                    // down movement
                    if (Move(i, j - 1, k))
                    {

                    }
                    else if (Move(i + 1, j - 1, k))
                    {

                    }
                    else if (Move(i - 1, j - 1, k))
                    {

                    }
                    else if (Move(i, j - 1, k + 1))
                    {

                    }
                    else if (Move(i, j - 1, k - 1))
                    {

                    }
                    else if (Move(i + 1, j - 1, k + 1))
                    {

                    }
                    else if (Move(i - 1, j - 1, k - 1))
                    {

                    }
                    else if (Move(i - 1, j - 1, k + 1))
                    {

                    }
                    else if (Move(i + 1, j - 1, k - 1))
                    {

                    }
                    // side movement
                    else if (Move(i + Game.Random.Next(-1, 1), j, k + Game.Random.Next(-1, 1)))
                    {

                    }
                    break;

                case ParticleType.Sand:

                    if (Chunk.IsParticle(ParticleType.Water, i, j - 1, k))
                    {
                        ParticleType particleType = Type;

                        Chunk.Particles[i, j, k].Type = Chunk.Particles[i, j - 1, k].Type;
                        Chunk.Particles[i, j - 1, k].Type = particleType;

                        Chunk.IsParticleMove = true;

                        break;
                    }

                    // down movement
                    if (Move(i, j - 1, k))
                    {

                    }
                    else if (Move(i + 1, j - 1, k))
                    {

                    }
                    else if (Move(i - 1, j - 1, k))
                    {

                    }
                    else if (Move(i, j - 1, k + 1))
                    {

                    }
                    else if (Move(i, j - 1, k - 1))
                    {

                    }
                    else if (Move(i + 1, j - 1, k + 1))
                    {

                    }
                    else if (Move(i - 1, j - 1, k - 1))
                    {

                    }
                    else if (Move(i - 1, j - 1, k + 1))
                    {

                    }
                    else if (Move(i + 1, j - 1, k - 1))
                    {

                    }

                    break;

                default:
                    break;
            }
        }

        public Color GetColor()
        {
            switch (Type)
            {
                case ParticleType.Air:
                    throw new Exception("Error: Air Particle cannot have color");
                case ParticleType.Dirt:
                    return Color.SaddleBrown;
                case ParticleType.Grass:
                    return Color.LawnGreen;
                case ParticleType.Stone:
                    return Color.Gray;
                case ParticleType.Water:
                    return Color.Blue;
                case ParticleType.Sand:
                    return Color.SandyBrown;
                default:
                    return new Color();
            }
        }
    }
}
