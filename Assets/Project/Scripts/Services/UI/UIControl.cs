using Grace.DependencyInjection;
using RedPanda.Project.UI;
using UnityEngine;

namespace RedPanda.Project.Services.UI
{
    public class UIControl
    {
        private readonly IExportLocatorScope _exportLocatorScope;
        
        public View View { get; private set; }
        
        public UIControl(string viewName, GameObject parent, IExportLocatorScope exportLocatorScope)
        {
            _exportLocatorScope = exportLocatorScope;
            View = Object.Instantiate(Resources.Load<View>($"UI/{viewName}"), parent.transform);
            View.name = viewName;
            _exportLocatorScope.Inject(View);
        }

        public void Destroy()
        {
            Object.Destroy(View.gameObject);
        }
    }
}