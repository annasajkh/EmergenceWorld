using EmergenceWorld.Scripts.Core.OpenGLObjects;
using EmergenceWorld.Scripts.Core.VertexArrayObjects;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace EmergenceWorld.Scripts.Core.Components
{
    public class Renderer : IDisposable
    {
        private Matrix4 model;
        private Matrix4 view;
        private Matrix4 projection;

        public Shader Shader { get; set; }
        public VertexArrayObject VertexArrayObject { get; private set; }

        public Matrix4 Model
        {
            get
            {
                return model;
            }
        }

        public Matrix4 View
        {
            get
            {
                return view;
            }

            set
            {
                view = value;
            }
        }

        public Matrix4 Projection
        {
            get
            {
                return projection;
            }

            set
            {
                projection = value;
            }
        }

        public Renderer(Shader shader, VertexArrayObject vertexArrayObject)
        {
            Shader = shader;
            VertexArrayObject = vertexArrayObject;

            model = Matrix4.Identity;
            View = Matrix4.Identity;
            Projection = Matrix4.Identity;
        }

        public void Begin()
        {
            Shader.Bind();
            VertexArrayObject.Bind();

            GL.UniformMatrix4(Shader.GetUniformLocation("uView"), false, ref view);
            GL.UniformMatrix4(Shader.GetUniformLocation("uProjection"), false, ref projection);
        }

        public void Render(Mesh mesh)
        {
            model = mesh.ModelMatrix;

            GL.UniformMatrix4(Shader.GetUniformLocation("uModel"), false, ref model);

            mesh.Bind();

            VertexArrayObject.ApplyAttributes();

            GL.DrawElements(PrimitiveType.Triangles, mesh.Indices.Length * 3, DrawElementsType.UnsignedInt, 0);

            mesh.Unbind();
        }

        public void End()
        {
            Shader.Unbind();
            VertexArrayObject.Unbind();
        }

        public void Dispose()
        {
            Shader.Dispose();
            VertexArrayObject.Dispose();

            GC.SuppressFinalize(this);
        }

        ~Renderer()
        {
            Dispose();
        }
    }
}