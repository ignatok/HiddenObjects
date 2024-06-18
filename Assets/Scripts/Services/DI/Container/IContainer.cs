using System;

namespace Services.DI.Container
{
    public interface IContainer: IInitializable, IDisposable, ITickable
    {
        T GetInstance<T>() where T : IContainerMember;
        void AddInstance(IContainerMember instance);
        void RemoveInstance(IContainerMember instance);
    }
}