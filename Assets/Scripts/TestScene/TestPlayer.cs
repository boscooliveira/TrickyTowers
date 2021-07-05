using GameProject.TrickyTowers.Model;

namespace GameProject.TrickyTowers.TestScene
{
    public class TestPlayer : IPlayerData
    {
        public EPlayerInputType Input { get; set; }

        public int Lives => 5;

        public void LoseLife() { }

        public TestPlayer(EPlayerInputType input)
        {
            Input = input;
        }
    }
}
