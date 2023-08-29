using EmergenceWorld.Scripts.Core.OpenGLObjects;
using StbImageSharp;

namespace EmergenceWorld.Scripts.Core.Managers
{
    public class ResourceManager
    {
        public Dictionary<string, Shader> Shaders { get; } = new Dictionary<string, Shader>();
        public Dictionary<string, Texture2D> Textures { get; } = new Dictionary<string, Texture2D>();

        public void AddShader(string shaderName)
        {
            Shaders.Add(shaderName, new Shader(vertexShaderPath: Path.GetFullPath($"Resources/Shaders/{shaderName}/shader.vert"),
                                               geometryShaderPath: Path.GetFullPath($"Resources/Shaders/{shaderName}/shader.geom"),
                                               fragmentShaderPath: Path.GetFullPath($"Resources/Shaders/{shaderName}/shader.frag")));
        }

        public void AddTexture(string name, string path)
        {
            Textures.Add(name, new Texture2D(ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha)));
        }
    }
}
