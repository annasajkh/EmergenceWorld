using EmergenceWorld.Scripts.Core.Components;
using EmergenceWorld.Scripts.Core.Managers;
using EmergenceWorld.Scripts.Core.Noise;
using EmergenceWorld.Scripts.Core.OpenGLObjects;
using EmergenceWorld.Scripts.Core.Scenes;
using EmergenceWorld.Scripts.Core.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;

#pragma warning disable CS8618

namespace EmergenceWorld.Scripts.Core
{
    public class Game : GameWindow
    {
        public VertexArrayObject VertexArrayObject { get; private set; }
        public Renderer Renderer { get; private set; }
        public static ResourceManager ResourceManager { get; private set; }
        public static SceneManager SceneManager { get; private set; }
        public static Random Random { get; } = new Random();
        public static FastNoiseLite Noise { get; } = new FastNoiseLite(Random.Next());
        public static int WindowWidth { get; private set; }
        public static int WindowHeight { get; private set; }
        public static float TimeScale { get; set; } = 1;
        public static bool Paused { get; set; }

        public Game(string title, int width, int height): base(GameWindowSettings.Default,
                        new NativeWindowSettings()
                        {
                            Title = title,
                            Size = (width, height)
                        })
        {
            WindowWidth = width;
            WindowHeight = height;

            CenterWindow();

            Settings.Init();

            ResourceManager = new ResourceManager();

            // Load resources
            ResourceManager.AddShader("VoxelShader");


            // Setup the Renderer
            Renderer = new Renderer(ResourceManager.Shaders["VoxelShader"]);

            Renderer.Shader.Bind();
            GL.Uniform3(Renderer.Shader.GetUniformLocation("material.ambient"), 1.25f, 1.25f, 1.25f);
            GL.Uniform3(Renderer.Shader.GetUniformLocation("material.diffuse"), 0.5f, 0.5f, 0.5f);
            GL.Uniform3(Renderer.Shader.GetUniformLocation("material.specular"), 1f, 1f, 1f);
            GL.Uniform1(Renderer.Shader.GetUniformLocation("material.shininess"), 32f);

            GL.Uniform3(Renderer.Shader.GetUniformLocation("dirLight.ambient"), 1.25f, 1.25f, 1.25f);
            GL.Uniform3(Renderer.Shader.GetUniformLocation("dirLight.diffuse"), 0.5f, 0.5f, 0.5f);
            GL.Uniform3(Renderer.Shader.GetUniformLocation("dirLight.specular"), 1f, 1f, 1f);
            Renderer.Shader.Unbind();

            VertexArrayObject = new VertexArrayObject();

            SceneManager = new SceneManager(new World());
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            SceneManager.CurrentScene.Load();
        }

        protected override void OnResize(ResizeEventArgs resizeEventArgs)
        {
            base.OnResize(resizeEventArgs);

            WindowWidth = resizeEventArgs.Width;
            WindowHeight = resizeEventArgs.Height;

            GL.Viewport(0, 0, WindowWidth, WindowHeight);

            SceneManager.CurrentScene.WindowResized(Renderer);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            Renderer.Dispose();
            SceneManager.CurrentScene.Unload();
        }

        protected override void OnUpdateFrame(FrameEventArgs frameEventArgs)
        {
            base.OnUpdateFrame(frameEventArgs);

            KeyboardState keyboardState = KeyboardState;


            if(keyboardState.IsKeyPressed(Keys.P))
            {
                Paused = !Paused;
            }

            if (Paused)
            {
                CursorState = CursorState.Normal;
            }
            else
            {
                CursorState = CursorState.Grabbed;
                

                if (keyboardState.IsKeyDown(Keys.Escape))
                {
                    Close();
                }

                Renderer.Shader.Bind();
                GL.Uniform3(Renderer.Shader.GetUniformLocation("lightColor"), 1f, 1f, 1f);
                Renderer.Shader.Unbind();

                SceneManager.CurrentScene.Update(Renderer, keyboardState, MouseState, (float)frameEventArgs.Time * TimeScale);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.ClearColor(Color.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Renderer.Begin();

            SceneManager.CurrentScene.Render(Renderer, VertexArrayObject);

            Renderer.End();

            SwapBuffers();
        }
    }
}