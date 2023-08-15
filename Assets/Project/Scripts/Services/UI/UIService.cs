using System.Collections.Generic;
using Grace.DependencyInjection;
using RedPanda.Project.Services.Interfaces;
using RedPanda.Project.UI;
using UnityEngine;

namespace RedPanda.Project.Services.UI
{
    public sealed class UIService : IUIService
    {
        private readonly IExportLocatorScope _container;
        private readonly GameObject _canvas;
        private readonly Dictionary<View, UIControl> _views = new();

        public UIService(IExportLocatorScope container)
        {
            _container = container;
            _canvas = Object.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
            _canvas.name = "Canvas";
        }

        public T CreateView<T>() where T : View
        {
            var uiControl = new UIControl(typeof(T).Name, _canvas, _container);
            var uiView = uiControl.View;
            _views.Add(uiView, uiControl);
            return (T)uiView;
        }

        public void DestroyView<T>(T view) where T : View
        {
            if (_views.TryGetValue(view, out var uiControl))
            {
                uiControl.Destroy();
            }
        }
    }
}