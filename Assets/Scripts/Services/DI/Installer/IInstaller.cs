using Services.DI.Container;

namespace Services.DI.Installer
{
    public interface IInstaller
    {
        void InstallBindings(IContainer container);
    }
}