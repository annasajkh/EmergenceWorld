using EmergenceWorld.Scripts.Core;

namespace EmergenceWorld.Scripts
{
    public class Program
    {
        static void Main(string[] args)
        {
            using(Game game = new Game("Emergence World", 960, 540))
            {
                game.Run();
            }
        }
    }
}