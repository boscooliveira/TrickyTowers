namespace GameProject.TrickyTowers.Model
{
    public class PlayerData : IPlayerData
    {
        public EPlayerInputType Input { get; private set; }
        public int Lives { get; private set; }

        public PlayerData(int lives, EPlayerInputType input)
        {
            Lives = lives;
            Input = input;
        }

        public void LoseLife()
        {
            if (Lives > 0)
                Lives--;
        }
    }

    public enum EPlayerInputType
    {
        Player1,
        RandomTargetAI
    }
}
