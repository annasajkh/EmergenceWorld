namespace EmergenceWorld.Scripts.Core.Abstractions
{
    // Buffer Objects are OpenGL Objects that store an array of unformatted memory allocated by the OpenGL context (AKA the GPU)
    public abstract class BufferObject<T> : OpenGLObject
    {
        public abstract void Data(T[] bufferData);
    }
}