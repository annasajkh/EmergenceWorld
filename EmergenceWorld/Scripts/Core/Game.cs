using EmergenceWorld.Scripts.Core.Components;
using EmergenceWorld.Scripts.Core.Noise;
using EmergenceWorld.Scripts.Core.OpenGLObjects;
using EmergenceWorld.Scripts.Core.Scenes;
using EmergenceWorld.Scripts.Core.WorldGeneration;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;
using System.Drawing;


namespace EmergenceWorld.Scripts.Core
{
    public class Game : GameWindow
    {
        public Texture2D DefaultTexture { get; private set; }

        public VertexArrayObject VertexArrayObject { get; private set; }
        public Renderer Renderer { get; private set; }
        public static int WindowWidth { get; private set; }
        public static int WindowHeight { get; private set; }

        private World world;

        private bool mouseFree = false;

        public Game(string title, int width, int height)
                : base(GameWindowSettings.Default,
                        new NativeWindowSettings()
                        {
                            Title = title,
                            Size = (width, height)
                        })
        {
            WindowWidth = width;
            WindowHeight = height;

            // Settings
            World.noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);

            CenterWindow();

            GL.Enable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            StbImage.stbi_set_flip_vertically_on_load(1);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);


            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);



            // Setup the Renderer
            Renderer = new Renderer(new Shader(vertexShaderPath: Path.GetFullPath("Resources/Shaders/default.vert"),
                                               geometryShaderPath: Path.GetFullPath("Resources/Shaders/default.geom"),
                                               fragmentShaderPath: Path.GetFullPath("Resources/Shaders/default.frag")));

            Renderer.Shader.Bind();
            GL.Uniform3(Renderer.Shader.GetUniformLocation("material.ambient"), 1.25f, 1.25f, 1.25f);
            GL.Uniform3(Renderer.Shader.GetUniformLocation("material.diffuse"), 0.5f, 0.5f, 0.5f);
            GL.Uniform3(Renderer.Shader.GetUniformLocation("material.specular"), 1f, 1f, 1f);
            GL.Uniform1(Renderer.Shader.GetUniformLocation("material.shininess"), 32f);

            GL.Uniform3(Renderer.Shader.GetUniformLocation("dirLight.ambient"), 1.25f, 1.25f, 1.25f);
            GL.Uniform3(Renderer.Shader.GetUniformLocation("dirLight.diffuse"), 0.5f, 0.5f, 0.5f);
            GL.Uniform3(Renderer.Shader.GetUniformLocation("dirLight.specular"), 1f, 1f, 1f);
            Renderer.Shader.Unbind();

            DefaultTexture = new Texture2D(ImageResult.FromStream(File.OpenRead("Resources/Textures/default.png"), ColorComponents.RedGreenBlueAlpha));

            VertexArrayObject = new VertexArrayObject();

            world = new World();


            new Chunk(new Vector3i(0, 0, 0));
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            world.OnLoad();
        }

        protected override void OnResize(ResizeEventArgs resizeEventArgs)
        {
            base.OnResize(resizeEventArgs);

            WindowWidth = resizeEventArgs.Width;
            WindowHeight = resizeEventArgs.Height;

            GL.Viewport(0, 0, WindowWidth, WindowHeight);

            world.OnResize();
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            Renderer.Dispose();
            world.OnUnload();
        }

        protected override void OnUpdateFrame(FrameEventArgs frameEventArgs)
        {
            base.OnUpdateFrame(frameEventArgs);

            KeyboardState keyboardState = KeyboardState;


            if(keyboardState.IsKeyPressed(Keys.P))
            {
                mouseFree = !mouseFree;
            }

            if (mouseFree)
            {
                CursorState = CursorState.Normal;
            }
            else
            {
                CursorState = CursorState.Grabbed;
            }

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            Renderer.Shader.Bind();
            GL.Uniform3(Renderer.Shader.GetUniformLocation("lightColor"), 1f, 1f, 1f);
            Renderer.Shader.Unbind();

            world.OnUpdateFrame(keyboardState, MouseState, (float)frameEventArgs.Time);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.ClearColor(Color.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Renderer.Begin();

            world.OnRenderFrame(Renderer);

            Renderer.End();

            SwapBuffers();
        }
    }
}