using EmergenceWorld.Scripts.Core.OpenGLObjects;

namespace EmergenceWorld.Scripts.Core.Managers
{
    public class ResourceManager
    {
        public Dictionary<string, Shader> Shaders { get; } = new Dictionary<string, Shader>();

        public void AddShader(string shaderName)
        {
            Shaders.Add(shaderName, new Shader(vertexShaderPath: Path.GetFullPath($"Resources/Shaders/{shaderName}/shader.vert"),
                                               geometryShaderPath: Path.GetFullPath($"Resources/Shaders/{shaderName}/shader.geom"),
                                               fragmentShaderPath: Path.GetFullPath($"Resources/Shaders/{shaderName}/shader.frag")));
        }
    }
}
