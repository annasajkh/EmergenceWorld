using EmergenceWorld.Scripts.Core;

namespace EmergenceWorld.Scripts
{
    public class Program
    {

        public static Game game = new Game("Emergence World", 960, 540);

        static void Main(string[] args)
        {
            game.Run();
            game.Dispose();
        }
    }
}