using Game.Core.Input;
using VContainer.Unity;

namespace Game.Core
{
    public class CoreBootstrap : IInitializable
    {
        private readonly InputLogger _inputLogger;

        public CoreBootstrap(InputLogger inputLogger)
        {
            _inputLogger = inputLogger;
        }

        public void Initialize()
        {
            _inputLogger.Initialize();
        }
    }
}
