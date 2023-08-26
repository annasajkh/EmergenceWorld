using OpenTK.Mathematics;

namespace EmergenceWorld.Scripts.Core.Containers
{
    public class Vertex
    {
        public Vector3 Position { get; set; }
        public Color4 Color { get; set; }

        public Vertex(Vector3 position, Color4 color)
        {
            Position = position;
            Color = color;
        }

        public void UpdateVertex(Vector3 position, Color4 color)
        {
            Position = position;
            Color = color;
        }

        public override string ToString()
        {
            return $"Position: [{Position.X}, {Position.Y}, {Position.Z}]\nColor: [{Color.R}, {Color.G}, {Color.B}, {Color.A}]\n]";
        }
    }
}