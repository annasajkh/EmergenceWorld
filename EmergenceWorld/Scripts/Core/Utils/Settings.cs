using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace EmergenceWorld.Scripts.Core.Utils
{
    public static class Settings
    {
        public static int ChunkSize { get; } = 16; // in voxel unit
        public static int VoxelSize { get; } = 4;
        public static int ChunkFullSize { get; } = ChunkSize * VoxelSize; // chunk actual size

        public static int RenderDistance { get; } = 5;

        public static void Init()
        {
            // Settings

            GL.Enable(EnableCap.CullFace);

            GL.Enable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            StbImage.stbi_set_flip_vertically_on_load(1);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);


            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
        }
    }
}
