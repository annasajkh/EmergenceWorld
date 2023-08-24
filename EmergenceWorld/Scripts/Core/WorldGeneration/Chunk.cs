using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using EmergenceWorld.Scripts.Core.Components;
using EmergenceWorld.Scripts.Core.Containers;
using EmergenceWorld.Scripts.Core.Particles;
using EmergenceWorld.Scripts.Core.Scenes;
using EmergenceWorld.Scripts.Utils;

namespace EmergenceWorld.Scripts.Core.WorldGeneration
{
    public class Chunk
    {
        public Vector3i Position { get; private set; }
        public Particle[,,] Particles { get; private set; }
        public Mesh Mesh { get; private set; }

        private Dictionary<Vector3i, Vertex> VerticesDic = new Dictionary<Vector3i, Vertex>();
        public List<Vertex> Vertices { get; private set; }
        public List<uint> Indices { get; private set; }


        public Chunk(Vector3i position)
        {
            Position = position;
            Particles = new Particle[Settings.ChunkSize, Settings.ChunkSize, Settings.ChunkSize];

            Vertices = new List<Vertex>();
            Indices = new List<uint>();

            for (int i = 0; i < Particles.GetLength(0); i++)
            {
                for (int j = 0; j < Particles.GetLength(1); j++)
                {
                    for (int k = 0; k < Particles.GetLength(2); k++)
                    {
                        // particle position is its chunk position with offset depending on its index
                        Particles[i, j, k] = new Particle(position: new Vector3i(Position.X + i,
                                                                                 Position.Y + j, 
                                                                                 Position.Z + k),
                                                          type: World.noise.GetNoise(Position.X + i * 2, Position.Y + j * 2, Position.Z + k * 2) > 0 ? ParticleType.Grass : ParticleType.Empty);
                    }
                }
            }

            World.chunks.Add(Position, this);


            Mesh = new Mesh(position: position,
                            rotation: Vector3.Zero,
                            scale: Vector3.One,
                            bufferUsageHint: BufferUsageHint.DynamicDraw,
                            vertices: new float[] { },
                            indices: new uint[] { });


            Build();

            Mesh.Vertices = Helpers.VerticesBuilder(Vertices.ToArray());
            Mesh.Indices = Indices.ToArray();
        }

        private void BuildVertex(Particle particle, Vector3 vertexPosition, Vector2 textureCoordinate)
        {
            //if (!VerticesDic.ContainsKey((Vector3i)vertexPosition))
            //{
            //    VerticesDic.Add((Vector3i)vertexPosition,
            //    new Vertex(
            //        position: vertexPosition,
            //        color: particle.GetColor(),
            //        normal: Vector3.Zero,
            //        textureCoordinate: textureCoordinate
            //    ));
            //}

            Vertices.Add(new Vertex(
                    position: vertexPosition,
                    color: particle.GetColor(),
                    normal: Vector3.Zero,
                    textureCoordinate: textureCoordinate
                ));
        }


