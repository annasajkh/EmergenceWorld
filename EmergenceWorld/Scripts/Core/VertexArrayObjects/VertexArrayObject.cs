using EmergenceWorld.Scripts.Core.Abstractions;
using EmergenceWorld.Scripts.Core.Interfaces;

namespace EmergenceWorld.Scripts.Core.VertexArrayObjects
{
    public abstract class VertexArrayObject : OpenGLObject, IVertexArrayObject
    {

        public abstract void ApplyAttributes();
    }
}
