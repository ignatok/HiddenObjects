using Services.DI.Container;
using Services.DI.Installer;

namespace Services.GameData
{
    public class GameDataInstaller : MonoInstaller
    {
        public override void InstallBindings(IContainer container)
        {
            GameData gameData = new GameData();
            container.AddInstance(gameData);
        }
    }
}
