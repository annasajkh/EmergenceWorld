using OpenTK.Windowing.GraphicsLibraryFramework;
using EmergenceWorld.Scripts.Core.Components;

namespace EmergenceWorld.Scripts.Core.Scenes
{
    public abstract class Scene
    {

        public abstract void OnLoad();

        public abstract void OnUnload();

        public abstract void OnResize();

        public abstract void OnUpdateFrame(KeyboardState keyboardState, MouseState mouseState, float delta);

        public abstract void OnRenderFrame(Renderer renderer);

    }
}
