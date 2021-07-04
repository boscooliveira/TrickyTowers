using GameProject.TrickyTowers.Model;

namespace GameProject.TrickyTowers.TestScene
{
    public class TestPlayer : IPlayerData
    {
        public EPlayerInputType Input => EPlayerInputType.RandomTargetAI;

        public int Lives => 5;

        public void LoseLife() { }
    }
}
