using System;
using System.Collections.Generic;
using System.Linq;
using TestTask.Core.Tabs;
using TMPro;
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
            {
                bool isActive = entry.Type == active;
                entry.Highlight.SetActive(isActive);

                // костыль
                var buttonText = entry.Button.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.color = isActive
                        ? new Color32(0xFF, 0xBC, 0x12, 0xFF) 
                        : Color.white;                   
                }
            }
        }
    }
}