using OpenTK.Graphics.OpenGL4;
using EmergenceWorld.Scripts.Core.Abstractions;

namespace EmergenceWorld.Scripts.Core.OpenGLObjects
{
    public class VertexArrayObject : OpenGLObject
    {
        public VertexArrayObject()
        {
            Handle = GL.GenVertexArray();
        }

        public void ApplyAttributes()
        {
            // position attribute
            GL.VertexAttribPointer(index: 0,
                                   size: Shader.PositionAttributeSize,
                                   type: VertexAttribPointerType.Float,
                                   normalized: false,
                                   stride: Shader.AllAttributeSize * sizeof(float),
                                   offset: 0);
            GL.EnableVertexAttribArray(0);

            // color attribute
            GL.VertexAttribPointer(index: 1,
                                   size: Shader.ColorAttributeSize,
                                   type: VertexAttribPointerType.Float,
                                   normalized: false,
                                   stride: Shader.AllAttributeSize * sizeof(float),
                                   offset: Shader.PositionAttributeSize * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // normal attribute
            GL.VertexAttribPointer(index: 2,
                                   size: Shader.NormalAttributeSize,
                                   type: VertexAttribPointerType.Float,
                                   normalized: false,
                                   stride: Shader.AllAttributeSize * sizeof(float),
                                   offset: (Shader.PositionAttributeSize + Shader.ColorAttributeSize) * sizeof(float));
            GL.EnableVertexAttribArray(2);


            // texture coordinates attribute
            GL.VertexAttribPointer(index: 3,
                                   size: Shader.TextureCoordinateAttributeSize,
                                   type: VertexAttribPointerType.Float,
                                   normalized: false,
                                   stride: Shader.AllAttributeSize * sizeof(float),
                                   offset: (Shader.PositionAttributeSize + Shader.ColorAttributeSize + Shader.NormalAttributeSize) * sizeof(float));
            GL.EnableVertexAttribArray(3);
        }

        public override void Bind()
        {
            GL.BindVertexArray(Handle);
        }

        public override void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public override void Dispose()
        {
            Console.WriteLine($"VertexArrayObject: {Handle} is Unloaded");

            GL.DeleteBuffer(Handle);
            GC.SuppressFinalize(this);
        }
    }
}