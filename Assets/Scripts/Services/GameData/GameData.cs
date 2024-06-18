using Services.DI.Container;

namespace Services.GameData
{
    public class GameData : IContainerMember
    {
        private PackConfig _packConfig;
        private Level _currentLevel;
        public PackConfig Config => _packConfig;

        public Level CurrentLevel
        {
            get => _currentLevel;
            set => _currentLevel = value;
        }

        public void Initialization()
        {
        
        }

        public void SetConfigData(PackConfig data)
        {
            _packConfig = data;
        }
    
        public void Dispose()
        {
        
        }
    }
}
