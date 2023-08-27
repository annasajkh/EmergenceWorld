using EmergenceWorld.Scripts.Core.Components;
using EmergenceWorld.Scripts.Core.Entities;
using EmergenceWorld.Scripts.Core.OpenGLObjects;
using EmergenceWorld.Scripts.Core.Utils;
using EmergenceWorld.Scripts.Core.Voxels;
using EmergenceWorld.Scripts.Core.WorldGeneration;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Timer = EmergenceWorld.Scripts.Core.Components.Timer;

namespace EmergenceWorld.Scripts.Core.Scenes
{
    public class World : Scene
    {
        public Dictionary<int, Chunk> Chunks = new Dictionary<int, Chunk>();

        private bool stepSimulation = false; // if this set to true then the particle the simulation will step once and this will set to be false again
        private Timer simulationTimer;
        private Player player;
        private Mesh sun;
        private Vector3 lightPosition;

        // 0 - 360
        private float time = 45;

        public World()
        {
            // Initialization
            player = new Player(position: Vector3.Zero,
                                rotation: Vector3.Zero,
                                scale: Vector3.One,
                                cameraSize: new Vector2(Game.WindowWidth, Game.WindowHeight));

            sun = new Mesh(new Vector3(0, 0, 10000), Vector3.Zero, new Vector3(500, 500, 1), BufferUsageHint.StaticDraw, MeshInstance.Quad);

            lightPosition = new Vector3(0, 100, 0);

            simulationTimer = new Timer(0.01f, SimulationTimeout);
            simulationTimer.Start();



            new Chunk(new Vector3i(-Settings.ChunkSize / 2, -Settings.ChunkSize, -Settings.ChunkSize / 2), this);
        }

        public override void Load()
        {

        }

        public override void Unload()
        {
            sun.Dispose();
            Chunks.Clear();
        }

        public override void WindowResized(Renderer renderer)
        {
            player.Camera.Resize(Game.WindowWidth, Game.WindowHeight);
            renderer.Projection = player.Camera.ProjectionMatrix;
        }

        public override void Update(Renderer renderer, KeyboardState keyboardState, MouseState mouseState, float delta)
        {
            simulationTimer.Step(delta);

            player.Update(keyboardState, mouseState, delta);

            renderer.View = player.Camera.ViewMatrix;

            Vector3 sunDirection = Vector3.Normalize(lightPosition - player.Position);

            renderer.Shader.Bind();
            
            GL.Uniform3(renderer.Shader.GetUniformLocation("uViewPos"), player.Position);
            GL.Uniform3(renderer.Shader.GetUniformLocation("uLightPos"), lightPosition);
            GL.Uniform3(renderer.Shader.GetUniformLocation("dirLight.direction"), sunDirection);

            renderer.Shader.Unbind();


            lightPosition = player.Position + new Vector3((float)MathHelper.Cos(MathHelper.DegreesToRadians(time)), (float)MathHelper.Sin(MathHelper.DegreesToRadians(time)), 0) * 5000;

            if (stepSimulation)
            {
                foreach (KeyValuePair<int, Chunk> entry in Chunks)
                {
                    entry.Value.Update(keyboardState, mouseState, delta);
                }

                stepSimulation = false;
            }

            //sun.Position = lightPosition + new Vector3((float)MathHelper.Cos(MathHelper.DegreesToRadians(time)), (float)MathHelper.Sin(MathHelper.DegreesToRadians(time)), 0) * 100;
            //sun.Rotation = new Vector3(0, -90, -MathHelper.RadiansToDegrees((float)MathHelper.Atan2(MathHelper.Cos(MathHelper.DegreesToRadians(time)), MathHelper.Sin(MathHelper.DegreesToRadians(time)))));

            if (keyboardState.IsKeyPressed(Keys.Enter))
            {
                Game.Noise.SetSeed(Game.Random.Next());

                foreach (KeyValuePair<int, Chunk> entry in Chunks)
                {
                    entry.Value.BuildChunk();
                }
            }

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

        public override void Render(Renderer renderer, VertexArrayObject vertexArrayObject)
        {
            GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.CullFace);

            // draw the sun
            renderer.Draw(sun, vertexArrayObject);

            GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.CullFace);

