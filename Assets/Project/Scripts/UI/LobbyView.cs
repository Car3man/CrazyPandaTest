using UnityEngine;
using UnityEngine.UI;

namespace RedPanda.Project.UI
{
    public sealed class LobbyView : View
    {
        [SerializeField] private Button startButton;
        
        private void Start()
        {
            startButton.onClick.AddListener(OnButtonStartClick);
        }

        private void OnDestroy()
        {
            startButton.onClick.RemoveListener(OnButtonStartClick);
        }

        private void OnButtonStartClick()
        {
            UIService.DestroyView(this);
            UIService.CreateView<PromoView>();
        }
    }
}