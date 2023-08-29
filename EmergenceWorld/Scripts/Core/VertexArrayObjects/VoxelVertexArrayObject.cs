using EmergenceWorld.Scripts.Core.OpenGLObjects;
using OpenTK.Graphics.OpenGL4;

namespace EmergenceWorld.Scripts.Core.VertexArrayObjects
{
    public class VoxelVertexArrayObject : VertexArrayObject
    {
        public static int VoxelAttributeSize { get; } = Shader.PositionAttributeSize + Shader.ColorAttributeSize;

        public VoxelVertexArrayObject()
        {
            Handle = GL.GenVertexArray();
        }

        public override void ApplyAttributes()
        {
            // position attribute
            GL.VertexAttribPointer(index: 0,
                                   size: Shader.PositionAttributeSize,
                                   type: VertexAttribPointerType.Float,
                                   normalized: false,
                                   stride: VoxelAttributeSize * sizeof(float),
                                   offset: 0);
            GL.EnableVertexAttribArray(0);

            // color attribute
            GL.VertexAttribPointer(index: 1,
                                   size: Shader.ColorAttributeSize,
                                   type: VertexAttribPointerType.Float,
                                   normalized: false,
                                   stride: VoxelAttributeSize * sizeof(float),
                                   offset: Shader.PositionAttributeSize * sizeof(float));
            GL.EnableVertexAttribArray(1);

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