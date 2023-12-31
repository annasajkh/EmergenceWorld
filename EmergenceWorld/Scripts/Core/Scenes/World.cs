﻿using EmergenceWorld.Scripts.Core.Entities;
using EmergenceWorld.Scripts.Core.Noise;
using EmergenceWorld.Scripts.Core.Utils;
using EmergenceWorld.Scripts.Core.Voxels;
using EmergenceWorld.Scripts.Core.WorldGeneration;
using EmergenceWorld.Scripts.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Timer = EmergenceWorld.Scripts.Core.Components.Timer;

namespace EmergenceWorld.Scripts.Core.Scenes
{
    public class World : Scene
    {
        public Dictionary<int, Chunk> Chunks = new Dictionary<int, Chunk>();
        public Random Random { get; }
        public FastNoiseLite Noise { get; }
        public bool Determistic { get; } = true;

        // 0 - 360
        private float time = 45;
        private bool voxelUpdateStep = false; // if this set to true then the voxel update will step once and this will set to be false again
        private Timer voxelUpdateTimer;
        private Player player;
        private Vector3 lightPosition;

        public World()
        {
            // Settings
            if (Determistic)
            {
                Random = new Random(1);
                Noise = new FastNoiseLite(1);
            }
            else
            {
                Random = new Random();
                Noise = new FastNoiseLite(Random.Next());
            }

            Noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);

            // Setup the Renderer
            Game.Renderer.Shader.Bind();
            GL.Uniform3(Game.Renderer.Shader.GetUniformLocation("material.ambient"), 1.25f, 1.25f, 1.25f);
            GL.Uniform3(Game.Renderer.Shader.GetUniformLocation("material.diffuse"), 0.5f, 0.5f, 0.5f);
            GL.Uniform3(Game.Renderer.Shader.GetUniformLocation("material.specular"), 1f, 1f, 1f);
            GL.Uniform1(Game.Renderer.Shader.GetUniformLocation("material.shininess"), 32f);

            GL.Uniform3(Game.Renderer.Shader.GetUniformLocation("dirLight.ambient"), 1.25f, 1.25f, 1.25f);
            GL.Uniform3(Game.Renderer.Shader.GetUniformLocation("dirLight.diffuse"), 0.5f, 0.5f, 0.5f);
            GL.Uniform3(Game.Renderer.Shader.GetUniformLocation("dirLight.specular"), 1f, 1f, 1f);
            GL.Uniform3(Game.Renderer.Shader.GetUniformLocation("lightColor"), 1f, 1f, 1f);
            Game.Renderer.Shader.Unbind();



            // Initialization
            player = new Player(position: new Vector3(0, 100, 0),
                                rotation: Vector3.Zero,
                                scale: Vector3.One,
                                cameraSize: new Vector2(Game.WindowWidth, Game.WindowHeight));

            lightPosition = new Vector3(0, 100, 0);

            voxelUpdateTimer = new Timer(0.01f, VoxelUpdate);
            voxelUpdateTimer.Start();


            for (int i = -Settings.RenderDistance / 2; i < Settings.RenderDistance / 2; i++)
            {
                for (int j = -Settings.RenderDistance / 2; j < Settings.RenderDistance / 2; j++)
                {
                    for (int k = -Settings.RenderDistance / 2; k < Settings.RenderDistance / 2; k++)
                    {
                        Chunk chunk = new Chunk(new Vector3i(i * (Settings.ChunkSize / 2),
                                                             j * (Settings.ChunkSize / 2),
                                                             k * (Settings.ChunkSize / 2)), this);

                        Chunks.Add(chunk.GetHashCode(), chunk);
                    }
                }
            }

            foreach (Chunk chunk in Chunks.Values.ToList())
            {
                chunk.BuildChunk();
            }
        }

        public override void Load()
        {

        }

        public override void Unload()
        {
            Chunks.Clear();
        }

        public override void WindowResized()
        {
            player.Camera.Resize(Game.WindowWidth, Game.WindowHeight);
            Game.Renderer.Projection = player.Camera.ProjectionMatrix;
        }

