using System.Collections;
using Grace.DependencyInjection.Attributes;
using RedPanda.Project.Data;
using RedPanda.Project.Interfaces;
using RedPanda.Project.Services.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RedPanda.Project.UI
{
    public class PromoElementView : View
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Image backImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Sprite commonBackSprite;
        [SerializeField] private Sprite rareBackSprite;
        [SerializeField] private Sprite epicBackSprite;
        
        private IResourceProvider _resourceProvider;
        
        public delegate void ClickDelegate();
        public event ClickDelegate OnClick;

        [Import]
        public void Inject(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
        }

        private void Start()
        {
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnButtonClick);
        }

        public void SetPromo(IPromoModel promo)
        {
            titleText.text = promo.Title;
            backImage.sprite = promo.Rarity switch
            {
                PromoRarity.Common => commonBackSprite,
                PromoRarity.Rare => rareBackSprite,
                PromoRarity.Epic => epicBackSprite,
                _ => throw new System.Exception("PromoElementView background sprite out of range")
            };
            iconImage.sprite = _resourceProvider.GetIcon(promo.GetIcon());
            priceText.text = $"x{promo.Cost}";
        }

        private void OnButtonClick()
        {
            StopAllCoroutines();
            StartCoroutine(ClickScaleAnimation());
            
            OnClick?.Invoke();
        }

        private IEnumerator ClickScaleAnimation()
        {
            const float scaleFrom = 1f;
            const float scaleTo = 0.9f;
            const float duration = 0.3f;
            
            yield return DoScale(scaleFrom, scaleTo, duration / 2f);
            yield return DoScale(scaleTo, scaleFrom, duration / 2f);
        }

        private IEnumerator DoScale(float from, float to, float duration)
        {
            var waitForEndOfFrame = new WaitForEndOfFrame();
            
            float scaleDelta = to - from;
            float timeDown = duration;

            while (timeDown > 0f)
            {
                var t = 1f - timeDown / duration;
                transform.localScale = Vector3.one * (from + scaleDelta * t);
                yield return waitForEndOfFrame;
                timeDown -= Time.deltaTime;
            }
        }
    }
}