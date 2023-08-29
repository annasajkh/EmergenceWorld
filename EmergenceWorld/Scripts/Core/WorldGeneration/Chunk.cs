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
        public Dictionary<VoxelVertex, int> VerticesFastIndexOf { get; private set; }
        public List<VoxelVertex> Vertices { get; private set; }
        public List<uint> Indices { get; private set; }

        private Dictionary<int, VoxelVertex> verticesTemp = new Dictionary<int, VoxelVertex>();

        private List<int> quadVertexIndices = new List<int>();

        private int verticesIndex = 0;

        public Chunk(Vector3i position, World world)
        {
            Position = position;
            World = world;
            Voxels = new Voxel[Settings.ChunkSize, Settings.ChunkSize, Settings.ChunkSize];

            VerticesFastIndexOf = new Dictionary<VoxelVertex, int>();
            Vertices = new List<VoxelVertex>();
            Indices = new List<uint>();

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
            VoxelType[] voxelTypes = new VoxelType[] { VoxelType.Water, VoxelType.Water, VoxelType.Water, VoxelType.Water, VoxelType.Sand };

            for (int i = 0; i < Voxels.GetLength(0); i++)
            {
                for (int j = 0; j < Voxels.GetLength(1); j++)
                {
                    for (int k = 0; k < Voxels.GetLength(2); k++)
                    {
                        // voxel position is its chunk position with offset depending on its index
                        Vector3i voxelPosition = new Vector3i(Position.X + i, Position.Y + j, Position.Z + k);
                        VoxelType voxelType = World.Noise.GetNoise(Position.X + i * 2, Position.Y + j * 2, Position.Z + k * 2) > 0 ? voxelTypes[Game.Random.Next() % voxelTypes.Length] : VoxelType.Air;


                        Voxels[i, j, k] = new Voxel(position: voxelPosition, i: i, j: j, k: k, type: voxelType, chunk: this);
                    }
                }
            }

            if (!World.Chunks.ContainsKey(GetChunkHash()))
            {
                World.Chunks.Add(GetChunkHash(), this);
            }

            BuildMesh();
        }


        public void BuildMesh()
        {
            for (int i = 0; i < Voxels.GetLength(0); i++)
            {
                for (int j = 0; j < Voxels.GetLength(1); j++)
                {
                    for (int k = 0; k < Voxels.GetLength(2); k++)
                    {
                        Voxel voxel = Voxels[i, j, k];

                        if (!(voxel.Type == VoxelType.Air))
                        {
                            // voxel actual position in world coordinate and not voxel coordinate
                            Vector3 voxelActualPosition = voxel.Position;


                            // check front this voxel is if it is an air or not
                            if (IsVoxelEmpty(i, j, k + 1))
                            {
                                // build front quad

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[0].Position);

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[1].Position);

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[2].Position);

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[3].Position);

                                AddQuadIndices();
                            }

                            // check back this voxel is if it is an air or not
                            if (IsVoxelEmpty(i, j, k - 1))
                            {
                                // build back quad

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[4].Position);

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[5].Position);

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[6].Position);

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[7].Position);

                                AddQuadIndices();
                            }


                            // check left this voxel is if it is an air or not
                            if (IsVoxelEmpty(i - 1, j, k))
                            {
                                // build left quad

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[8].Position);

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[9].Position);

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[10].Position);

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[11].Position);

                                AddQuadIndices();
                            }


                            // check right this voxel is if it is an air or not
                            if (IsVoxelEmpty(i + 1, j, k))
                            {
                                // build right quad

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[12].Position);

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[13].Position);

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[14].Position);

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[15].Position);

                                AddQuadIndices();
                            }



                            // check above this voxel is if it is an air or not
                            if (IsVoxelEmpty(i, j + 1, k))
                            {

                                // build top quad

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[16].Position);

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[17].Position);

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[18].Position);

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[19].Position);

                                AddQuadIndices();

                            }

                            // check bellow this voxel is if it is an air or not
                            if (IsVoxelEmpty(i, j - 1, k))
                            {
                                // build bottom quad

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[20].Position);

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[21].Position);

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[22].Position);

                                AddVertex(voxel, voxelActualPosition + VoxelMeshInstance.VoxelCube.Vertices[23].Position);

                                AddQuadIndices();
                            }
                        }
                    }
                }
            }

            verticesIndex = 0;

            Mesh.Vertices = Helpers.VoxelVerticesBuilder(VerticesFastIndexOf.Keys.ToArray());
            Mesh.Indices = Indices.ToArray();

            Console.WriteLine(VerticesFastIndexOf.Keys.Count);
            Console.WriteLine(Indices.Count);

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


        private void AddVertex(Voxel voxel, Vector3 vertexPosition)
        {
            VoxelVertex vertex = new VoxelVertex(vertexPosition, voxel.GetColor());

            int vertexHashCode = vertexPosition.GetHashCode();

            // merge neighboring voxels
            if (!verticesTemp.ContainsKey(vertexHashCode))
            {
                verticesTemp.Add(vertexPosition.GetHashCode(), vertex);

                VerticesFastIndexOf.Add(vertex, verticesIndex);
                Vertices.Add(vertex);

                verticesIndex++;
            }
            else
            {
                if (verticesTemp[vertexHashCode].Color != voxel.GetColor())
                {
                    VerticesFastIndexOf.Add(vertex, verticesIndex);
                    Vertices.Add(vertex);

                    quadVertexIndices.Add(verticesIndex);

                    verticesIndex++;

                    return;
                }
            }

            quadVertexIndices.Add(VerticesFastIndexOf[verticesTemp[vertexHashCode]]);
        }


        private void AddQuadIndices()
        {
            for (int i = 0; i < VoxelMeshInstance.VoxelQuad.Indices.Length; i++)
            {
                Indices.Add((uint)quadVertexIndices[(int)VoxelMeshInstance.VoxelQuad.Indices[i]]);
            }


            quadVertexIndices.Clear();

            //Console.Write("[ ");

            //for (int i = 0; i < Indices.Count; i++)
            //{
            //    Console.Write($"{Indices[i]}, ");
            //}

            //Console.Write("]\n");
        }

        /// <summary>
        /// Check if vertex need to be merged
        /// </summary>
        /// <param name="voxel"></param>
        /// <param name="voxelSide"></param>
        /// <returns></returns>
        //private bool CheckIsVoxelMerged(Voxel voxel, VoxelSide voxelSide)
        //{
        //    List<Voxel> voxelAdjacents = World.GetAllAdjacentVoxelsAtSide(voxel, voxelSide);

        //    bool isVoxelMerged = false;

        //    int voxelAdjacentsCount = 0;


        //    for (int i = 0; i < voxelAdjacents.Count; i++)
        //    {
        //        if (voxelAdjacents[i].Type == voxel.Type)
        //        {
        //            voxelAdjacentsCount++;
        //        }
        //    }

        //    if (voxelAdjacentsCount == 8)
        //    {
        //        isVoxelMerged = true;
        //    }

        //    return isVoxelMerged;
        //}


        /// <summary>
        /// Get voxel in this chunk, if it doesn't exist return null
        /// </summary>
        /// <param name="voxelType"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public Voxel? GetVoxel(int i, int j, int k)
        {
            // voxel chunk array bountry check
            if (i < 0 || i > Settings.ChunkSize - 1 ||
                j < 0 || j > Settings.ChunkSize - 1 ||
                k < 0 || k > Settings.ChunkSize - 1)
            {
                return null;
            }

            return Voxels[i, j, k];
        }


        /// <summary>
        /// Check voxel in this chunk with a spesific type, if it doesn't exist return false
        /// </summary>
        /// <param name="voxelType"></param>
        /// <param name="voxelPosition"></param>
        public bool VoxelIsType(VoxelType voxelType, int i, int j, int k)
        {
            Voxel? voxel = GetVoxel(i, j, k);

            // check if the voxel exist this chunk or not
            if (voxel == null)
            {
                return false;
            }

            return voxel.Type == voxelType;
        }

        /// <summary>
        /// Check voxel in this chunk if it's empty or not, empty means the voxel is either air or doesn't exist
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public bool IsVoxelEmpty(int i, int j, int k)
        {
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


        public static int GetChunkHash(Vector3i voxelPosition)
        {
            return Helpers.SnapToGrid(voxelPosition, Settings.ChunkSize).GetHashCode();
        }


        /// <summary>
        /// Get chunk position base on voxel position
        /// </summary>
        /// <param name="voxelPosition"></param>
        /// <returns></returns>
        public static Vector3i GetChunkPosition(Vector3i voxelPosition)
        {
            return (Vector3i)Helpers.SnapToGrid(voxelPosition, Settings.ChunkSize);
        }
    }
}
