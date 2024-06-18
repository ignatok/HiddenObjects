using System;
using System.Threading;
using Services.DI.Context;
using Services.Downloader;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.MainScreen
{
    public class LevelButton : BaseBarButton<Level>, IReleasable, IPointerClickHandler
    {
        public event Action<Level> Click;

        [SerializeField] private Image _previewImage;
        [SerializeField] private Image progressIndicator;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private GameObject _completedMask;

        [SerializeField] private TextMeshProUGUI _levelName;
        [SerializeField] private TextMeshProUGUI _downloadErrorText;

        private CancellationTokenSource _cancellationToken;
        public string LevelName { get; }

        protected override async void OnSetData(Level data)
        {
            if (Debug.isDebugBuild)
            {
                _levelName.gameObject.SetActive(true);
            }

            var downloadService = ProjectContext.GetInstance<DownloadService>();
            SetPreview(data);

            if (data.Progress.IsExist)
            {
                data.Progress.Preload();
            }

            _levelName.text = data.Resources.Name;
            _progressText.text = $"{data.Progress.Counter}/{data.Resources.ItemsCount}";
            if (data.Progress.Counter <= 0)
            {
                progressIndicator.fillAmount = 1;
            }
            else
            {
                progressIndicator.fillAmount = 1 - (float)(data.Progress.Counter / data.Resources.ItemsCount);
            }


            _completedMask.SetActive(data.Progress.IsComplete);
            _levelName.text = LevelName;
        }

        protected override void OnClick()
        {
            if (Data.Progress.IsComplete)
            {
                Data.Progress.Counter = Data.Resources.ItemsCount;
                Data.Restart();
            }

            Click?.Invoke(Data);
        }

        private async void SetPreview(Level data)
        {
            Sprite sprite = await data.Resources.GetPreview();
            _downloadErrorText.gameObject.SetActive(sprite == null);

            if (_previewImage != null)
            {
                _previewImage.sprite = sprite;
            }

            data.Resources.Data.Background = sprite;
        }

        public void OnRelease()
        {
            _cancellationToken?.Cancel();
            Data = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }
    }

    public interface IReleasable
    {
        public void OnRelease();
    }
}