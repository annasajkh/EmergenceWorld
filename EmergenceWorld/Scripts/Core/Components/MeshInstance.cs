using EmergenceWorld.Scripts.Core.Containers;
using OpenTK.Mathematics;

namespace EmergenceWorld.Scripts.Core.Components
{
    public class MeshInstance
    {
        public static MeshInstance Quad { get; } = new MeshInstance(

        new Vertex[]
        {
            new Vertex(new Vector3(0.5f,  0.5f, 0.0f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(0.5f, -0.5f, 0.0f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(-0.5f, -0.5f, 0.0f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(-0.5f,  0.5f, 0.0f), new Color4(1f, 1f, 1f, 1f))
        },

        new uint[]
        {
            0, 1, 3, 
            1, 2, 3
        });

        public static MeshInstance Cube { get; } = new MeshInstance(

        new Vertex[]
        {
            // front
            new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(0.5f, -0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(0.5f,  0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f)),

            // back
            new Vertex(new Vector3(0.5f, 0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(0.5f, -0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(-0.5f,  -0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f)),

            // left
            new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f)),

            // right
            new Vertex(new Vector3(0.5f, -0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(0.5f,  0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(0.5f,  0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(0.5f, -0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f)),

            // top
            new Vertex(new Vector3(0.5f,  0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(0.5f,  0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f)),

            // bottom
            new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(0.5f, -0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(0.5f, -0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f)),
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


        public Vertex[] Vertices { get; }
        public uint[] Indices { get; }

        public MeshInstance(Vertex[] vertices, uint[] indices)
        {
            Vertices = vertices;
            Indices = indices;
        }
    }
}