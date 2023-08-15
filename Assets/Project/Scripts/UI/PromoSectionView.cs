using System.Collections.Generic;
using RedPanda.Project.Interfaces;
using TMPro;
using UnityEngine;

namespace RedPanda.Project.UI
{
    public class PromoSectionView : View
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private RectTransform content;
        [SerializeField] private float contentOffset;
        [SerializeField] private float contentSpacing;

        private readonly List<PromoElementView> _promoViews = new();
        
        public delegate void PromoClickDelegate(IPromoModel promo);
        public event PromoClickDelegate OnPromoClick;
        
        public void SetTitle(string title)
        {
            titleText.text = title;
        }

        public void SetPromos(List<IPromoModel> promos)
        {
            var horizontalOffset = contentOffset;
            var contentWidth = horizontalOffset;

            for (var i = 0; i < promos.Count; i++)
            {
                var promo = promos[i];
                
                PromoElementView promoView = UIService.CreateView<PromoElementView>();
                _promoViews.Add(promoView);
                
                promoView.SetPromo(promo);
                promoView.OnClick += () => OnPromoClick?.Invoke(promo);

                var promoViewRT = promoView.GetComponent<RectTransform>();
                var promoViewRect = promoViewRT.rect;
                promoViewRT.SetParent(content);
                
                if (i == 0)
                {
                    horizontalOffset += promoViewRect.width / 2f;
                }
                
                promoViewRT.anchoredPosition = new Vector2(horizontalOffset, 0f);

                horizontalOffset += promoViewRect.width + contentSpacing;
                contentWidth += promoViewRect.width + contentSpacing;
            }

            content.offsetMax = new Vector2(contentWidth, content.offsetMax.y);
        }

        public void CleanPromos()
        {
            foreach (PromoElementView promoView in _promoViews)
            {
                Destroy(promoView.gameObject);
            }
        }
    }
}