        public override void Update(KeyboardState keyboardState, MouseState mouseState, float delta)
        {
            voxelUpdateTimer.Step(delta);

            player.Update(keyboardState, mouseState, delta);

            Game.Renderer.View = player.Camera.ViewMatrix;

            Vector3 sunDirection = Vector3.Normalize(lightPosition - player.Position);

            Game.Renderer.Shader.Bind();
            
            GL.Uniform3(Game.Renderer.Shader.GetUniformLocation("uViewPos"), player.Position);
            GL.Uniform3(Game.Renderer.Shader.GetUniformLocation("uLightPos"), lightPosition);
            GL.Uniform3(Game.Renderer.Shader.GetUniformLocation("dirLight.direction"), sunDirection);

            Game.Renderer.Shader.Unbind();

            lightPosition = player.Position + new Vector3((float)MathHelper.Cos(MathHelper.DegreesToRadians(time)), (float)MathHelper.Sin(MathHelper.DegreesToRadians(time)), 0) * 5000;


            if (voxelUpdateStep)
            {
                foreach (KeyValuePair<int, Chunk> chunk in Chunks)
                {
                    chunk.Value.Update(keyboardState, mouseState, delta);
                }

                voxelUpdateStep = false;
            }

            //sun.Position = lightPosition + new Vector3((float)MathHelper.Cos(MathHelper.DegreesToRadians(time)), (float)MathHelper.Sin(MathHelper.DegreesToRadians(time)), 0) * 100;
            //sun.Rotation = new Vector3(0, -90, -MathHelper.RadiansToDegrees((float)MathHelper.Atan2(MathHelper.Cos(MathHelper.DegreesToRadians(time)), MathHelper.Sin(MathHelper.DegreesToRadians(time)))));

            if (keyboardState.IsKeyPressed(Keys.Enter))
            {
                Noise.SetSeed(Game.Random.Next());

                foreach (KeyValuePair<int, Chunk> chunk in Chunks)
                {
                    chunk.Value.BuildChunk();
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

        public override void Render()
        {
            foreach (KeyValuePair<int, Chunk> entry in Chunks)
            {
                Game.Renderer.Render(entry.Value.Mesh);
            }
        }


        /// <summary>
        /// Update all the voxel in this world
        /// </summary>
        public void VoxelUpdate()
        {
            voxelUpdateStep = true;
        }


        /// <summary>
        /// Get voxel in this world, if it doesn't exist return null
        /// </summary>
        /// <param name="voxelPosition"></param>
        /// <returns></returns>
        public Voxel? GetVoxel(Vector3i voxelPosition)
        {
            Vector3i chunkPosition = Chunk.GetChunkPosition(voxelPosition);

            int chunkHash = Helpers.GetChunkHashCode(chunkPosition);

            Vector3i voxelIndices = voxelPosition - chunkPosition;

            if (Chunks.ContainsKey(chunkHash))
            {
                Chunk chunk = Chunks[chunkHash];

                return chunk.Voxels[voxelIndices.X, voxelIndices.Y, voxelIndices.Z];
            }

            return null;
        }


        /// <summary>
        /// Check voxel in this world with a spesific type, if it doesn't exist return false
        /// </summary>
        /// <param name="voxelType"></param>
        /// <param name="voxelPosition"></param>
        public bool VoxelIsType(Vector3i voxelPosition, VoxelType voxelType)
        {
            Voxel? voxel = GetVoxel(voxelPosition);

            // check if the voxel exist in the world or not
            if (voxel == null)
            {
                return false;
            }

            return voxel.Type == voxelType;
        }


        /// <summary>
        /// Check voxel in this world if it's empty or not, empty means the voxel is either air or doesn't exist
        /// </summary>
        /// <param name="voxelType"></param>
        /// <param name="voxelPosition"></param>
        public bool IsVoxelEmpty(Vector3i voxelPosition)
        {
            Voxel? voxel = GetVoxel(voxelPosition);

            // check if the voxel exist in the world or not
            if (voxel == null)
            {
                return true;
            }

            return voxel.Type == VoxelType.Air;
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
