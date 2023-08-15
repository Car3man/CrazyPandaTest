using UnityEngine;

namespace RedPanda.Project.Services.Interfaces
{
    public interface IResourceProvider
    {
        Sprite GetIcon(string icon);
    }
}