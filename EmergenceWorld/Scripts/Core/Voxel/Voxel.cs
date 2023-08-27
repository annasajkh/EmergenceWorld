using EmergenceWorld.Scripts.Core.Interfaces;
using EmergenceWorld.Scripts.Core.WorldGeneration;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;

namespace EmergenceWorld.Scripts.Core.Voxels
{
    public enum VoxelType
    {
        Air,
        Dirt,
        Grass,
        Stone,
        Water,
        Sand
    }

    public enum VoxelSide
    {
        TopAndBottom,
        LeftAndRight,
        FrontAndBack
    }

    public class Voxel : IUpdateable
    {
        public Vector3i Position { get; }
        public int i { get; }
        public int j { get; }
        public int k { get; }
        public VoxelType Type { get; set; }
        public Chunk Chunk { get; private set; }



        public Voxel(Vector3i position, int i, int j, int k, VoxelType type, Chunk chunk) 
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
            if (Chunk.IsVoxel(VoxelType.Air, iParams, jParams, kParams))
            {
                VoxelType particleType = Type;

                Chunk.Voxels[i, j, k].Type = VoxelType.Air;
                Chunk.Voxels[iParams, jParams, kParams].Type = particleType;

                return true;
            }

            return false;
        }

        public void Update(KeyboardState keyboardState, MouseState mouseState, float delta)
        {
            
        }

        public Color GetColor()
        {
            switch (Type)
            {
                case VoxelType.Air:
                    throw new Exception("Error: Air Voxel cannot have color");
                case VoxelType.Dirt:
                    return Color.SaddleBrown;
                case VoxelType.Grass:
                    return Color.LawnGreen;
                case VoxelType.Stone:
                    return Color.Gray;
                case VoxelType.Water:
                    return Color.Blue;
                case VoxelType.Sand:
                    return Color.SandyBrown;
                default:
                    return new Color();
            }
        }
    }
}
