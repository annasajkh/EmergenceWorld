using EmergenceWorld.Scripts.Core.Components;
using EmergenceWorld.Scripts.Core.Containers;
using EmergenceWorld.Scripts.Core.Scenes;
using EmergenceWorld.Scripts.Core.Utils;
using EmergenceWorld.Scripts.Core.Voxels;
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
        public Voxel[,,] Voxels { get; private set; }
        public Mesh Mesh { get; private set; }
        public Dictionary<Vertex, int> VerticesFastIndexOf { get; private set; }
        public List<Vertex> Vertices { get; private set; }
        public List<uint> Indices { get; private set; }

        private Dictionary<int, Vertex> verticesTemp = new Dictionary<int, Vertex>();

        private List<int> quadVertexIndices = new List<int>();

        private int verticesIndex = 0;

        public Chunk(Vector3i position, World world)
        {
            Position = position;
            World = world;
            Voxels = new Voxel[Settings.ChunkSize, Settings.ChunkSize, Settings.ChunkSize];

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
            //VoxelType[] particleTypes = new VoxelType[] { VoxelType.Water, VoxelType.Sand };

            // World.noise.GetNoise(Position.X + i * 2, Position.Y + j * 2, Position.Z + k * 2) > 0 ? particleTypes[World.random.Next() % particleTypes.Length] : VoxelType.Empty

            for (int i = 0; i < Voxels.GetLength(0); i++)
            {
                for (int j = 0; j < Voxels.GetLength(1); j++)
                {
                    for (int k = 0; k < Voxels.GetLength(2); k++)
                    {

                        // particle position is its chunk position with offset depending on its index
                        Voxels[i, j, k] = new Voxel(position: new Vector3i(Position.X + i,
                                                                                 Position.Y + j,
                                                                                 Position.Z + k),
                                                          i: i,
                                                          j: j,
                                                          k: k,
                                                          type: Game.Noise.GetNoise(Position.X + i * 2, Position.Y + j * 2, Position.Z + k * 2) > 0 ? VoxelType.Water : VoxelType.Air,
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
            for (int i = 0; i < Voxels.GetLength(0); i++)
            {
                for (int j = 0; j < Voxels.GetLength(1); j++)
                {
                    for (int k = 0; k < Voxels.GetLength(2); k++)
                    {
                        Voxel particle = Voxels[i, j, k];

                        if (!(particle.Type == VoxelType.Air))
                        {
                            // particle actual position in world coordinate and not particle coordinate
                            Vector3 particleActualPosition = particle.Position;


                            // check front this particle is if it is an air or not
                            if (IsVoxelEmpty(i, j, k + 1))
                            {
                                bool isVoxelMerged = IsVoxelMerged(particle, VoxelSide.FrontAndBack);

                                // build front quad

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[0].Position, isVoxelMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[1].Position, isVoxelMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[2].Position, isVoxelMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[3].Position, isVoxelMerged);

                                AddQuadIndices();
                            }



                            // check right this particle is if it is an air or not
                            if (IsVoxelEmpty(i + 1, j, k))
                            {
                                bool isVoxelMerged = IsVoxelMerged(particle, VoxelSide.LeftAndRight);

                                // build right quad

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[12].Position, isVoxelMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[13].Position, isVoxelMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[14].Position, isVoxelMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[15].Position, isVoxelMerged);

                                AddQuadIndices();
                            }



                            // check back this particle is if it is an air or not
                            if (IsVoxelEmpty(i, j, k - 1))
                            {
                                bool isVoxelMerged = IsVoxelMerged(particle, VoxelSide.FrontAndBack);

                                // build back quad

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[4].Position, isVoxelMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[5].Position, isVoxelMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[6].Position, isVoxelMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[7].Position, isVoxelMerged);

                                AddQuadIndices();
                            }



                            // check left this particle is if it is an air or not
                            if (IsVoxelEmpty(i - 1, j, k))
                            {
                                bool isVoxelMerged = IsVoxelMerged(particle, VoxelSide.LeftAndRight);

                                // build left quad

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[8].Position, isVoxelMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[9].Position, isVoxelMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[10].Position, isVoxelMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[11].Position, isVoxelMerged);

                                AddQuadIndices();
                            }



                            // check above this particle is if it is an air or not
                            if (IsVoxelEmpty(i, j + 1, k))
                            {
                                bool isVoxelMerged = IsVoxelMerged(particle, VoxelSide.TopAndBottom);

                                // build top quad

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[16].Position, isVoxelMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[17].Position, isVoxelMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[18].Position, isVoxelMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[19].Position, isVoxelMerged);

                                AddQuadIndices();

                            }



                            // check bellow this particle is if it is an air or not
                            if (IsVoxelEmpty(i, j - 1, k))
                            {
                                bool isVoxelMerged = IsVoxelMerged(particle, VoxelSide.TopAndBottom);

                                // build bottom quad

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[20].Position, isVoxelMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[21].Position, isVoxelMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[22].Position, isVoxelMerged);

                                AddVertex(particle, particleActualPosition + MeshInstance.Cube.Vertices[23].Position, isVoxelMerged);

                                AddQuadIndices();
                            }
                        }
                    }
                }
            }

            verticesIndex = 0;

            Mesh.Vertices = Helpers.VerticesBuilder(VerticesFastIndexOf.Keys.ToArray());
            Mesh.Indices = Indices.ToArray();

            Mesh.Scale = Vector3.One * Settings.VoxelSize;

            VerticesFastIndexOf.Clear();
            Vertices.Clear();
            Indices.Clear();

            verticesTemp.Clear();
        }

        public void Update(KeyboardState keyboardState, MouseState mouseState, float delta)
        {

            for (int i = 0; i < Voxels.GetLength(0); i++)
            {
                for (int j = 0; j < Voxels.GetLength(1); j++)
                {
                    for (int k = 0; k < Voxels.GetLength(2); k++)
                    {
                        Voxels[i, j, k].Update(keyboardState, mouseState, delta);
                    }
                }
            }
        }


        private void AddVertex(Voxel particle, Vector3 vertexPosition, bool isVoxelMerged)
        {
            Vertex vertex = new Vertex(position: vertexPosition, color: particle.GetColor());

            int vertexHashCode = vertexPosition.GetHashCode();

            if (!verticesTemp.ContainsKey(vertexHashCode) && isVoxelMerged)
            {
                verticesTemp.Add(vertexPosition.GetHashCode(), vertex);

                VerticesFastIndexOf.Add(vertex, verticesIndex);
                Vertices.Add(vertex);

                verticesIndex++;
            }

            if (!isVoxelMerged)
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

        private bool IsVoxelMerged(Voxel particle, VoxelSide particleSide)
        {
            List<Voxel> particleAdjacents = World.GetAllAdjacentVoxelsAtSide(particle, VoxelSide.FrontAndBack);

            bool isVoxelMerged = false;

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
                isVoxelMerged = true;
            }

            return isVoxelMerged;
        }

        public bool IsVoxel(VoxelType particleType, int i, int j, int k)
        {
            // particle chunk array bountry check
            if (i < 0 || i > Settings.ChunkSize - 1 ||
                j < 0 || j > Settings.ChunkSize - 1 ||
                k < 0 || k > Settings.ChunkSize - 1)
            {
                return false;
            }

            return Voxels[i, j, k].Type == particleType;
        }

        public bool IsVoxelEmpty(int i, int j, int k)
        {
            // particle chunk array bountry check
            if (i < 0 || i > Settings.ChunkSize - 1 ||
                j < 0 || j > Settings.ChunkSize - 1 ||
                k < 0 || k > Settings.ChunkSize - 1)
            {
                return true;
            }

            return Voxels[i, j, k].Type == VoxelType.Air;
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
