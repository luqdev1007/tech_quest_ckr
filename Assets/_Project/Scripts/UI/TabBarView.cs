using System;
using System.Collections.Generic;
using System.Linq;
using TestTask.Core.Tabs;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TestTask.UI
{
    public class TabBarView : MonoBehaviour
    {
        [Serializable]
        public class TabButtonEntry
        {
            public TabType Type;
            public Button Button;
            public GameObject Highlight;
        }

        [SerializeField] private List<TabButtonEntry> _tabButtons;

        public IObservable<TabType> TabClicked { get; private set; }

        private void Awake()
        {
            TabClicked = Observable.Merge(_tabButtons.Select(entry =>
                entry.Button.OnClickAsObservable().Select(_ => entry.Type)));
        }

        public void SetActiveTab(TabType active)
        {
            foreach (var entry in _tabButtons)
                entry.Highlight.SetActive(entry.Type == active);
        }
    }
}