            foreach (KeyValuePair<int, Chunk> entry in Chunks)
            {
                renderer.Draw(entry.Value.Mesh, vertexArrayObject);
            }
        }

        public void SimulationTimeout()
        {
            stepSimulation = true;
        }



        /// <summary>
        /// Get a particle in the world, if it doesn't exist (i.e the chunk doesn't exist) then return null
        /// </summary>
        /// <param name="particlePosition"></param>
        /// <returns></returns>
        public Voxel? GetVoxel(Vector3i particlePosition)
        {
            Voxel? particle = null;


            Vector3i chunkPosition = Chunk.GetChunkPosition(particlePosition);
            int chunkHash = Chunk.GetChunkHash(particlePosition);

            Vector3i particleIndices = particlePosition - chunkPosition;

            if (particleIndices[0] < 0 || particleIndices[0] > Settings.ChunkSize - 1 ||
                particleIndices[1] < 0 || particleIndices[1] > Settings.ChunkSize - 1 ||
                particleIndices[2] < 0 || particleIndices[1] > Settings.ChunkSize - 1)
            {
                return particle;
            }

            if (Chunks.ContainsKey(chunkHash))
            {
                Chunk chunk = Chunks[chunkHash];

                particle = chunk.Voxels[particleIndices.X, particleIndices.Y, particleIndices.Z];
            }

            return particle;
        }

        /// <summary>
        /// Check if a particle is empty, empty means the particle type is Air or if it doesn't exist in the world
        /// </summary>
        /// <param name="particlePosition"></param>
        public bool IsVoxelEmpty(Vector3i particlePosition)
        {
            Voxel? particle = GetVoxel(particlePosition);

            // check if the particle exist in the world or not
            if (particle == null)
            {
                return true;
            }

            return particle.Type == VoxelType.Air;
        }


        public bool IsVoxelAir(Vector3i particlePosition)
        {
            Voxel? particle = GetVoxel(particlePosition);

            // check if the particle exist in the world or not
            if (particle == null)
            {
                return false;
            }

            return particle.Type == VoxelType.Air;
        }

        // get all Adjacent particles base on side
        //
        // visualisation, o is the particle and x is the side it check
        //
        // x | x | x
        // ---------
        // x | o | x
        //----------
        // x | x | x
        public List<Voxel> GetAllAdjacentVoxelsAtSide(Voxel particle, VoxelSide particleSide)
        {
            List<Voxel> adjacentVoxels = new List<Voxel>();

            switch (particleSide)
            {
                case VoxelSide.TopAndBottom:
                    Voxel? adjacentVoxel = GetVoxel(particle.Position + new Vector3i(1, 0, 0));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(-1, 0, 0));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(0, 0, 1));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(0, 0, -1));


                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(1, 0, 1));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(-1, 0, -1));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(1, 0, -1));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(-1, 0, 1));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }
                    break;

                case VoxelSide.LeftAndRight:
                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(0, 1, 0));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(0, -1, 0));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(0, 0, 1));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(0, 0, -1));


                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(0, 1, 1));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(0, -1, -1));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(0, 1, -1));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(0, -1, 1));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }
                    break;

                case VoxelSide.FrontAndBack:
                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(0, 1, 0));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(0, -1, 0));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(1, 0, 0));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(-1, 0, 0));


                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(1, 1, 0));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(-1, -1, 0));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(1, -1, 0));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }

                    adjacentVoxel = GetVoxel(particle.Position + new Vector3i(-1, 1, 0));

                    if (adjacentVoxel != null)
                    {
                        adjacentVoxels.Add(adjacentVoxel);
                    }
                    break;

                default:
                    break;
            }

            return adjacentVoxels;
        }


    }
}
