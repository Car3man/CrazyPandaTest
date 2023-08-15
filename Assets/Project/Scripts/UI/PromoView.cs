using System.Collections.Generic;
using System.Linq;
using Grace.DependencyInjection.Attributes;
using RedPanda.Project.Interfaces;
using RedPanda.Project.Services.Interfaces;
using TMPro;
using UnityEngine;

namespace RedPanda.Project.UI
{
    public class PromoView : View
    {
        [SerializeField] private TextMeshProUGUI currencyText;
        [SerializeField] private RectTransform content;
        [SerializeField] private float contentSpacing;

        private IPromoService _promoService;
        private IUserService _userService;
        private readonly List<PromoSectionView> _sectionViews = new();

        [Import]
        public void Inject(IPromoService promoService, IUserService userService)
        {
            _promoService = promoService;
            _userService = userService;
        }
        
        private void Start()
        {
            UpdateCurrencyText();
            PopulatePromos(_promoService.GetPromos());
        }

        private void OnDestroy()
        {
            CleanPromos();
        }
        
        private void PopulatePromos(IReadOnlyList<IPromoModel> promos)
        {
            var promoTypes = promos
                .Select(promo => promo.Type)
                .Distinct()
                .ToList();
            
            var verticalOffset = 0f;

            for (var i = 0; i < promoTypes.Count; i++)
            {
                var promoType = promoTypes[i];
                var promosByType = promos
                    .Where(promo => promo.Type == promoType)
                    .OrderByDescending(promo => promo.Rarity)
                    .ToList();

                PromoSectionView sectionView = UIService.CreateView<PromoSectionView>();
                _sectionViews.Add(sectionView);
                
                sectionView.SetPromos(promosByType);
                sectionView.SetTitle(promoType.ToString());
                sectionView.OnPromoClick += OnPromoClick;
                
                var sectionViewRT = sectionView.GetComponent<RectTransform>();
                sectionViewRT.SetParent(content);
                sectionViewRT.anchoredPosition = new Vector2(sectionViewRT.anchoredPosition.x, verticalOffset);
                verticalOffset -= sectionViewRT.rect.height;
                
                if (i < promoTypes.Count - 1)
                {
                    verticalOffset -= contentSpacing;
                }
            }

            content.offsetMin = new Vector2(content.offsetMin.x, verticalOffset);
        }

        private void OnPromoClick(IPromoModel promo)
        {
            if (!_userService.HasCurrency(promo.Cost))
            {
                throw new System.Exception("Not enough currency to purchase promo");
            }
            
            _userService.ReduceCurrency(promo.Cost);
            UpdateCurrencyText();
            
            Debug.Log($"Promo purchased, title: " + promo.Title);
        }

        private void CleanPromos()
        {
            foreach (PromoSectionView sectionView in _sectionViews)
            {
                sectionView.CleanPromos();
                Destroy(sectionView.gameObject);
            }
        }

        private void UpdateCurrencyText()
        {
            currencyText.text = _userService.Currency.ToString();
        }
    }
}