using OpenTK.Mathematics;
using EmergenceWorld.Scripts.Core.Containers;

namespace EmergenceWorld.Scripts.Core.Components
{
    public class MeshInstance
    {
        public static MeshInstance Quad { get; } = new MeshInstance(

        new Vertex[]
        {
            new Vertex(new Vector3(-1, 0, -1), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2(0, 0)),
            new Vertex(new Vector3(-1, 0, 1), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2(1, 0)),
            new Vertex(new Vector3(1, 0, 1), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2(1, 1)),
            new Vertex(new Vector3(1, 0, -1), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2(0, 1))
        },

        new uint[]
        {
            0, 1, 3,
            1, 2, 3
        });

        public static MeshInstance Cube { get; } = new MeshInstance(

        new Vertex[]
        {
            new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2( 0.0f, 0.0f)),
            new Vertex(new Vector3(0.5f, -0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2(1.0f, 0.0f)),
            new Vertex(new Vector3(0.5f,  0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2(1.0f, 1.0f)),
            new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2( 0.0f, 1.0f)),
            new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2( 0.0f, 0.0f)),
            new Vertex(new Vector3(0.5f, -0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2(1.0f, 0.0f)),
            new Vertex(new Vector3(0.5f,  0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2(1.0f, 1.0f)),
            new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2( 0.0f, 1.0f)),
            new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2( 0.0f, 0.0f)),
            new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2( 1.0f, 0.0f)),
            new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2( 1.0f, 1.0f)),
            new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2( 0.0f, 1.0f)),
            new Vertex(new Vector3(0.5f, -0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2(0.0f, 0.0f)),
            new Vertex(new Vector3(0.5f,  0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2(1.0f, 0.0f)),
            new Vertex(new Vector3(0.5f,  0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2(1.0f, 1.0f)),
            new Vertex(new Vector3(0.5f, -0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2(0.0f, 1.0f)),
            new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2( 0.0f, 0.0f)),
            new Vertex(new Vector3(0.5f, -0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2(1.0f, 0.0f)),
            new Vertex(new Vector3(0.5f, -0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2(1.0f, 1.0f)),
            new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2( 0.0f, 1.0f)),
            new Vertex(new Vector3(0.5f,  0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2( 0.0f, 0.0f)),
            new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2( 1.0f, 0.0f)),
            new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2( 1.0f, 1.0f)),
            new Vertex(new Vector3(0.5f,  0.5f,  0.5f), new Color4(1f, 1f, 1f, 1f), Vector3.Zero, new Vector2( 0.0f, 1.0f))
        },

        new uint[]
        {
            0, 3, 2,
            2, 1, 0,
            4, 5, 6,
            6, 7 ,4,
            11, 8, 9,
            9, 10, 11,
            12, 13, 14,
            14, 15, 12,
            16, 17, 18,
            18, 19, 16,
            20, 21, 22,
            22, 23, 20
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