namespace GameProject.TrickyTowers.TestScene.Editor.Config
{
    public interface IConfigMenuDrawer
    {
        EConfigMenuType Option { get; }
        void Draw(TestSceneController controller);
    }

    public enum EConfigMenuType
    {
        None = 0,
        Physics = 1,
        Gameplay = 2
    }
}
