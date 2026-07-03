using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TestTask.Configs;
using TestTask.Core.Network;
using TestTask.Core.Tabs;
using TestTask.Features.Breeds.Dto;
using TestTask.UI;
using UniRx;
using UnityEngine;
using Zenject;

namespace TestTask.Features.Breeds
{
    public sealed class BreedsPresenter : IInitializable, IDisposable
    {
        private readonly BreedsView _view;
        private readonly PopupView _popup;
        private readonly IRequestQueue _requestQueue;
        private readonly ITabsService _tabsService;
        private readonly BreedsConfig _config;
        private readonly BreedRowView.Factory _rowFactory;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly List<BreedRowView> _rows = new List<BreedRowView>();

        private CompositeDisposable _rowsDisposable = new CompositeDisposable();
        private IRequestHandle<BreedsListResponse> _listHandle;
        private IRequestHandle<BreedResponse> _detailsHandle;
        private BreedRowView _activeSpinnerRow;

        public BreedsPresenter(
            BreedsView view,
            PopupView popup,
            IRequestQueue requestQueue,
            ITabsService tabsService,
            BreedsConfig config,
            BreedRowView.Factory rowFactory)
        {
            _view = view;
            _popup = popup;
            _requestQueue = requestQueue;
            _tabsService = tabsService;
            _config = config;
            _rowFactory = rowFactory;
        }

        public void Initialize()
        {
            _popup.OkClicked
                .Subscribe(_ => _popup.Hide())
                .AddTo(_disposables);

            _tabsService.Active
                .Subscribe(tab =>
                {
                    if (tab == TabType.Breeds)
                        OnEnter();
                    else
                        OnLeave();
                })
                .AddTo(_disposables);
        }

        private void OnEnter()
        {
            ClearRows();
            _view.SetListLoading(true);

            _listHandle = _requestQueue.Enqueue(new BreedsListRequest(_config));
            AwaitListAsync(_listHandle).Forget();
        }

        private void OnLeave()
        {
            CancelActiveRequests();
            HideActiveSpinner();
            _view.SetListLoading(false);
            _popup.Hide();
        }

        private async UniTaskVoid AwaitListAsync(IRequestHandle<BreedsListResponse> handle)
        {
            BreedsListResponse response;
            try
            {
                response = await handle.Task;
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[Breeds] list request failed: {ex.Message}");
                _view.SetListLoading(false);
                return;
            }

            if (_listHandle != handle)
                return;

            _view.SetListLoading(false);

            var data = response?.Data;
            if (data == null || data.Count == 0)
            {
                Debug.LogWarning("[Breeds] breeds list empty");
                return;
            }

            var count = Mathf.Min(_config.BreedsToShow, data.Count);
            for (var i = 0; i < count; i++)
            {
                var breed = data[i];
                var row = _rowFactory.Create();
                row.SetData(i + 1, breed.Attributes?.Name);

                var id = breed.Id;
                row.Clicked
                    .Subscribe(_ => OnRowClicked(id, row))
                    .AddTo(_rowsDisposable);

                _rows.Add(row);
            }
        }

        private void OnRowClicked(string id, BreedRowView row)
        {
            CancelDetailsHandle();
            HideActiveSpinner();

            row.ShowSpinner();
            _activeSpinnerRow = row;

            _detailsHandle = _requestQueue.Enqueue(new BreedDetailsRequest(_config, id));
            AwaitDetailsAsync(_detailsHandle, row).Forget();
        }

        private async UniTaskVoid AwaitDetailsAsync(IRequestHandle<BreedResponse> handle, BreedRowView row)
        {
            BreedResponse response;
            try
            {
                response = await handle.Task;
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[Breeds] details request failed: {ex.Message}");
                row.HideSpinner();
                return;
            }

            if (_detailsHandle != handle)
                return;

            row.HideSpinner();

            var attributes = response?.Data?.Attributes;
            if (attributes == null)
            {
                Debug.LogWarning("[Breeds] breed details empty");
                return;
            }

            _popup.Show(attributes.Name, attributes.Description);
        }

        private void ClearRows()
        {
            _rowsDisposable.Dispose();
            _rowsDisposable = new CompositeDisposable();

            foreach (var row in _rows)
            {
                if (row != null)
                    UnityEngine.Object.Destroy(row.gameObject);
            }

            _rows.Clear();
        }

        private void CancelActiveRequests()
        {
            CancelListHandle();
            CancelDetailsHandle();
        }

        private void CancelListHandle()
        {
            if (_listHandle == null)
                return;

            _listHandle.Cancel();
            _listHandle = null;
        }

        private void CancelDetailsHandle()
        {
            if (_detailsHandle == null)
                return;

            _detailsHandle.Cancel();
            _detailsHandle = null;
        }

        private void HideActiveSpinner()
        {
            if (_activeSpinnerRow == null)
                return;

            _activeSpinnerRow.HideSpinner();
            _activeSpinnerRow = null;
        }

        public void Dispose()
        {
            CancelActiveRequests();
            _rowsDisposable.Dispose();
            _disposables.Dispose();
        }
    }
}
