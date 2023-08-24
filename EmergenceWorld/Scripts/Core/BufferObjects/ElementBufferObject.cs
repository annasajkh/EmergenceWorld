using OpenTK.Graphics.OpenGL4;
using EmergenceWorld.Scripts.Core.Abstractions;

namespace EmergenceWorld.Scripts.Core.BufferObjects
{
    public class ElementBufferObject : BufferObject<uint>
    {
        public BufferUsageHint BufferUsageHint { get; set; }

        public ElementBufferObject(BufferUsageHint bufferUsageHint)
        {
            Handle = GL.GenBuffer();

            BufferUsageHint = bufferUsageHint;
        }

        public override void Data(uint[] bufferData)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Handle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, bufferData.Length * sizeof(uint), bufferData, BufferUsageHint);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public override void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Handle);
        }

        public override void Unbind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public override void Dispose()
        {
            Console.WriteLine($"ElementBufferObject: {Handle} is Unloaded");

            GL.DeleteBuffer(Handle);
            GC.SuppressFinalize(this);
        }
    }
}