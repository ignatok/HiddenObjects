using Services.DI.Container;
using UnityEngine;

namespace Services.DI.Installer
{
    public abstract class MonoInstaller: MonoBehaviour, IInstaller
    {
        public abstract void InstallBindings(IContainer container);
    }
}