        /// <summary>
        /// Build this chunk
        /// </summary>
        public void Build()
        {
            Vertices.Clear();
            Indices.Clear();
            VerticesDic.Clear();


            for (int i = 0; i < Particles.GetLength(0); i++)
            {
                for (int j = 0; j < Particles.GetLength(1); j++)
                {
                    for (int k = 0; k < Particles.GetLength(2); k++)
                    {
                        Particle particle = Particles[i, j, k];

                        if (!(particle.Type == ParticleType.Empty))
                        {
                            // particle actual position in world coordinate and not particle coordinate
                            Vector3 particleSettingsPosition = particle.Position * Settings.ParticleSize;

                            // check above this particle is empty or not
                            if (World.IsParticleEmpty(Position + new Vector3i(i, j + 1, k)))
                            {
                                // build top quad

                                BuildVertex(particle, particleSettingsPosition + new Vector3(Settings.ParticleSize / 2, Settings.ParticleSize / 2, -Settings.ParticleSize / 2), new Vector2(1f, 1f));

                                BuildVertex(particle, particleSettingsPosition + new Vector3(Settings.ParticleSize / 2, Settings.ParticleSize / 2, Settings.ParticleSize / 2), new Vector2(1f, 0f));

                                BuildVertex(particle, particleSettingsPosition + new Vector3(-Settings.ParticleSize / 2, Settings.ParticleSize / 2, Settings.ParticleSize / 2), new Vector2(0f, 0f));

                                BuildVertex(particle, particleSettingsPosition + new Vector3(-Settings.ParticleSize / 2, Settings.ParticleSize / 2, -Settings.ParticleSize / 2), new Vector2(0f, 1f));
                            }




                            // check bellow this particle is empty or not
                            if (World.IsParticleEmpty(Position + new Vector3i(i, j - 1, k)))
                            {
                                // build bottom quad

                                BuildVertex(particle, particleSettingsPosition + new Vector3(Settings.ParticleSize / 2, -Settings.ParticleSize / 2, -Settings.ParticleSize / 2), new Vector2(1f, 1f));

                                BuildVertex(particle, particleSettingsPosition + new Vector3(Settings.ParticleSize / 2, -Settings.ParticleSize / 2, Settings.ParticleSize / 2), new Vector2(1f, 0f));

                                BuildVertex(particle, particleSettingsPosition + new Vector3(-Settings.ParticleSize / 2, -Settings.ParticleSize / 2, Settings.ParticleSize / 2), new Vector2(0f, 0f));

                                BuildVertex(particle, particleSettingsPosition + new Vector3(-Settings.ParticleSize / 2, -Settings.ParticleSize / 2, -Settings.ParticleSize / 2), new Vector2(0f, 1f));
                            }




                            // check right this particle is empty or not
                            if (World.IsParticleEmpty(Position + new Vector3i(i + 1, j, k)))
                            {
                                // build right quad

                                BuildVertex(particle, particleSettingsPosition + new Vector3(Settings.ParticleSize / 2, Settings.ParticleSize / 2, Settings.ParticleSize / 2), new Vector2(1f, 1f));

                                BuildVertex(particle, particleSettingsPosition + new Vector3(Settings.ParticleSize / 2, -Settings.ParticleSize / 2, Settings.ParticleSize / 2), new Vector2(1f, 0f));

                                BuildVertex(particle, particleSettingsPosition + new Vector3(Settings.ParticleSize / 2, -Settings.ParticleSize / 2, -Settings.ParticleSize / 2), new Vector2(0f, 0f));

                                BuildVertex(particle, particleSettingsPosition + new Vector3(Settings.ParticleSize / 2, Settings.ParticleSize / 2, -Settings.ParticleSize / 2), new Vector2(0f, 1f));
                            }




                            // check left this particle is empty or not
                            if (World.IsParticleEmpty(Position + new Vector3i(i - 1, j, k)))
                            {
                                // build left quad

                                BuildVertex(particle, particleSettingsPosition + new Vector3(-Settings.ParticleSize / 2, Settings.ParticleSize / 2, Settings.ParticleSize / 2), new Vector2(1f, 1f));

                                BuildVertex(particle, particleSettingsPosition + new Vector3(-Settings.ParticleSize / 2, -Settings.ParticleSize / 2, Settings.ParticleSize / 2), new Vector2(1f, 0f));

                                BuildVertex(particle, particleSettingsPosition + new Vector3(-Settings.ParticleSize / 2, -Settings.ParticleSize / 2, -Settings.ParticleSize / 2), new Vector2(0f, 0f));

                                BuildVertex(particle, particleSettingsPosition + new Vector3(-Settings.ParticleSize / 2, Settings.ParticleSize / 2, -Settings.ParticleSize / 2), new Vector2(0f, 1f));
                            }




                            // check front this particle is empty or not
                            if (World.IsParticleEmpty(Position + new Vector3i(i, j, k + 1)))
                            {
                                // build front quad

                                BuildVertex(particle, particleSettingsPosition + new Vector3(Settings.ParticleSize / 2, Settings.ParticleSize / 2, Settings.ParticleSize / 2), new Vector2(1f, 1f));

                                BuildVertex(particle, particleSettingsPosition + new Vector3(Settings.ParticleSize / 2, -Settings.ParticleSize / 2, Settings.ParticleSize / 2), new Vector2(1f, 0f));

                                BuildVertex(particle, particleSettingsPosition + new Vector3(-Settings.ParticleSize / 2, -Settings.ParticleSize / 2, Settings.ParticleSize / 2), new Vector2(0f, 0f));

                                BuildVertex(particle, particleSettingsPosition + new Vector3(-Settings.ParticleSize / 2, Settings.ParticleSize / 2, Settings.ParticleSize / 2), new Vector2(0f, 1f));
                            }




                            // check back this particle is empty or not
                            if (World.IsParticleEmpty(Position + new Vector3i(i, j, k - 1)))
                            {
                                // build back quad

                                BuildVertex(particle, particleSettingsPosition + new Vector3(Settings.ParticleSize / 2, Settings.ParticleSize / 2, -Settings.ParticleSize / 2), new Vector2(1f, 1f));

                                BuildVertex(particle, particleSettingsPosition + new Vector3(Settings.ParticleSize / 2, -Settings.ParticleSize / 2, -Settings.ParticleSize / 2), new Vector2(1f, 0f));

                                BuildVertex(particle, particleSettingsPosition + new Vector3(-Settings.ParticleSize / 2, -Settings.ParticleSize / 2, -Settings.ParticleSize / 2), new Vector2(0f, 0f));

                                BuildVertex(particle, particleSettingsPosition + new Vector3(-Settings.ParticleSize / 2, Settings.ParticleSize / 2, -Settings.ParticleSize / 2), new Vector2(0f, 1f));
                            }

                        }
                    }
                }
            }

            //Vertices = VerticesDic.Values.ToList();
            //VerticesDic.Clear();


            uint[] quadIndices = new uint[] { 0, 1, 3, 1, 2, 3 };

            for (uint i = 0; i < Vertices.Count / 4; i++)
            {
                Indices.Add(quadIndices[0] + i * 4);
                Indices.Add(quadIndices[1] + i * 4);
                Indices.Add(quadIndices[2] + i * 4);

                Indices.Add(quadIndices[3] + i * 4);
                Indices.Add(quadIndices[4] + i * 4);
                Indices.Add(quadIndices[5] + i * 4);
            }
        }

        public static Vector3i GetChunkHash(Vector3i particlePosition)
        {
            return Helpers.SnapToGrid(particlePosition, Settings.ChunkSize);
        }
    }
}
