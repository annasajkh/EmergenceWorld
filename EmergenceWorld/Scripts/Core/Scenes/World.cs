using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using EmergenceWorld.Scripts.Core.Components;
using EmergenceWorld.Scripts.Core.Entities;
using EmergenceWorld.Scripts.Core.Noise;
using EmergenceWorld.Scripts.Core.Particles;
using EmergenceWorld.Scripts.Core.WorldGeneration;

namespace EmergenceWorld.Scripts.Core.Scenes
{
    public class World : Scene
    {
        public static Random random = new Random();
        public static FastNoiseLite noise = new FastNoiseLite(random.Next());
        public static Dictionary<Vector3i, Chunk> chunks = new Dictionary<Vector3i, Chunk>();

        private Player player;
        private Mesh sun;
        private Vector3 lightPosition;

        // 0 - 360
        private float time;

        public World()
        {
            // Initialization
            player = new Player(position: Vector3.Zero,
                                rotation: Vector3.Zero,
                                scale: Vector3.One,
                                cameraSize: new Vector2(Game.WindowWidth, Game.WindowHeight));

            sun = new Mesh(Vector3.Zero, Vector3.Zero, new Vector3(500, 1, 500), BufferUsageHint.StaticDraw, MeshInstance.Quad);

            lightPosition = new Vector3(0, 100, 0);
        }

        public override void OnLoad()
        {

        }

        public override void OnUnload()
        {
            sun.Dispose();
        }

        public override void OnResize()
        {
            player.Camera.Resize(Game.WindowWidth, Game.WindowHeight);
            Program.game.Renderer.Projection = player.Camera.ProjectionMatrix;
        }

        public override void OnUpdateFrame(KeyboardState keyboardState, MouseState mouseState, float delta)
        {
            player.Update(keyboardState, mouseState, delta);

            Program.game.Renderer.View = player.Camera.ViewMatrix;

            Vector3 sunDirection = Vector3.Normalize(lightPosition - player.Position);

            Program.game.Renderer.Shader.Bind();
            
            GL.Uniform3(Program.game.Renderer.Shader.GetUniformLocation("uViewPos"), player.Position);
            GL.Uniform3(Program.game.Renderer.Shader.GetUniformLocation("uLightPos"), lightPosition);
            GL.Uniform3(Program.game.Renderer.Shader.GetUniformLocation("dirLight.direction"), sunDirection);

            Program.game.Renderer.Shader.Unbind();

            lightPosition = player.Position + new Vector3((float)MathHelper.Cos(MathHelper.DegreesToRadians(time)), (float)MathHelper.Sin(MathHelper.DegreesToRadians(time)), 0) * 5000;

            sun.Position = lightPosition + new Vector3((float)MathHelper.Cos(MathHelper.DegreesToRadians(time)), (float)MathHelper.Sin(MathHelper.DegreesToRadians(time)), 0) * 100;
            sun.Rotation = new Vector3(0, 90, -MathHelper.RadiansToDegrees((float)MathHelper.Atan2(MathHelper.Cos(MathHelper.DegreesToRadians(time)), MathHelper.Sin(MathHelper.DegreesToRadians(time)))));

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                time += delta * 100;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                time -= delta * 100;
            }

            if (time > 360)
            {
                time = 0;
            }
        }

        public override void OnRenderFrame(Renderer renderer)
        {
            Program.game.DefaultTexture.Bind();


            GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.CullFace);

            // draw the sun
            renderer.Draw(sun, Program.game.VertexArrayObject);

            foreach (KeyValuePair<Vector3i, Chunk> entry in chunks)
            {
                renderer.Draw(entry.Value.Mesh, Program.game.VertexArrayObject);
            }

            GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.CullFace);
        }

        /// <summary>
        /// Get a particle in the world, if it doesn't exist (i.e the chunk doesn't exist) then return null
        /// </summary>
        /// <param name="particlePosition"></param>
        /// <returns></returns>
        public static Particle? GetParticle(Vector3i particlePosition)
        {

            Particle? particle = null;

            Vector3i chunkPosition = Chunk.GetChunkHash(particlePosition);
            Vector3i particleIndices = particlePosition - chunkPosition;


            if (particleIndices[0] < 0 || particleIndices[0] > Settings.ChunkSize - 1 ||
                particleIndices[1] < 0 || particleIndices[1] > Settings.ChunkSize - 1 ||
                particleIndices[2] < 0 || particleIndices[1] > Settings.ChunkSize - 1)
            {
                return particle;
            }

            if (chunks.ContainsKey(chunkPosition))
            {
                Chunk chunk = chunks[chunkPosition];

                particle = chunk.Particles[particleIndices.X, particleIndices.Y, particleIndices.Z];
            }

            return particle;
        }

        /// <summary>
        /// Check if a particle is empty, empty means the particle type is empty or if it doesn't exist in the world
        /// </summary>
        /// <param name="particlePosition"></param>
        public static bool IsParticleEmpty(Vector3i particlePosition)
        {
            Particle? particle = GetParticle(particlePosition);

            // check if the particle exist in the world or not
            if (particle == null)
            {
                return true;
            }

            return particle.Type == ParticleType.Empty;
        }
    }
}
