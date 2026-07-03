using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TestTask.Configs;
using TestTask.Core.Network;
using TestTask.Core.Tabs;
using TestTask.Features.Weather.Dto;
using UniRx;
using UnityEngine;
using Zenject;

namespace TestTask.Features.Weather
{
    public sealed class WeatherPresenter : IInitializable, IDisposable
    {
        private readonly WeatherView _view;
        private readonly IRequestQueue _requestQueue;
        private readonly ITabsService _tabsService;
        private readonly WeatherConfig _config;
        private readonly Dictionary<string, Texture2D> _iconCache = new Dictionary<string, Texture2D>();
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private CancellationTokenSource _loopCts;
        private IRequestHandle<WeatherForecastResponse> _forecastHandle;
        private IRequestHandle<Texture2D> _iconHandle;

        public WeatherPresenter(WeatherView view, IRequestQueue requestQueue, ITabsService tabsService, WeatherConfig config)
        {
            _view = view;
            _requestQueue = requestQueue;
            _tabsService = tabsService;
            _config = config;
        }

        public void Initialize()
        {
            _tabsService.Active
                .Subscribe(tab =>
                {
                    if (tab == TabType.Weather)
                        StartPolling();
                    else
                        StopPolling();
                })
                .AddTo(_disposables);
        }

        private void StartPolling()
        {
            if (_loopCts != null)
                return;

            Debug.Log("[Weather] activated → start polling");
            _loopCts = new CancellationTokenSource();
            RunLoop(_loopCts.Token).Forget();
        }

        private void StopPolling()
        {
            if (_loopCts == null)
                return;

            Debug.Log("[Weather] deactivated → cancel all");
            _loopCts.Cancel();
            _loopCts.Dispose();
            _loopCts = null;

            CancelHandles();
        }

        private void CancelHandles()
        {
            _forecastHandle?.Cancel();
            _forecastHandle = null;

            _iconHandle?.Cancel();
            _iconHandle = null;
        }

        private async UniTaskVoid RunLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                CancelHandles();
                ProcessTick().Forget();

                try
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(_config.PollInterval), cancellationToken: ct);
                }
                catch (OperationCanceledException)
                {
                    // цикл остановлен деактивацией вкладки
                }
            }
        }

        private async UniTaskVoid ProcessTick()
        {
            var forecastHeaders = new Dictionary<string, string>
            {
                ["User-Agent"] = _config.UserAgent,
                ["Accept"] = "application/geo+json"
            };

            _forecastHandle = _requestQueue.Enqueue(new JsonGetRequest<WeatherForecastResponse>(_config.ForecastUrl, forecastHeaders));

            WeatherForecastResponse response;
            try
            {
                response = await _forecastHandle.Task;
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[Weather] forecast request failed: {ex.Message}");
                return;
            }

            var periods = response?.Properties?.Periods;
            if (periods == null || periods.Count == 0)
            {
                Debug.LogWarning("[Weather] forecast periods empty, skipping update");
                return;
            }

            var period = periods[0];
            _view.SetForecast($"Today: {period.Temperature}{period.TemperatureUnit}");

            if (string.IsNullOrEmpty(period.Icon))
            {
                Debug.LogWarning("[Weather] icon url empty, skipping icon update");
                return;
            }

            await UpdateIcon(period.Icon);
        }

        private async UniTask UpdateIcon(string iconUrl)
        {
            if (_iconCache.TryGetValue(iconUrl, out var cached))
            {
                _view.SetIcon(cached);
                return;
            }

            var iconHeaders = new Dictionary<string, string> { ["User-Agent"] = _config.UserAgent };
            _iconHandle = _requestQueue.Enqueue(new TextureGetRequest(iconUrl, iconHeaders));

            Texture2D texture;
            try
            {
                texture = await _iconHandle.Task;
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[Weather] icon request failed: {ex.Message}");
                return;
            }

            _iconCache[iconUrl] = texture;
            _view.SetIcon(texture);
        }

        public void Dispose()
        {
            StopPolling();
            _disposables.Dispose();
        }
    }
}
