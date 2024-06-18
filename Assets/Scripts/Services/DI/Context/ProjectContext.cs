using System.Collections.Generic;
using Services.DI.Container;
using Services.DI.Installer;
using Services.SharedUtils;
using UnityEngine;

namespace Services.DI.Context
{
    public class ProjectContext : MonoSingleton<ProjectContext>
    {
        [SerializeField] private List<MonoInstaller> monoInstallers;

        private readonly IContainer _container = new BaseContainer();

        public static T GetInstance<T>() where T : IContainerMember => Instance._container.GetInstance<T>();

        protected override void Awake()
        {
            base.Awake();

            foreach (var monoInstaller in monoInstallers)
            {
                monoInstaller.InstallBindings(_container);
            }
        }

        private void Start()
        {
            _container.Initialization();
        }

        private void Update()
        {
            _container.Tick();
        }

        private void OnDestroy()
        {
            _container.Dispose();
        }
    }
}