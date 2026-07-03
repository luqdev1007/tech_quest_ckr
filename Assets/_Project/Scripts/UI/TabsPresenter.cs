using System;
using System.Collections.Generic;
using TestTask.Core.Tabs;
using UniRx;
using Zenject;

namespace TestTask.UI
{
    public sealed class TabsPresenter : IInitializable, IDisposable
    {
        private readonly ITabsService _tabsService;
        private readonly TabBarView _tabBarView;
        private readonly IEnumerable<ITabView> _tabViews;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public TabsPresenter(ITabsService tabsService, TabBarView tabBarView, IEnumerable<ITabView> tabViews)
        {
            _tabsService = tabsService;
            _tabBarView = tabBarView;
            _tabViews = tabViews;
        }

        public void Initialize()
        {
            _tabBarView.TabClicked
                .Subscribe(type => _tabsService.SwitchTo(type))
                .AddTo(_disposables);

            _tabsService.Active
                .Subscribe(active =>
                {
                    foreach (var view in _tabViews)
                        view.SetVisible(view.Type == active);

                    _tabBarView.SetActiveTab(active);
                })
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
