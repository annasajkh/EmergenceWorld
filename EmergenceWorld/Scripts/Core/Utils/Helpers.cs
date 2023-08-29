using EmergenceWorld.Scripts.Core.Containers;
using EmergenceWorld.Scripts.Core.VertexArrayObjects;
using OpenTK.Mathematics;

namespace EmergenceWorld.Scripts.Utils
{
    public static class Helpers
    {
        private static float SnapToGrid(float value, float gridSize)
        {
            return (float)(MathHelper.Floor(value / gridSize) * gridSize);
        }

        public static Vector3 SnapToGrid(Vector3 value, int gridSize)
        {
            return new Vector3(SnapToGrid(value.X, gridSize), SnapToGrid(value.Y, gridSize), SnapToGrid(value.Z, gridSize));
        }


        public static Color4 LerpColor(Color4 color1, Color4 color2, float t)
        {
            return new Color4(color1.R + (color2.R - color1.R) * t,
                              color1.G + (color2.G - color1.G) * t,
                              color1.B + (color2.B - color1.B) * t,
                              color1.A + (color2.A - color1.A) * t);
        }

        // t is between 0 - 1
        public static Color4 Lerp3Color(Color4 color1, Color4 color2, Color4 color3, float t)
        {
            if (t < 0.5f)
            {
                return LerpColor(color1, color2, t * 2);
            }
            else
            {
                return LerpColor(color2, color3, (t - 0.5f) * 2);
            }
        }

        public static float[] VoxelVerticesBuilder(VoxelVertex[] vertices)
        {
            float[] verticesResult = new float[VoxelVertexArrayObject.VoxelAttributeSize * vertices.Length];

            int index = 0;

            for (int i = 0; i < verticesResult.Length; i += VoxelVertexArrayObject.VoxelAttributeSize)
            {
                verticesResult[i] = vertices[index].Position.X;
                verticesResult[i + 1] = vertices[index].Position.Y;
                verticesResult[i + 2] = vertices[index].Position.Z;

                verticesResult[i + 3] = vertices[index].Color.R;
                verticesResult[i + 4] = vertices[index].Color.G;
                verticesResult[i + 5] = vertices[index].Color.B;
                verticesResult[i + 6] = vertices[index].Color.A;

                index++;
            }


            return verticesResult;

        }
    }
}