using RedPanda.Project.UI;

namespace RedPanda.Project.Services.Interfaces
{
    public interface IUIService
    {
        T CreateView<T>() where T : View;
        void DestroyView<T>(T view) where T : View;
    }
}