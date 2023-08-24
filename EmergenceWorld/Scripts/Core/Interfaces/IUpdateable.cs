using OpenTK.Windowing.GraphicsLibraryFramework;

namespace EmergenceWorld.Scripts.Core.Interfaces
{
    public interface IUpdateable
    {
        public void Update(KeyboardState keyboardState, MouseState mouseState, float delta);
    }
}