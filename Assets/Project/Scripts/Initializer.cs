using Grace.DependencyInjection;
using RedPanda.Project.Services;
using RedPanda.Project.Services.Interfaces;
using RedPanda.Project.Services.UI;
using RedPanda.Project.UI;
using UnityEngine;

namespace RedPanda.Project
{
    public sealed class Initializer : MonoBehaviour
    {
        private readonly DependencyInjectionContainer _container = new();
        
        private void Awake()
        {
            _container.Configure(block =>
            {
                block.Export<UserService>().As<IUserService>().Lifestyle.Singleton();
                block.Export<PromoService>().As<IPromoService>().Lifestyle.Singleton();
                block.Export<UIService>().As<IUIService>().Lifestyle.Singleton();
                block.Export<ResourceProvider>().As<IResourceProvider>().Lifestyle.Singleton();
            });
            
            _container.Locate<IUIService>().CreateView<LobbyView>();
        }

        private void OnDestroy()
        {
            _container.Dispose();
        }
    }
}