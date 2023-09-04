using EmergenceWorld.Scripts.Core.Interfaces;
using EmergenceWorld.Scripts.Core.Scenes;
using EmergenceWorld.Scripts.Core.WorldGeneration;
using EmergenceWorld.Scripts.Utils;
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

    public class Voxel : IUpdateable
    {
        public Vector3i Position { get; }
        public VoxelType Type { get; set; }
        public World World { get; }
        public Chunk Chunk
        {
            get
            {
                return World.Chunks[Helpers.GetChunkHashCode(Position)];
            }
        }

        public Voxel(Vector3i position, VoxelType type, World world) 
        {
            Position = position;
            Type = type;
            World = world;
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
