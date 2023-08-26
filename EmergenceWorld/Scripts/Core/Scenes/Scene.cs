using EmergenceWorld.Scripts.Core.Components;
using EmergenceWorld.Scripts.Core.OpenGLObjects;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace EmergenceWorld.Scripts.Core.Scenes
{
    public abstract class Scene
    {
        public abstract void Load();

        public abstract void Unload();

        public abstract void WindowResized(Renderer renderer);

        public abstract void Update(Renderer renderer, KeyboardState keyboardState, MouseState mouseState, float delta);

        public abstract void Render(Renderer renderer, VertexArrayObject vertexArrayObject);

    }
}
