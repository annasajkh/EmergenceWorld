using EmergenceWorld.Scripts.Core.Components;
using EmergenceWorld.Scripts.Core.Containers;
using EmergenceWorld.Scripts.Core.Particles;
using EmergenceWorld.Scripts.Core.Scenes;
using EmergenceWorld.Scripts.Core.Utils;
using EmergenceWorld.Scripts.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace EmergenceWorld.Scripts.Core.WorldGeneration
{
    public class Chunk
    {
        public World World { get; }
        public Vector3i Position { get; private set; }
        public Particle[,,] Particles { get; private set; }
        public Mesh Mesh { get; private set; }
        public Dictionary<Vertex, int> VerticesFastIndexOf { get; private set; }
        public List<Vertex> Vertices { get; private set; }
        public List<uint> Indices { get; private set; }

        // for checking if any particle in this chunk moves
        public bool IsParticleMove { get; set; } = false;

        private Dictionary<int, Vertex> verticesTemp = new Dictionary<int, Vertex>();

        private List<int> quadVertexIndices = new List<int>();

        private int verticesIndex = 0;

        public Chunk(Vector3i position, World world)
        {
            Position = position;
            World = world;
            Particles = new Particle[Settings.ChunkSize, Settings.ChunkSize, Settings.ChunkSize];

            VerticesFastIndexOf = new Dictionary<Vertex, int>();
            Vertices = new List<Vertex>();
            Indices = new List<uint>();


            World.Chunks.Add(GetChunkHash(), this);

            Mesh = new Mesh(position: position,
                            rotation: Vector3.Zero,
                            scale: Vector3.One,
                            bufferUsageHint: BufferUsageHint.DynamicDraw,
                            vertices: new float[] { },
                            indices: new uint[] { });

            BuildChunk();
        }

        public void BuildChunk()
        {
            //ParticleType[] particleTypes = new ParticleType[] { ParticleType.Water, ParticleType.Sand };

            // World.noise.GetNoise(Position.X + i * 2, Position.Y + j * 2, Position.Z + k * 2) > 0 ? particleTypes[World.random.Next() % particleTypes.Length] : ParticleType.Empty

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
                                                          i: i,
                                                          j: j,
                                                          k: k,
                                                          type: Game.Noise.GetNoise(Position.X + i * 2, Position.Y + j * 2, Position.Z + k * 2) > 0 ? ParticleType.Water : ParticleType.Air,
                                                          chunk: this);
                    }
                }
            }

            BuildMesh();
        }


        /// <summary>
        /// Build this chunk
        /// </summary>
        public void BuildMesh()
        {
            for (int i = 0; i < Particles.GetLength(0); i++)
            {
                for (int j = 0; j < Particles.GetLength(1); j++)
                {
                    for (int k = 0; k < Particles.GetLength(2); k++)
                    {
                        Particle particle = Particles[i, j, k];

                        if (!(particle.Type == ParticleType.Air))
                        {
                            // particle actual position in world coordinate and not particle coordinate
                            Vector3 particleActualPosition = particle.Position;


                            // check front this particle is if it is an air or not
                            if (IsParticleEmpty(i, j, k + 1))
                            {
                                bool isParticleMerged = true;
                                //bool isParticleMerged = IsParticleMerged(particle, ParticleSide.FrontAndBack);
                                // build front quad

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[0].Position, isParticleMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[1].Position, isParticleMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[2].Position, isParticleMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[3].Position, isParticleMerged);

                                AddQuadIndices();
                            }



                            // check right this particle is if it is an air or not
                            if (IsParticleEmpty(i + 1, j, k))
                            {
                                bool isParticleMerged = true;
                                //bool isParticleMerged = IsParticleMerged(particle, ParticleSide.LeftAndRight);

                                // build right quad

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[12].Position, isParticleMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[13].Position, isParticleMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[14].Position, isParticleMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[15].Position, isParticleMerged);

                                AddQuadIndices();
                            }



                            // check back this particle is if it is an air or not
                            if (IsParticleEmpty(i, j, k - 1))
                            {
                                bool isParticleMerged = true;
                                //bool isParticleMerged = IsParticleMerged(particle, ParticleSide.FrontAndBack);

                                // build back quad

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[4].Position, isParticleMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[5].Position, isParticleMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[6].Position, isParticleMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[7].Position, isParticleMerged);

                                AddQuadIndices();
                            }



                            // check left this particle is if it is an air or not
                            if (IsParticleEmpty(i - 1, j, k))
                            {
                                bool isParticleMerged = true;
                                //bool isParticleMerged = IsParticleMerged(particle, ParticleSide.LeftAndRight);

                                // build left quad

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[8].Position, isParticleMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[9].Position, isParticleMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[10].Position, isParticleMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[11].Position, isParticleMerged);

                                AddQuadIndices();
                            }



                            // check above this particle is if it is an air or not
                            if (IsParticleEmpty(i, j + 1, k))
                            {
                                bool isParticleMerged = true;
                                //bool isParticleMerged = IsParticleMerged(particle, ParticleSide.TopAndBottom);

                                // build top quad

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[16].Position, isParticleMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[17].Position, isParticleMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[18].Position, isParticleMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[19].Position, isParticleMerged);

                                AddQuadIndices();

                            }



                            // check bellow this particle is if it is an air or not
                            if (IsParticleEmpty(i, j - 1, k))
                            {
                                bool isParticleMerged = true;
                                //bool isParticleMerged = IsParticleMerged(particle, ParticleSide.TopAndBottom);

                                // build bottom quad

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[20].Position, isParticleMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[21].Position, isParticleMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[22].Position, isParticleMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[23].Position, isParticleMerged);

                                AddQuadIndices();
                            }
                        }
                    }
                }
            }

            verticesIndex = 0;

            Mesh.Vertices = Helpers.VerticesBuilder(VerticesFastIndexOf.Keys.ToArray());
            Mesh.Indices = Indices.ToArray();

            Mesh.Scale = Vector3.One * Settings.ParticleSize;

            VerticesFastIndexOf.Clear();
            Vertices.Clear();
            Indices.Clear();

            verticesTemp.Clear();
        }

        public void Update(KeyboardState keyboardState, MouseState mouseState, float delta)
        {

            for (int i = 0; i < Particles.GetLength(0); i++)
            {
                for (int j = 0; j < Particles.GetLength(1); j++)
                {
                    for (int k = 0; k < Particles.GetLength(2); k++)
                    {
                        Particles[i, j, k].Update(keyboardState, mouseState, delta);
                    }
                }
            }

            // only rebuild the chunk if a particle move
            if (IsParticleMove)
            {
                BuildMesh();
                IsParticleMove = false;
            }
        }


        private void AddVertex(Particle particle, Vector3 vertexPosition, bool isParticleMerged)
        {
            Vertex vertex = new Vertex(position: vertexPosition, color: particle.GetColor());

            int vertexHashCode = vertexPosition.GetHashCode();

            if (!verticesTemp.ContainsKey(vertexHashCode) && isParticleMerged)
            {
                verticesTemp.Add(vertexPosition.GetHashCode(), vertex);

                VerticesFastIndexOf.Add(vertex, verticesIndex);
                Vertices.Add(vertex);

                verticesIndex++;
            }

            if (!isParticleMerged)
            {
                VerticesFastIndexOf.Add(vertex, verticesIndex);
                Vertices.Add(vertex);

                quadVertexIndices.Add(verticesIndex);
                
                verticesIndex++;

                return;
            }

            quadVertexIndices.Add(VerticesFastIndexOf[verticesTemp[vertexHashCode]]);
        }

        private void AddQuadIndices()
        {
            for (int i = 0; i < MeshInstance.Quad.Indices.Length; i++)
            {
                Indices.Add((uint)quadVertexIndices[(int)MeshInstance.Quad.Indices[i]]);
            }


            quadVertexIndices.Clear();

            //Console.Write("[ ");

            //for (int i = 0; i < Indices.Count; i++)
            //{
            //    Console.Write($"{Indices[i]}, ");
            //}

            //Console.Write("]\n");
        }

        private bool IsParticleMerged(Particle particle, ParticleSide particleSide)
        {
            List<Particle> particleAdjacents = World.GetAllAdjacentParticlesAtSide(particle, ParticleSide.FrontAndBack);

            bool isParticleMerged = false;

            int particleAdjacentsCount = 0;


            for (int h = 0; h < particleAdjacents.Count; h++)
            {
                if (particleAdjacents[h].Type == particle.Type)
                {
                    particleAdjacentsCount++;
                }
            }

            if (particleAdjacentsCount == 8)
            {
                isParticleMerged = true;
            }

            return isParticleMerged;
        }

        public bool IsParticle(ParticleType particleType, int i, int j, int k)
        {
            // particle chunk array bountry check
            if (i < 0 || i > Settings.ChunkSize - 1 ||
                j < 0 || j > Settings.ChunkSize - 1 ||
                k < 0 || k > Settings.ChunkSize - 1)
            {
                return false;
            }

            return Particles[i, j, k].Type == particleType;
        }

        public bool IsParticleEmpty(int i, int j, int k)
        {
            // particle chunk array bountry check
            if (i < 0 || i > Settings.ChunkSize - 1 ||
                j < 0 || j > Settings.ChunkSize - 1 ||
                k < 0 || k > Settings.ChunkSize - 1)
            {
                return true;
            }

            return Particles[i, j, k].Type == ParticleType.Air;
        }

        public int GetChunkHash()
        {
            return Helpers.SnapToGrid(Position, Settings.ChunkSize).GetHashCode();
        }

        public static int GetChunkHash(Vector3i particlePosition)
        {
            return Helpers.SnapToGrid(particlePosition, Settings.ChunkSize).GetHashCode();
        }

        // get chunk position base on particle position
        public static Vector3i GetChunkPosition(Vector3i particlePosition)
        {
            return Helpers.SnapToGrid(particlePosition, Settings.ChunkSize);
        }
    }
}
