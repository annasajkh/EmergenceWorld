namespace EmergenceWorld.Scripts.Core.Components
{
    public class Settings
    {
        public static int ChunkSize { get; } = 64; // in particle unit
        public static int ParticleSize { get; } = 50;
        public static int ChunkFullSize { get; } = ChunkSize * ParticleSize; // chunk actual size
    }
}
