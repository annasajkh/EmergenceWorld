using EmergenceWorld.Scripts.Core.Abstractions;
using OpenTK.Graphics.OpenGL4;

namespace EmergenceWorld.Scripts.Core.OpenGLObjects
{
    public class Shader : OpenGLObject
    {
        public static int PositionAttributeSize { get; } = 3;
        public static int ColorAttributeSize { get; } = 4;
        public static int NormalAttributeSize { get; } = 3;
        public static int TextureCoordinateAttributeSize { get; } = 2;

        private static Dictionary<string, int> uniformLocations = new Dictionary<string, int>();

        public Shader(string vertexShaderPath, string geometryShaderPath, string fragmentShaderPath)
        {
            string vertexShaderSource = File.ReadAllText(vertexShaderPath);
            string geometryShaderSource = File.ReadAllText(geometryShaderPath);
            string fragmentShaderSource = File.ReadAllText(fragmentShaderPath);


            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);

            int geometryShader = GL.CreateShader(ShaderType.GeometryShader);
            GL.ShaderSource(geometryShader, geometryShaderSource);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);


            GL.CompileShader(vertexShader);
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int successVertex);

            if (successVertex == 0)
            {
                string infoLog = GL.GetShaderInfoLog(vertexShader);
                Console.WriteLine(infoLog);
            }


            GL.CompileShader(geometryShader);
            GL.GetShader(geometryShader, ShaderParameter.CompileStatus, out int successGeometry);

            if (successGeometry == 0)
            {
                string infoLog = GL.GetShaderInfoLog(geometryShader);
                Console.WriteLine(infoLog);
            }


            GL.CompileShader(fragmentShader);
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int successFragment);

            if (successFragment == 0)
            {
                string infoLog = GL.GetShaderInfoLog(fragmentShader);
                Console.WriteLine(infoLog);
            }


            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, geometryShader);
            GL.AttachShader(Handle, fragmentShader);

            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success);

            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(infoLog);
            }


            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, geometryShader);
            GL.DetachShader(Handle, fragmentShader);


            GL.DeleteShader(vertexShader);
            GL.DeleteShader(geometryShader);
            GL.DeleteShader(fragmentShader);
        }

        public override void Bind()
        {
            GL.UseProgram(Handle);
        }

        public override void Unbind()
        {
            GL.UseProgram(0);
        }

        public int GetUniformLocation(string name)
        {
            if (uniformLocations.ContainsKey(name))
            {
                return uniformLocations[name];
            }
            else
            {
                uniformLocations.Add(name, GL.GetUniformLocation(Handle, name));
                return uniformLocations[name];
            }
        }

        public override void Dispose()
        {
            Console.WriteLine($"Shader Program: {Handle} is Unloaded");

            GL.DeleteProgram(Handle);
            GC.SuppressFinalize(this);
        }
    }
}