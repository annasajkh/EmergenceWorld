using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using EmergenceWorld.Scripts.Core.BufferObjects;
using EmergenceWorld.Scripts.Core.Interfaces;
using EmergenceWorld.Scripts.Utils;

namespace EmergenceWorld.Scripts.Core.Components
{
    public class Mesh : IDisposable, IBindable
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public VertexBufferObject VertexBufferObject { get; }
        public ElementBufferObject ElementBufferObject { get; }

        public BufferUsageHint BufferUsageHint { get; set; }

        private float[] vertices;
        private uint[] indices;


        public float[] Vertices
        {
            get
            {
                return vertices;
            }

            set
            {
                vertices = value;
                VertexBufferObject.Data(value);
            }
        }

        public uint[] Indices
        {
            get
            {
                return indices;
            }

            set
            {
                indices = value;
                ElementBufferObject.Data(value);
            }
        }

        public Matrix4 ModelMatrix
        {
            get
            {
                Vector3 rotationRadians = new Vector3(MathHelper.DegreesToRadians(Rotation.X),
                                                      MathHelper.DegreesToRadians(Rotation.Y),
                                                      MathHelper.DegreesToRadians(Rotation.Z));

                return Matrix4.CreateScale(Scale.X, Scale.Y, Scale.Z) *
                       Matrix4.CreateRotationX(rotationRadians.X) *
                       Matrix4.CreateRotationY(rotationRadians.Y) *
                       Matrix4.CreateRotationZ(rotationRadians.Z) *
                       Matrix4.CreateTranslation(Position.X, Position.Y, Position.Z);
            }
        }

        public Mesh(Vector3 position, Vector3 rotation, Vector3 scale, BufferUsageHint bufferUsageHint, float[] vertices, uint[] indices)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;

            this.vertices = vertices;
            this.indices = indices;

            VertexBufferObject = new VertexBufferObject(bufferUsageHint);
            ElementBufferObject = new ElementBufferObject(bufferUsageHint);

            Vertices = vertices;
            Indices = indices;

            BufferUsageHint = bufferUsageHint;
        }


        public Mesh(Vector3 position, Vector3 rotation, Vector3 scale, BufferUsageHint bufferUsageHint, MeshInstance meshInstance)
            : this(position, rotation, scale, bufferUsageHint, Helpers.VerticesBuilder(meshInstance.Vertices), meshInstance.Indices)
        {

        }

        public void Bind()
        {
            VertexBufferObject.Bind();
            ElementBufferObject.Bind();
        }

        public void Unbind()
        {
            VertexBufferObject.Unbind();
            ElementBufferObject.Unbind();
        }

        public void Dispose()
        {
            VertexBufferObject.Dispose();
            ElementBufferObject.Dispose();

            GC.SuppressFinalize(this);
        }

        ~Mesh()
        {
            Dispose();
        }
    }
}