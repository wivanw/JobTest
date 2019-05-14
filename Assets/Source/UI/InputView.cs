using System;
using EventHabs;
using UnityEngine;
using UnityEngine.UI;
using VBCM;

namespace UI
{
    public class InputView : MonoBehaviour, GameRessetEvent.IBindable, GameRessetEvent.IEventSource
    {
        [SerializeField] private Button _ressetBtn;
        public Hub<GameRessetEvent, object, object>.ActionsPack BindActions { get; set; }
        public event Action Event;

        public InputView()
        {
            GameRessetEvent.Builder(this).Build();
        }

        private void Awake()
        {
            _ressetBtn.onClick.AddListener(() => Event?.Invoke());
        }
    }
}