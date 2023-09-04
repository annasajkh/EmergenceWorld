using EmergenceWorld.Scripts.Core.Containers;
using OpenTK.Mathematics;

namespace EmergenceWorld.Scripts.Core.Components
{
    public class VoxelMeshInstance
    {
        public static VoxelMeshInstance VoxelQuad { get; } = new VoxelMeshInstance(

        new VoxelVertex[]
        {
            new VoxelVertex(new Vector3(0.5f,  0.5f, 0.0f), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(0.5f, -0.5f, 0.0f), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(-0.5f, -0.5f, 0.0f), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(-0.5f,  0.5f, 0.0f), new Color4(1f, 1f, 1f, 1f))
        },

        new uint[]
        {
            0, 1, 3, 
            1, 2, 3
        });

        public static VoxelMeshInstance VoxelCube { get; } = new VoxelMeshInstance(

        new VoxelVertex[]
        {
            // front
            new VoxelVertex(new Vector3(0, 0, 1), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(1, 0, 1), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(1, 1, 1), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(0, 1, 1), new Color4(1f, 1f, 1f, 1f)),

            // back
            new VoxelVertex(new Vector3(1, 1, 0), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(1, 0, 0), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(0, 0, 0), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(0, 1, 0), new Color4(1f, 1f, 1f, 1f)),

            // left
            new VoxelVertex(new Vector3(0, 1, 0), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(0, 0, 0), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(0, 0,  1), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(0, 1,  1), new Color4(1f, 1f, 1f, 1f)),

            // right
            new VoxelVertex(new Vector3(1, 0, 0), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(1, 1, 0), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(1, 1, 1), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(1, 0, 1), new Color4(1f, 1f, 1f, 1f)),

            // top
            new VoxelVertex(new Vector3(1, 1, 0), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(0, 1, 0), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(0, 1, 1), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(1, 1, 1), new Color4(1f, 1f, 1f, 1f)),

            // bottom
            new VoxelVertex(new Vector3(0, 0, 0), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(1, 0, 0), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(1, 0, 1), new Color4(1f, 1f, 1f, 1f)),
            new VoxelVertex(new Vector3(0, 0, 1), new Color4(1f, 1f, 1f, 1f)),
        },

        new uint[]
        {
            0, 1, 3, 
            1, 2, 3, 
            
            4, 5, 7, 
            5, 6, 7, 
            
            8, 9, 11, 
            9, 10, 11, 
            
            12, 13, 15, 
            13, 14, 15, 
            
            16, 17, 19, 
            17, 18, 19, 
            
            20, 21, 23, 
            21, 22, 23,
        });


        public VoxelVertex[] Vertices { get; }
        public uint[] Indices { get; }

        public VoxelMeshInstance(VoxelVertex[] vertices, uint[] indices)
        {
            Vertices = vertices;
            Indices = indices;
        }
    }
}