using Services.DI.Container;
using Services.DI.Installer;
using UnityEngine;

namespace Services.Downloader
{
    public class DownloadServiceInstaller : MonoInstaller
    {
        [SerializeField] private GithubContentProvider _githubContentProvider;
        [SerializeField] private GoogleDiskContentProvider _googleDiskContentProvider;
        public override void InstallBindings(IContainer container)
        {
            DownloadService downloadService = new DownloadService(_googleDiskContentProvider);
            container.AddInstance(downloadService);
        }
    }
}
