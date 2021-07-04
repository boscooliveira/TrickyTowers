namespace GameProject.TrickyTowers.Model
{
    public interface IPlayerData
    {
        EPlayerInputType Input { get; }
        int Lives { get; }
        void LoseLife();
    }
}